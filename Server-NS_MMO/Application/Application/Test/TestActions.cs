using Application.Server.Entities.Message;
using Application.Server.Entities.Message;

namespace Application.Test
{
    class TestActions
    {

        /// <summary> 	
        /// Send message to client using socket connection. 	
        /// </summary> 	
        private static void SendMessage(SendMessage serverMessage)
        {

        }

        public static void UnknowAction(ReceivedMessage msg)
        {
            SendMessage serverMessage = new SendMessage();
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
