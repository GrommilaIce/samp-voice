using ProcessMemoryReaderLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    struct Position
    {
        public int baseAdr;
        public int[] X, Y, Z;
    }

    class MemoryWork
    {       
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize,
            out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

        

        private static int ProcessId;
        private static ProcessModule module;

        public static void GetProcess(string pName)
        {
            var pList = Process.GetProcesses();
            foreach (var i in pList)
            { 
                if (i.ProcessName == pName)
                { 
                    ProcessId = i.Id;
                    module = i.MainModule;
                }
            }
        }

        public static void WriteByteMoney(int money)
        {
            var moneyAdress = 0xB7CE50;
            byte[] bMoney = BitConverter.GetBytes(money);

            var size = 4;
            var dummu = new IntPtr();
            var handle = OpenProcess(0x001F0FFF, false, ProcessId);

            WriteProcessMemory(handle, (IntPtr)moneyAdress, bMoney, size, out dummu);

            CloseHandle(handle);
        }

        public static byte[] ReadByteMoney(int size)
        {
            byte[] buffer = new byte[size];
            int[] x = { 0x14, 0x30 };
            IntPtr IP = IntPtr.Zero;
            IntPtr handle = OpenProcess(0x098A9768, false, ProcessId);
            ReadProcessMemory(handle, (IntPtr)0xB7CE50, buffer, size, out IP);
            return buffer; 
        }

        public static float[] GetPosition()
        {
            Position sPos = new Position()
            {
                baseAdr = 0xB6F5F0,
                X = new int[] { 0x14, 0x30 },                
                Y = new int[] { 0x14, 0x34 },
                Z = new int[] { 0x14, 0x38 }
            };
            ProcessMemoryReader rMem = new ProcessMemoryReader();
            rMem.ReadProcess = Process.GetProcessById(ProcessId);
            rMem.OpenProcess();
            
            int XAddress = rMem.ReadMultiLevelPointer(sPos.baseAdr, 4, sPos.X);
            int YAddress = rMem.ReadMultiLevelPointer(sPos.baseAdr, 4, sPos.Y);
            int ZAddress = rMem.ReadMultiLevelPointer(sPos.baseAdr, 4, sPos.Z);

            float[] pos = new float[]
            {
                rMem.ReadFloat(XAddress),
                rMem.ReadFloat(YAddress),
                rMem.ReadFloat(ZAddress)
            };

            return pos;
        }

        public static bool GetSphere(float x, float y, float z)
        {
            int r = 10;
            if (x * x + y * y + z * z <= r * r)
                return true;
            return false;
        }
    }
}
