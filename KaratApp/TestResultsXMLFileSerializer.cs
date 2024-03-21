using KaratApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace KaratApp
{
    internal static class TestResultsXMLFileSerializer
    {
        private static string path = "Results.xml";

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
        }

        internal static void StartXMLWriteTask(string[] ipAddresses)
        {
            CreateNewTestResultFile(ipAddresses);
        }

        internal static void AddTestToWrite()
        {

        }
    }
}
