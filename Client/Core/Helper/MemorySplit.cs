using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace xClient.Core.Helper
{
    public class MemorySplit
    {
        private int _maxBlocks;

        private const int MAX_PACKET_SIZE = Client.MAX_PACKET_SIZE - Client.HEADER_SIZE - (1024 * 2);
        public string Path { get; private set; }
        public string LastError { get; private set; }
        public MemoryStream MainStream { get; private set; }

        public int MaxBlocks
        {
            get
            {
                if (this._maxBlocks > 0 || this._maxBlocks == -1)
                    return this._maxBlocks;
                try
                {
                    FileInfo fInfo = new FileInfo(this.Path);

                    if (!fInfo.Exists)
                        throw new FileNotFoundException();

                    this._maxBlocks = (int)Math.Ceiling(fInfo.Length / (double)MAX_PACKET_SIZE);
                }
                catch (UnauthorizedAccessException)
                {
                    this._maxBlocks = -1;
                    this.LastError = "Access denied";
                }
                catch (IOException)
                {
                    this._maxBlocks = -1;
                    this.LastError = "File not found";
                }

                return this._maxBlocks;
            }
        }

        public MemorySplit(string path)
        {
            this.Path = path;
        }

        private int GetSize(long length)
        {
            return (length < MAX_PACKET_SIZE) ? (int)length : MAX_PACKET_SIZE;
        }

        public bool ReadBlock(int blockNumber, out byte[] readBytes)
        {
            try
            {
                if (blockNumber > this.MaxBlocks)
                    throw new ArgumentOutOfRangeException();

                using (FileStream fStream = File.OpenRead(this.Path))
                {
                    if (blockNumber == 0)
                    {
                        fStream.Seek(0, SeekOrigin.Begin);
                        readBytes = new byte[this.GetSize(fStream.Length - fStream.Position)];
                        fStream.Read(readBytes, 0, readBytes.Length);
                    }
                    else
                    {
                        fStream.Seek(blockNumber * MAX_PACKET_SIZE, SeekOrigin.Begin);
                        readBytes = new byte[this.GetSize(fStream.Length - fStream.Position)];
                        fStream.Read(readBytes, 0, readBytes.Length);
                    }
                }

                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                readBytes = new byte[0];
                this.LastError = "BlockNumber bigger than MaxBlocks";
            }
            catch (UnauthorizedAccessException)
            {
                readBytes = new byte[0];
                this.LastError = "Access denied";
            }
            catch (IOException)
            {
                readBytes = new byte[0];
                this.LastError = "File not found";
            }

            return false;
        }

        public bool AppendBlock(byte[] block, int blockNumber)
        {
            try
            {
                if (!File.Exists(this.Path) && blockNumber > 0)
                    throw new FileNotFoundException();

                if (blockNumber == 0)
                {
                    this.MainStream = new MemoryStream();
                    MainStream.Seek(0, SeekOrigin.Begin);
                    MainStream.Write(block, 0, block.Length);

                    return true;
                }
                MainStream.Seek(blockNumber * MAX_PACKET_SIZE, SeekOrigin.Begin);
                MainStream.Write(block, 0, block.Length);

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                this.LastError = "Access denied";
            }
            catch (IOException)
            {
                this.LastError = "File not found";
            }

            return false;
        }

        public bool DropFile()
        {
            try
            {
                File.WriteAllBytes(this.Path, ToByteArray());
                MainStream.Close();
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                this.LastError = "Access denied";
            }
            catch (IOException)
            {
                this.LastError = "File not found";
            }
            return false;
        }
        public byte[] ToByteArray()
        {
            try
            {
                return MainStream.ToArray();
            }
            catch (Exception)
            {

            }
            return null;
        }

    }
}
