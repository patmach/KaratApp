using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratApp
{
    /// <summary>
    /// Class that contains method that will start IP address tests for each IP address
    /// </summary>
    internal static class IPTestOrchestrator
    {
        /// <summary>
        /// starts IP address tests for each IP address
        /// </summary>
        /// <param name="testTimeInSeconds">How long the test will run</param>
        /// <param name="ipAddresses">IP addresses</param>
        internal static void CreateAndRunIPTests(int testTimeInSeconds, string[] ipAddresses)
        {
            IPTest.testTimeInSeconds = testTimeInSeconds;
            List<Task> ipTestsTaskList = new List<Task>();
            foreach (string ipAddress in ipAddresses)
            {            
                IPTest ipTest = new IPTest(ipAddress);
                var task = new Task(ipTest.RunTests);
                task.Start();
                ipTestsTaskList.Add(task);
            }
            Task allTestsFinished = Task.WhenAll(ipTestsTaskList.ToArray());
            allTestsFinished.Wait();
        }
    }
}
