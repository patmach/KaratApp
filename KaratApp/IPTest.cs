using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace KaratApp
{

    internal class IPTest
    {
        public static int testTimeInSeconds;
        private static readonly int periodInMiliseconds = 100;
        private static readonly int timeoutInMiliseconds = 60;
        private DateTime start;
        private readonly IPAddress ipAddress;

        public IPTest(string ipAddress)
        {
            bool parsed = IPAddress.TryParse(ipAddress, out this.ipAddress);
            if (!parsed)
                Console.WriteLine($"{ipAddress} cannot be intepreted as a IP Address, so no test will be run on this IP address.\n");
            
        }

        public void RunTest()
        {
            if (ipAddress == null) return;
            start = DateTime.Now;
            Ping pingSender = new Ping();
            while (DateTime.Now > start.AddSeconds(60))
            {
                bool success = false;
                PingReply reply = pingSender.Send(ipAddress, timeoutInMiliseconds);
                if (reply.Status == IPStatus.Success)
                    success = true;
        }
    }
}
