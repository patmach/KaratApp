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
    /// <summary>
    /// Class working that ensure the write to XML file
    /// </summary>
    internal static class TestResultsXMLFileWriter
    {
        /// <summary>
        /// Path to the XML file
        /// </summary>
        internal static string path = "Results.xml";

        /// <summary>
        /// Queue that contains test results that must be written
        /// </summary>
        private static Queue<IPAddressTest> IPAddressTestResultQueue = new Queue<IPAddressTest>();

        /// <summary>
        /// StreamWriter that writes text to the XML file
        /// </summary>
        private static StreamWriter testResultsXMLStreamWriter;

        /// <summary>
        /// Creating XML file structure by the serializer.
        /// </summary>
        internal static void CreateNewTestResultFile()
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
            // The closing tags has to be removed so the data of IP address tests can be appended to the file
            var doc = File.ReadAllText(path);
            doc = doc.Replace(" />", ">").Replace($"</{nameof(Testing)}>", "");
            File.WriteAllText(path, doc);
            testResultsXMLStreamWriter = new StreamWriter(path, append: true);
        }
        
        /// <summary>
        /// Starts write loop
        /// </summary>
        /// <param name="timeoutInSeconds">How long the write will run</param>
        internal static Task StartXMLWriteTask(int timeoutInSeconds)
        {
            CreateNewTestResultFile();
            Task writeTask = new Task(() => WriteTestsToXML(timeoutInSeconds));
            writeTask.Start();
            return writeTask;
        }

        /// <summary>
        /// Adds result of IP address test to the write queue
        /// </summary>
        /// <param name="ipAddress">Pinged IP address</param>
        /// <param name="requestTime">Time when the request was sent</param>
        /// <param name="success">Result of the ping test</param>
        internal static void AddTestToWriteQueue(string ipAddress, DateTime requestTime, bool success)
        {
            IPAddressTest ipAddressTest = new IPAddressTest(ipAddress, requestTime, success);
            lock(IPAddressTestResultQueue)
            {
                IPAddressTestResultQueue.Enqueue(ipAddressTest);
                Monitor.PulseAll(IPAddressTestResultQueue);
            }
        }

        /// <summary>
        /// Loop that writes all IP address tests result from the queue
        /// </summary>
        /// <param name="timeoutInSeconds">How long the write will run</param>
        internal static void WriteTestsToXML(int timeoutInSeconds)
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

        /// <summary>
        /// Writes one test result to the XML
        /// </summary>
        /// <param name="ipAddressTest"></param>
        private static void WriteTest(IPAddressTest ipAddressTest)
        {
            testResultsXMLStreamWriter.WriteLine(ipAddressTest.ToXML());
        }

        /// <summary>
        /// Writes closing tags that was removed when the file was created
        /// </summary>
        private static void WriteFileEnd()
        {
            testResultsXMLStreamWriter.WriteLine($"</{nameof(IPAddressTest)}s>");
            testResultsXMLStreamWriter.WriteLine($"</{nameof(Testing)}>");
        }

        


    }
}
