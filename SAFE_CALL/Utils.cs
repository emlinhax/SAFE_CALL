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
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool IsDebuggerPresent();

        public static bool Isx64() { return IntPtr.Size == 8; }

        public static MethodInfo FindCorrectMethod(Type _class, string name, object[] args = null)
        {
            foreach (MethodInfo method in _class.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            {
                try
                {
                    if (method.Name == name)
                    {
                        if (args == null)
                            return method;

                        bool invalid = false;
                        for (int i = method.IsStatic ? 0 : 1; i < args.Length; i++)   //Cycle through all arguments and see if they match up
                        {
                            if (method.GetParameters()[i].ParameterType != args[i].GetType())
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
    }
}
