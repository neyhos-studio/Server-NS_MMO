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
        public static IPAddress SERVER_ADDRESS = IPAddress.Parse("51.91.156.75");
        public static int SERVER_PORT = 5757;
        public static int SERVER_TIMEOUT = 10_000;
    }
}
