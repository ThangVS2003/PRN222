using System;
using System.Net.Sockets;
using System.Text;

namespace ClientApp
{
    class Program
    {
        static void ConnectServer(string server, int port)
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
                        break; // Exit if input is empty
                    }

                    // Translate the passed message into ASCII and store it as a byte array
                    byte[] data = Encoding.ASCII.GetBytes(message);

                    // Send the message to the connected TcpServer
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine($"Sent: {message}");

                    // Receive the TcpServer response
                    data = new byte[256];
                    int bytes = stream.Read(data, 0, data.Length);
                    string responseData = Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine($"Received: {responseData}");
                }

                // Shutdown and end connection
                Console.WriteLine("Closing connection...");
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }
        }

        static void Main(string[] args)
        {
            string server = "127.0.0.1"; // Server IP (localhost)
            int port = 8080;  
            ConnectServer(server, port);
        }
    }
}
