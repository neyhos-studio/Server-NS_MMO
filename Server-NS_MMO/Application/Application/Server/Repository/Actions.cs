using System;
using System.Net.Sockets;
using System.Text;
using Application.Server.Entities.Authentication;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Server.Mapper;
using System.Net;
using Application.Server.Constants;
using Application.Server.Entities.Client;
using Application.Server.Entities.Server;

namespace Application.Server.Repository
{
    class Actions
    {



        #region NOT ACTIONS
        // This region contains everythings that not ACTION that the server can receive.


        /// <summary>
        /// Send message to client using socket connection. 	
        /// </summary>
        /// <param name="serverMessage">A server message to send to the client</param>
        private static void SendMessage(ServerMessage serverMessage)
        {
            try
            {
                // Get a stream object for writing. 			
                NetworkStream stream = Program.connectedTcpClient.GetStream();
                if (stream.CanWrite)
                {
                    // Convert string message to byte array.                 
                    byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage.getMessage());
                    // Write byte array to socketConnection stream.               
                    stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                }
            }
            catch (SocketException socketException)
            {
                Console.WriteLine("Socket exception: " + socketException);
            }
        }



        /// <summary>
        /// Call the API using await async
        /// </summary>
        /// <param name="apiUrl">The api url you want to POST or GET</param>
        /// <param name="output">The string of the object as JSON format</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> CallApiAsync(string apiUrl, string output)
        {
            // Prepare the response of the API
            HttpResponseMessage response = null;

            using (var client = new HttpClient())
            {
                // Await for API response
                response = await client.PostAsync(apiUrl, new StringContent(output, Encoding.UTF8, "application/json"));
            }


            return response;
        }



        #endregion



        #region ACTIONS
        // This region contains ACTIONS that the server have to process when a client is asking for.


        /// <summary>
        /// Default unknow action
        /// </summary>
        /// <param name="msg">The client message</param>
        public static void UnknowAction(ClientMessage msg)
        {
            // Prepare the server message to return
            ServerMessage serverMessage = new ServerMessage();
            serverMessage.action = ConstsActions.UNKNOW_ACTION;
            serverMessage.data = new string[] { msg.action };

            // Send the message
            SendMessage(serverMessage);
        }



        /// <summary>
        /// Case when a client try to connect using his credentials
        /// </summary>
        /// <param name="msg">A client message</param>
        /// <returns></returns>
        public static Client ClientConnect(ClientMessage msg)
        {
            // TODO demock ip address

            // Get user credentials
            string[] credentials = msg.data;
            Credentials userCredentials = new Credentials(credentials[0], credentials[1], new IPAddress(new byte[] { 10, 10, 10, 10 }));

            // Call API to try log in to the game
            string apiUrl = "http://51.91.156.75:5000/api/Connexion/Connexion";
            string ouput = userCredentials.ToString();
            HttpResponseMessage response = CallApiAsync(apiUrl, ouput).Result;

            // If we get a good response we send SUCCESS to the user, else we send FAIL ...
            if (response.IsSuccessStatusCode)
            {
                // Map the response from HttpResponseMessage to Client 
                Client client = ClientMapper.mapAsync(response).Result;

                // Set the ip of the client to the object client
                client.idAddress = userCredentials.ip;

                // Prepare the return message of the server to the client 
                ServerMessage succes = new ServerMessage();
                succes.action = ConstsActions.CONNECTION;
                succes.data = new string[] { "SUCCESS", client.getToken() };

                // Send the message
                SendMessage(succes);

                // Return the client to add him in the client list
                return client;
            }
            else
            {
                // Prepare the return message of the server to the client 
                ServerMessage fail = new ServerMessage();
                fail.action = ConstsActions.CONNECTION;
                fail.data = new string[] { "FAIL", response.StatusCode.ToString() };

                // Send the message
                SendMessage(fail);

                return null;
            }

        }



        #endregion


    }
}
