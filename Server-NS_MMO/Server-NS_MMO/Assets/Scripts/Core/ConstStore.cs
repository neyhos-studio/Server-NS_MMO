using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core
{
    class ConstStore
    {
        public static IPAddress SERVER_ADDRESS = IPAddress.Parse("127.0.0.1");
        public static int SERVER_PORT = 8888;
        public static int SERVER_TIMEOUT = 10_000;
    }
}
