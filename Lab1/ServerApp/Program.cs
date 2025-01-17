using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerApp
{
    class Program
    {
        static void ProcessMessage(object parm)
        {
            try
            {
                TcpClient client = parm as TcpClient;
                if (client == null) return;

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                string data;

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                // Log the client IP
                Console.WriteLine($"Client connected from IP: {((IPEndPoint)client.Client.RemoteEndPoint).Address}");

                // Loop to receive all the data sent by the client
                while (true)
                {
                    int count = stream.Read(bytes, 0, bytes.Length);
                    if (count == 0) break; // Client disconnected

                    // Translate data bytes to a ASCII string
                    data = Encoding.ASCII.GetString(bytes, 0, count);
                    Console.WriteLine($"Received: {data} at {DateTime.Now:t}");

                    // Process the data sent by the client (convert to uppercase)
                    string response = data.ToUpper();
                    byte[] msg = Encoding.ASCII.GetBytes(response);

                    // Send back a response
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine($"Sent: {response}");
                }

                // Close client connection
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ExecuteServer(string host, int port)
        {
            TcpListener server = null;
            try
            {
                Console.Title = "Server Application";
                IPAddress localAddr = IPAddress.Parse(host);
                server = new TcpListener(localAddr, port);

                server.Start();
                Console.WriteLine("**************************************");
                Console.WriteLine("Waiting for a connection...");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Client connected.");

                    // Create a thread to process each client's message
                    Thread thread = new Thread(new ParameterizedThreadStart(ProcessMessage));
                    thread.Start(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server Error: {ex.Message}");
            }
            finally
            {
                server?.Stop();
                Console.WriteLine("Server stopped.");
            }
        }

        static void Main(string[] args)
        {
            string host = "127.0.0.1"; // Localhost
            int port = 8080;  // Port 8080
            ExecuteServer(host, port);
        }
    }
}
