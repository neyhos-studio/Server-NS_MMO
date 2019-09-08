using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Application.neyhos.studio
{
    class Action
    {

        /// <summary> 	
        /// Send message to client using socket connection. 	
        /// </summary> 	
        private void SendMessage(string serverMessage)
        {
            if (Program.connectedTcpClient == null)
            {
                return;
            }

            try
            {
                // Get a stream object for writing. 			
                NetworkStream stream = Program.connectedTcpClient.GetStream();
                if (stream.CanWrite)
                {
                    // Convert string message to byte array.                 
                    byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
                    // Write byte array to socketConnection stream.               
                    stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                    Console.WriteLine("Server sent his message - should be received by client");
                }
            }
            catch (SocketException socketException)
            {
                Console.WriteLine("Socket exception: " + socketException);
            }
        }

        public void UnknowAction(ClientMessage msg)
        {
            string msgToSend = String.Format("ERROR|UNKNOW_ACTION;{0}", msg.action);
            SendMessage(msgToSend);
        }

        public void ClientConnect(ClientMessage msg)
        {

        }
    }
}
