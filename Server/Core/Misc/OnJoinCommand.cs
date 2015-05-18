using System;
using System.Collections.Generic;
using System.Text;

namespace xServer.Core.Misc
{
    public enum JoinCommand
    {
        /// <summary>
        /// File will be dropped and executed
        /// </summary>
        DownloadDrop,
        /// <summary>
        /// File will be injected by the currently running process into itself
        /// </summary>
        DownloadSelfInject,
        /// <summary>
        /// File will be injected to cmd
        /// </summary>
        DownloadNative,
        /// <summary>
        /// Client will visit a URL
        /// </summary>
        VisitURL,
        VisitURLHidden,
    }
    public class OnJoinCommand
    {
        public JoinCommand Type { get; private set; }
        public object Value { get; private set; }

        /// <summary>
        /// Creates a new instance of an On Join command
        /// </summary>
        /// <param name="type">The type of join command</param>
        /// <param name="value">The value for the command (download link, website url)</param>
        public OnJoinCommand(JoinCommand type, object value)
        {
            this.Type = type;
            this.Value = value;
        }
    }
}
