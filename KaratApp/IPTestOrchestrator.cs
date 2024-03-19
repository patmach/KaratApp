using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratApp
{
    internal static class IPTestOrchestrator
    {
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
