using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SimbaPlugin
{
    public class Class1
    {
        [DllExport]
        static int GetPluginABIVersion() { return 2; }
        [DllExport]
        static int GetFunctionCount() { return 2; }
        [DllExport]
        static int GetTypeCount() { return 0;  }

        //int GetFunctionInfo(int Index, void** Address, char** Definition)
        //int GetTypeInfo(int Index, char** Type, char** Definition)
        //void OnAttach(void *info)
        //void OnDetach()

        static string[] functionInfo =
        {
            "AFunc", "procedure AFunc;",
            "Return0", "function Return0: Integer;"
        };

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        // int GetFunctionInfo(int, void **, char **);
        [DllExport]
        static int GetFunctionInfo(int index, IntPtr address, IntPtr definition)
        {
            IntPtr addr = GetProcAddress(GetModuleHandle("SimbaPlugin"), functionInfo[index * 2]);
            Marshal.WriteIntPtr(address, addr);

            // TODO: Cleaner way?
            IntPtr[] defin = { IntPtr.Zero };
            Marshal.Copy(definition, defin, 0, 1);
            
            byte[] raw = Encoding.ASCII.GetBytes(functionInfo[index * 2 + 1]);
            Marshal.Copy(raw, 0, defin[0], raw.Length);
            return index;
        }

        [DllExport]
        static int GetTypeInfo(int index, IntPtr type, IntPtr definition) { return index; }

        [DllExport("OnAttach", CallingConvention.Cdecl)]
        static void OnAttach(IntPtr info) { }

        [DllExport("OnDetach", CallingConvention.Cdecl)]
        static void OnDetach() { }

        [DllExport("AFunc", CallingConvention.Cdecl)]
        static void AFunc() { }

        [DllExport("Return0", CallingConvention.Cdecl)]
        static int Return0() { return 0; }
    }
}
