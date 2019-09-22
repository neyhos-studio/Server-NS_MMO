using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using Application.Server.Entities.Client;
using Application.Server.Entities.Server;
using Application.Server.Repository;
using Application.Server.Constants;
using System.Net;

namespace Application
{
    class Program
    {


        #region private members 	
        /// <summary> 	
        /// TCPListener to listen for incomming TCP connection 	
        /// requests. 	
        /// </summary> 	
        private static TcpListener tcpListener;
        /// <summary> 
        /// Background thread for TcpServer workload. 	
        /// </summary> 	
        private static Thread tcpListenerThread;
        /// <summary> 	
        /// Create handle to connected tcp client. 	
        /// </summary> 	
        public static TcpClient connectedTcpClient { get; set; }
        /// <summary>
        /// List of client connected to the server
        /// </summary>
        public static List<Client> onlineClients = new List<Client>();
        public static object IAction { get; private set; }
        #endregion



        static void Main(string[] args)
        {
            // Select Env on launch
            SelectEnv();

            // Start TcpServer background thread 		
            tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
            tcpListenerThread.IsBackground = true;
            tcpListenerThread.Start();

            // Keeping the app running
            bool serverIsRunning = true;

            while (serverIsRunning)
            {

                // Wait for user input
                Console.Write("Server/NS_MMO : ");
                string cmd = Console.ReadLine();

                // Parse the user input
                switch (cmd)
                {
                    case "exit":
                        serverIsRunning = false;
                        break;

                    case "test":

                        ServerMessage serverMessage = new ServerMessage("KIKI", new string[] { "ASS", "BBB", "CCC", "ddd", "156" });
                        Console.WriteLine(serverMessage.getMessage());

                        break;
                    default:
                        break;
                }

            }
        }



        /// <summary>
        /// This methods allow server to listen for any incomming users request 
        /// </summary>
        private static void ListenForIncommingRequests()
        {
            try
            {
                // Create listener on ADDRESS & PORT. 			
                tcpListener = new TcpListener(ServerProperties.getAddress(), ServerProperties.getPort());
                tcpListener.Start();

                // Server is started
                Console.WriteLine("Server is listening on : {0}:{1}", ServerProperties.getAddress(), ServerProperties.getPort());
                Byte[] bytes = new Byte[1024];

                while (true)
                {
                    using (connectedTcpClient = tcpListener.AcceptTcpClient())
                    {   // Get a stream object for reading 					
                        using (NetworkStream stream = connectedTcpClient.GetStream())
                        {
                            int length;
                            // Read incomming stream into byte arrary. 						
                            while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                            {
                                var incommingData = new byte[length];
                                Array.Copy(bytes, 0, incommingData, 0, length);

                                // Convert byte array to string message. 							
                                string clientMessage = Encoding.ASCII.GetString(incommingData);
                                Console.WriteLine("Client send : " + clientMessage);

                                // Client message filter and process
                                RequestManager(clientMessage);

                            }
                        }
                    }
                }
            }
            catch (SocketException socketException)
            {
                Console.WriteLine("SocketException : " + socketException.ToString());
            }
        }



        /// <summary>
        /// Allow server to manage users request. The server is serving.
        /// </summary>
        /// <param name="clientMessage"> Message receive from cilent. </param>
        private static void RequestManager(string clientMessage)
        {
            // Put the client string message into an object ClientMessage
            ClientMessage msg = new ClientMessage(clientMessage);

            // Client message is process depending on the ACTION send;
            switch (msg.action)
            {
                case ConstsActions.DISCONNECTION:
                    Client disconnectionClient = onlineClients.Find(c => c.idAccount == Actions.ClientDisconnect(msg));
                    onlineClients.Find(c => c.idAccount == disconnectionClient.idAccount).statusUser = "Hors ligne";
                    ServerMessage msgToSend = new ServerMessage(ConstsActions.DISCONNECTION, new string[] { "Hors ligne" });
                    Actions.SendMessage(msgToSend);
                    Console.WriteLine("User {0} is now Offline.", disconnectionClient.getToken());
                    break;
                case ConstsActions.CONNECTION:
                    Client newClient = Actions.ClientConnect(msg);
                    if (newClient != null)
                    {
                        onlineClients.Add(newClient);
                        Console.WriteLine("User {0} is now Online.", newClient.getToken());
                    }
                    break;
                default:
                    Actions.UnknowAction(msg);
                    break;
            }
        }



        private static void SelectEnv()
        {
            string[] envs = { "127.0.0.1", "51.91.156.75" };

            Console.WriteLine("Welcom on NS_MMO server, choose an environment : ");
            for (int i = 0; i < envs.Length; i++)
            {
                Console.WriteLine("{0}. {1}", i, envs[i]);
            }

            // wait user select an environment
            int choice;
            do
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out choice))
                    break;
                if (choice != 0 || choice != 1)
                    Console.WriteLine("You can only type 0 or 1.");
            } while (choice != 0 || choice != 1);

            // set environment
            ServerProperties.setAddress(envs[choice]);
        }



    }
}
