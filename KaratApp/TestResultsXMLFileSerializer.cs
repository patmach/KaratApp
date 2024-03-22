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
    internal static class TestResultsXMLFileSerializer
    {
        private static string path = "Results.xml";
        private static Queue<TestResult> IPAddressTestResultQueue = new Queue<TestResult>();
        private static XDocument doc;
        private static StringWriter stringwriter = new StringWriter();

        internal static void CreateNewTestResultFile(string[] ipAddresses)
        {            
                IPAddressTest[] tests = ipAddresses.Select(ipAddress =>
                    new IPAddressTest(ipAddress, Array.Empty<TestResult>()))
                .ToArray();
                Testing testing = new Testing(tests);
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
                doc = XDocument.Load(path);
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
            TestResult testResult = new TestResult(ipAddress, requestTime, success);
            lock(IPAddressTestResultQueue)
            {
                IPAddressTestResultQueue.Enqueue(testResult);
                Monitor.Pulse(IPAddressTestResultQueue);
            }
        }

        internal static void WriteTestsToXML(int timeoutInSeconds)
        {
            try 
            { 
                DateTime start = DateTime.Now;
                TestResult testResult = new TestResult();
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
                doc.Save(path);
            }
            catch (Exception ex)
            {
                var debug = 1;
            }
        }

        private static void WriteTest(TestResult testResult)
        {       
            XElement? ipAddressTestXMLElement = doc.Descendants("IPAddressTest").FirstOrDefault(ipAddressTest =>
            ipAddressTest.Element("IPAddress")?.Value == testResult.IPAddress);
            XElement testsXElement = ipAddressTestXMLElement.Element("Tests");
            if (ipAddressTestXMLElement == null)
                Console.WriteLine($"XML Element with tests for IP address \"{testResult.IPAddress}\" does not exist!");
            var xml = testResult.ToXML();
            testsXElement.Add(XElement.Parse(xml));
        }

        


    }
}
