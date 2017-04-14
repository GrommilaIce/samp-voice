using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server_Voice_Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.StartServer();
            server.ListenServer();
            string str = new WebClient().DownloadString("http://icanhazip.com").ToString().Remove(14);
            Console.WriteLine(str);
            Console.ReadKey();
        }
        
    }
}
