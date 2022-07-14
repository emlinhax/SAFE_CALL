using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SAFE_CALL
{
    public static class Utils
    {
        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }
        
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool IsDebuggerPresent();

        [DllImport("kernel32")]
        public static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memset(IntPtr dest, int c, int byteCount);

        public static bool Isx64() { return IntPtr.Size == 8; }

        public static MethodInfo FindCorrectMethod(Type _class, string name, object[] args = null)
        {
            foreach (MethodInfo method in _class.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            {
                try
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (method.Name == name && args.Length == parameters.Length)
                    {
                        if (args == null || parameters == null)
                            return method;
                        
                        bool invalid = false;
                        for (int i = 0; i < args.Length; i++)   //Cycle through all arguments and see if they match up
                        {
                            if (args[i] == null)
                                continue;

                            if (parameters[i].ParameterType != args[i].GetType())
                            {
                                invalid = true;
                                break;
                            }
                        }
                        if (!invalid)
                            return method;
                    }
                }
                catch
                {
                    continue;
                }
            }

            return null;
        }

        public static void CopyFromPtrToPtr(IntPtr src, uint srcLen, IntPtr dst, uint dstLen)
        {
            var buffer = new byte[srcLen];
            Marshal.Copy(src, buffer, 0, buffer.Length);
            Marshal.Copy(buffer, 0, dst, (int)Math.Min(buffer.Length, dstLen));
        }
    }
}
