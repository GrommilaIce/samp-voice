using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server_Voice_Chat
{
    class Server
    {
        private int port = 8001;
        private Socket listenSocket;
        IPEndPoint ip;

        public Server()
        {
            ip = new IPEndPoint(IPAddress.Any, port);
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void StartServer()
        {
            try
            {
                listenSocket.Bind(ip);
                listenSocket.Listen(500);
                Console.WriteLine("Сервер запущен. Ожидание подключений...");
            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void ListenServer()
        {
            while (true)
            {
                Socket handler = listenSocket.Accept();
                Console.WriteLine("Новое подключение: " + handler.AddressFamily);
                // получаем сообщение
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байтов
                byte[] data = new byte[1028]; // буфер для получаемых данных

                do
                {
                    bytes = handler.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (handler.Available > 0);

                Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                // закрываем сокет
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
    }
}
