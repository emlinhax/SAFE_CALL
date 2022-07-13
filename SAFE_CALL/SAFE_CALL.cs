using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SAFE_CALL
{
    public unsafe static class SC
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool IsDebuggerPresent();

        public static bool Isx64() { return IntPtr.Size == 8; }

        public static object SAFE_CALL(Type t, string name, params object[] args)
        {
            return SAFE_CALL(null, t, name, args);
        }

        public static object SAFE_CALL(object instance, Type t, string name, params object[] args)
        {
            MethodInfo method = Utils.FindCorrectMethod(t, name, args);
            if (method == null)
                throw new Exception("Could not find " + t.Name + " | " + name);

            //if (IsHooked_IsDebuggerPresent())
            //    return null;

            if (!IsMethodSafe(method))
                return null;

            object result = method.Invoke(instance, args);
            return result;
        }

        public static bool IsMethodSafe(MethodInfo method)
        {
            IntPtr mptr = method.MethodHandle.GetFunctionPointer();
            if (mptr == null)
                return false;

            byte* actualPtr = (byte*)mptr.ToPointer();

            byte firstByte = *actualPtr;
            byte secondByte = *(actualPtr + 1);

            if (Isx64())
            {
                if (firstByte != 0x56 || secondByte != 0x48)
                    return false;
            }
            else
            {
                if (firstByte != 0x55 || secondByte != 0x8B)
                    return false;
            }

            return true;
        }

        static bool IsHooked_IsDebuggerPresent()
        {
            IntPtr kernel32 = LoadLibrary("kernel32.dll");
            IntPtr IsDebuggerPresentAddr = GetProcAddress(kernel32, "IsDebuggerPresent");

            byte[] data = new byte[2];
            Marshal.Copy(IsDebuggerPresentAddr, data, 0, 2);

            if (Environment.Is64BitProcess)
            {
                if ((data[0] != 0x48 || data[1] != 0xFF))
                    return true;
            }
            else
            {
                if ((data[0] != 0xFF || data[1] != 0x25))
                    return true;
            }

            return IsDebuggerPresent();
        }
    }
}
