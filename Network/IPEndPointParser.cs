using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace CheatGameModel.Network
{
    public static class IPEndPointParser
    {
        public static IPEndPoint Parse(string s)
        {
            string[] values = s.Split(':');
            IPEndPoint result = new IPEndPoint(IPAddress.Parse(values[0]), int.Parse(values[1]));
            return result;
        }
    }
}
