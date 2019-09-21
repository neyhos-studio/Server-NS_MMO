using System;
using System.Net.Sockets;
using System.Text;
using Application.Server.Entities;
using Application.Entities;

namespace Application.Test
{
    class TestActions
    {
        
        /// <summary> 	
        /// Send message to client using socket connection. 	
        /// </summary> 	
        private static void SendMessage(ServerMessage serverMessage)
        {
           
        }

        public static void UnknowAction(ClientMessage msg)
        {
            ServerMessage serverMessage = new ServerMessage();
                    serverMessage.action = "UNKNOW_ACTION";
                    serverMessage.data = new string[] { msg.action };

            SendMessage(serverMessage);
        }

        public static Client ClientConnect()
        {
            return null;
        }
    }
}
