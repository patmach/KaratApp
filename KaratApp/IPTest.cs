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
    /// <summary>
    /// Class that takes care of initialization and run of tests for one IP address 
    /// </summary>
    internal sealed class IPTest
    {
        /// <summary>
        /// Reset event that should be set when the test time is up
        /// </summary>
        private ManualResetEventSlim stopTimerResetEvent = new ManualResetEventSlim(false);

        /// <summary>
        /// How long the test will run
        /// </summary>
        public static int testTimeInSeconds;

        /// <summary>
        /// How often the request should be sent
        /// </summary>
        private static readonly int periodInMiliseconds = 100;

        /// <summary>
        /// Maximum time to wait for response
        /// </summary>
        private static readonly int timeoutInMiliseconds = 300;

        /// <summary>
        /// Start of the test
        /// </summary>
        private DateTime start;

        /// <summary>
        /// Tested IP address
        /// </summary>
        private readonly IPAddress ipAddress;

        /// <summary>
        /// Timer used for starting new test each "periodInMiliseconds" miliseconds
        /// </summary>
        private System.Timers.Timer timer = new System.Timers.Timer(periodInMiliseconds);

        /// <summary>
        /// COnstructor that tries to assign the IP address
        /// </summary>
        /// <param name="ipAddress">IP address</param>
        internal IPTest(string ipAddress)
        {
            bool parsed = IPAddress.TryParse(ipAddress, out this.ipAddress);
            if (!parsed)
                Console.WriteLine($"{ipAddress} cannot be intepreted as an IP Address," +
                    $"so no test will be run on this IP address.\n");
            
        }

        /// <summary>
        /// Starts the timer that will ensure run all the tests and waits for the signal when time is up
        /// </summary>
        public void RunTests()
        {
            if (ipAddress == null)
                return;
            start = DateTime.Now;
            timer.Elapsed += async (sender, e) => await Test();
            timer.Start();
            stopTimerResetEvent.Wait();
            timer.Stop();
            stopTimerResetEvent.Dispose();            
            return;
        }

        /// <summary>
        /// Checks if the time is up. If not calls ping
        /// </summary>
        private async Task Test()
        {
            if (DateTime.Now < start.AddSeconds(testTimeInSeconds))
                PingIPAddress();
            else
                stopTimerResetEvent.Set();

        }

        /// <summary>
        /// Pings the IP address, waits for the response, sends the result
        /// </summary>
        private void PingIPAddress()
        {
            bool success = false;
            Ping pingSender = new Ping();
            DateTime requestTime = DateTime.Now;            
            PingReply reply = pingSender.Send(ipAddress, timeoutInMiliseconds);
            if (reply.Status == IPStatus.Success)
                success = true;
            TestResultsXMLFileWriter.AddTestToWriteQueue(ipAddress.ToString(), requestTime, success);
        }
    }
}
