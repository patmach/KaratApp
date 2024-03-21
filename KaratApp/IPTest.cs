using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Timers;

namespace KaratApp
{

    internal sealed class IPTest
    {
        private ManualResetEventSlim stopTimerResetEvent = new ManualResetEventSlim(false);
        public static int testTimeInSeconds;
        private static readonly int periodInMiliseconds = 100;
        private static readonly int timeoutInMiliseconds = 300;
        private DateTime start;
        private readonly IPAddress ipAddress;
        private System.Timers.Timer timer = new System.Timers.Timer(periodInMiliseconds);

        internal IPTest(string ipAddress)
        {
            bool parsed = IPAddress.TryParse(ipAddress, out this.ipAddress);
            if (!parsed)
                Console.WriteLine($"{ipAddress} cannot be intepreted as a IP Address, so no test will be run on this IP address.\n");
            
        }

        public void RunTests()
        {
            if (ipAddress == null)
                return;
            start = DateTime.Now;
            timer.Elapsed += async (sender, e) => await Test();
            timer.Start();
            stopTimerResetEvent.Wait();
            stopTimerResetEvent.Dispose();
            timer.Stop();
            return;
        }


        private Task Test()
        {
            if (DateTime.Now > start.AddSeconds(testTimeInSeconds))
                stopTimerResetEvent.Set();
            else
                PingIPAddress();
            return Task.CompletedTask;

        }

        private void PingIPAddress()
        {
            bool success = false;
            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send(ipAddress, timeoutInMiliseconds);
            if (reply.Status == IPStatus.Success)
                success = true;
            Console.WriteLine($"{DateTime.Now.ToString("hh.mm.ss.ffffff")}.....{ipAddress}: {success}");
        }
    }
}
