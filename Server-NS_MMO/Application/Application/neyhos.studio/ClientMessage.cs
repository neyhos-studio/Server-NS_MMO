using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.neyhos.studio
{
    class ClientMessage
    {
        public string id { get; }
        public string action { get; }
        public string data { get; }

        public ClientMessage(string clientMessage)
        {
            string[] message = clientMessage.Split('|');
            this.id = message[0];
            this.action = message[1];
            this.data = message[2];
        }
    }
}
