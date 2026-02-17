using System.Net;
using System.Net.Sockets;
using System.Text;
using BuditelWebServer.Server.HTTP;
using static BuditelWebServer.Server.HTTP.IRoutingTables;

namespace BuditelWebServer.Server
{
    public class HttpServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener serverListener;

        // Това липсваше:
        private readonly RoutingTable routingTable;

        public HttpServer(string ipAddress, int port, Action<IRoutingTable> routingTableConfiguration)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.port = port;
            this.serverListener = new TcpListener(this.ipAddress, this.port);

            this.routingTable = new RoutingTable();
            routingTableConfiguration(this.routingTable);
        }

        // Допълнителен конструктор за по-лесно ползване
        public HttpServer(int port, Action<IRoutingTable> routingTableConfiguration)
            : this("127.0.0.1", port, routingTableConfiguration)
        {
        }

        public void Start()
        {
            this.serverListener.Start();

            Console.WriteLine($"Server started on port {port}...");
            Console.WriteLine("Listening for requests...");

            while (true)
            {
                var connection = serverListener.AcceptTcpClient();
                var networkStream = connection.GetStream();

                var requestText = this.ReadRequest(networkStream);

                // Тук беше старата логика. Ето новата:
                var request = Request.Parse(requestText);
                var response = this.routingTable.MatchRequest(request);

                // Изпълнение на Pre-Render Action (ако има)
                if (response.PreRenderAction != null)
                {
                    response.PreRenderAction(request, response);
                }

                this.WriteResponse(networkStream, response);

                connection.Close();
            }
        }

        private string ReadRequest(NetworkStream networkStream)
        {
            var bufferLength = 1024;
            var buffer = new byte[bufferLength];
            var requestBuilder = new StringBuilder();
            int totalBytes = 0;

            do
            {
                var bytesRead = networkStream.Read(buffer, 0, bufferLength);
                totalBytes += bytesRead;
                if (totalBytes > 10 * 1024) throw new InvalidOperationException("Request too large");
                requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }
            while (networkStream.DataAvailable);

            return requestBuilder.ToString();
        }

        private void WriteResponse(NetworkStream networkStream, Response response)
        {
            var responseBytes = Encoding.UTF8.GetBytes(response.ToString());
            networkStream.Write(responseBytes);
        }
    }
}