using KaratApp.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace KaratApp
{
    internal static class TestResultsXMLFileWriter
    {
        private static string path = "Results.xml";
        private static Queue<IPAddressTest> IPAddressTestResultQueue = new Queue<IPAddressTest>();
        private static StreamWriter testResultsXMLStreamWriter;

        internal static void CreateNewTestResultFile(string[] ipAddresses)
        {                          
            Testing testing = new Testing(Array.Empty<IPAddressTest>());
            XmlSerializer serializer = new XmlSerializer(typeof(Testing));
            using (XmlWriter writer = XmlWriter.Create(path))
            {
                try
                {
                    serializer.Serialize(writer, testing);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("Error has occured during creation of XML file.");
                    Environment.Exit(1);
                }
            }
            var doc = File.ReadAllText(path);
            doc = doc.Replace(" />", ">").Replace($"</{nameof(Testing)}>", "");
            File.WriteAllText(path, doc);
            testResultsXMLStreamWriter = new StreamWriter(path, append: true);
        }
        

        internal static Task StartXMLWriteTask(string[] ipAddresses, int timeoutInSeconds)
        {
            CreateNewTestResultFile(ipAddresses);
            Task writeTask = new Task(() => WriteTestsToXML(timeoutInSeconds));
            writeTask.Start();
            return writeTask;
        }

        internal static void AddTestToWriteQueue(string ipAddress, DateTime requestTime, bool success)
        {
            IPAddressTest ipAddressTest = new IPAddressTest(ipAddress, requestTime, success);
            lock(IPAddressTestResultQueue)
            {
                IPAddressTestResultQueue.Enqueue(ipAddressTest);
                Monitor.Pulse(IPAddressTestResultQueue);
            }
        }

        internal static void WriteTestsToXML(int timeoutInSeconds)
        {
            try 
            { 
                DateTime start = DateTime.Now;
                IPAddressTest? testResult = new IPAddressTest();
                while (start.AddSeconds(timeoutInSeconds) > DateTime.Now) {
                    lock (IPAddressTestResultQueue)
                    {
                        if (!IPAddressTestResultQueue.TryDequeue(out testResult)) {
                            Monitor.Wait(IPAddressTestResultQueue);
                            testResult = null;
                        }
                    }
                    if (testResult != null)
                        WriteTest(testResult);
                }
                WriteFileEnd();
                testResultsXMLStreamWriter.Close();
            }
            catch (Exception ex)
            {
                var debug = 1;
            }
        }

        private static void WriteTest(IPAddressTest ipAddressTest)
        {
            testResultsXMLStreamWriter.WriteLine(ipAddressTest.ToXML());
        }

        private static void WriteFileEnd()
        {
            testResultsXMLStreamWriter.WriteLine($"</{nameof(IPAddressTest)}s>");
            testResultsXMLStreamWriter.WriteLine($"</{nameof(Testing)}>");
        }

        


    }
}
