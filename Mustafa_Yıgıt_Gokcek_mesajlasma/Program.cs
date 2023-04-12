using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MesajlasmaUygulamasi
{
    class Program
    {
        static TcpListener serverSocket = new TcpListener(IPAddress.Any, 8888);
        static void Main(string[] args)
        {
            serverSocket.Start();
            Console.WriteLine("Sunucu Başlatıldı.");
            while (true)
            {
                TcpClient clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine("Yeni Bağlantı Kabul Edildi.");
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.Start(clientSocket);
            }
        }

        static void HandleClient(object client)
        {
            TcpClient clientSocket = (TcpClient)client;
            byte[] bytesFrom = new byte[65536];
            string dataFromClient = null;
            while (true)
            {
                try
                {
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    Console.WriteLine("Alınan mesaj: " + dataFromClient);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    break;
                }
            }
        }
    }


    namespace MesajlasmaUygulamasiIstemci
    {
        class Program
        {
            static void Main(string[] args)
            {
                TcpClient clientSocket = new TcpClient("127.0.0.1", 8888);
                Console.WriteLine("Bağlantı kuruldu.");
                while (true)
                {
                    Console.Write("Mesajınızı girin: ");
                    string message = Console.ReadLine();
                    NetworkStream serverStream = clientSocket.GetStream();
                    byte[] outStream = Encoding.ASCII.GetBytes(message + "$");
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();
                }
            }
        }
    }

}
