using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        static int port = 8001; // порт сервера
        static string address = "127.0.0.1";

        public Form1()
        {
            InitializeComponent();
            MemoryWork.GetProcess("gta_sa");
            timer1.Tick += Timer1_Tick;
            timer1.Start();
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            byte[] data = Encoding.Unicode.GetBytes("Мы не одни во вселенной!");
            socket.Send(data);

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            float[] position = MemoryWork.GetPosition();
            tbPosX.Text = position[0].ToString();
            tbPosY.Text = position[1].ToString();
            tbPosZ.Text = position[2].ToString();
        }
    }
}
