using System;
using System.Net.Sockets;
using System.Text;

namespace ClientApp
{
    class Program
    {
        static void ConnectServer(String server, int port)
        {
            try
            {
                // Create a TcpClient
                TcpClient client = new TcpClient(server, port);
                Console.Title = "Client Application";
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    Console.Write("Input message <press Enter to exit>: ");
                    string message = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(message))
                    {
                        break;
                    }

                    // Translate the passed message into ASCII and store it as a byte array
                    byte[] data = Encoding.ASCII.GetBytes(message);

                    // Send the message to the connected TcpServer
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Sent: {0}", message);

                    // Receive the TcpServer response
                    data = new byte[256];
                    int bytes = stream.Read(data, 0, data.Length);
                    string responseData = Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Received: {0}", responseData);
                }

                // Shutdown and end connection
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
        }

        static void Main(string[] args)
        {
            string server = "127.0.0.1";
            int port = 13000;
            ConnectServer(server, port);
        }
    }
}
