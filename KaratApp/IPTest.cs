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

    internal class IPTest
    {
        private ManualResetEvent stopTimerResetEvent = new ManualResetEvent(false);
        public static int testTimeInSeconds;
        private static readonly int periodInMiliseconds = 100;
        private static readonly int timeoutInMiliseconds = 300;
        private DateTime start;
        private readonly IPAddress ipAddress;
        private System.Timers.Timer timer = new System.Timers.Timer(periodInMiliseconds);

        public IPTest(string ipAddress)
        {
            bool parsed = IPAddress.TryParse(ipAddress, out this.ipAddress);
            if (!parsed)
                Console.WriteLine($"{ipAddress} cannot be intepreted as a IP Address, so no test will be run on this IP address.\n");
            
        }

        public Task RunTests()
        {
            if (ipAddress == null) 
                return Task.CompletedTask;
            start = DateTime.Now;
            Ping pingSender = new Ping();
            timer.Elapsed += async (sender, e) => await Test(pingSender);
            timer.Start();
            stopTimerResetEvent.WaitOne();
            stopTimerResetEvent.Close();
            timer.Stop();
            return Task.CompletedTask;
        }


        private Task Test(Ping pingSender)
        {
            if (DateTime.Now > start.AddSeconds(testTimeInSeconds))
                stopTimerResetEvent.Set();
            else
                Ping(pingSender);
            return Task.CompletedTask;

        }

        private void Ping(Ping pingSender)
        {
            var lastteststart = DateTime.Now;
            bool success = false;
            PingReply reply = pingSender.Send(ipAddress, timeoutInMiliseconds);
            if (reply.Status == IPStatus.Success)
                success = true;

        }
    }
}
