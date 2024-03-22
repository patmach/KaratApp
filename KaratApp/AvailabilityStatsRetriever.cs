using KaratApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KaratApp
{
    /// <summary>
    /// Class contianing method that retrieves IP address tests statistics and print them
    /// </summary>
    internal sealed class AvailabilityStatsRetriever
    {
        /// <summary>
        /// Dictionary containing stats for each IP address
        /// </summary>
        private static Dictionary<string, AvailaibilityStat> IPAddressAvailaibiltyDict = 
            new Dictionary<string, AvailaibilityStat>();

        /// <summary>
        /// The main method that will perform all steps of retrieval and printing
        /// </summary>
        /// <param name="ipAddresses">IP addresses whose stats should be retrieved</param>
        /// <param name="pathToXMLFile">Path to XML file with results of the IP address tests</param>
        public static void RetrieveAndPrint(string[] ipAddresses, string pathToXMLFile)
        {
            CreateDictionary(ipAddresses);
            RetrieveFromXMLFile(pathToXMLFile);
            Print();
        }

        /// <summary>
        /// Fill Dictionary with all IP addreses as keys and default Stat
        /// </summary>
        /// <param name="ipAddresses">IP addresses used as keys of the dictionary</param>
        private static void CreateDictionary(string[] ipAddresses)
        {
            foreach (var ipAddress in ipAddresses)
            {
                IPAddressAvailaibiltyDict.Add(ipAddress, 
                    new AvailaibilityStat
                    { 
                        successfulTests = 0,
                        tests = 0
                    });
            }
        }

        /// <summary>
        /// Retrieves number of IP address tests and number of successful tests
        /// </summary>
        /// <param name="pathToXMLFile">Path to XML file with results of the IP address tests</param>
        private static void RetrieveFromXMLFile(string pathToXMLFile)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(pathToXMLFile))
                {
                    while (reader.ReadToFollowing(nameof(IPAddressTest)))
                    {
                        reader.ReadToDescendant(nameof(IPAddressTest.IPAddress));
                        reader.Read(); //next descendant = text
                        string ipAddress = reader.Value;
                        reader.ReadToFollowing(nameof(IPAddressTest.Success));
                        reader.Read(); //next descendant = text
                        bool success = false;
                        if (!bool.TryParse(reader.Value, out success))
                        {
                            Console.WriteLine($"One of the tests result for {ipAddress} can not be parsed.");
                        }
                        else
                        {
                            IPAddressAvailaibiltyDict[ipAddress] = new AvailaibilityStat
                            {
                                successfulTests = IPAddressAvailaibiltyDict[ipAddress].successfulTests + (success ? 1 : 0),
                                tests = IPAddressAvailaibiltyDict[ipAddress].tests + 1
                            };
                        }
                    }
                }
            }
            catch (XmlException ex)
            {
                Console.WriteLine("Processing of XML file with results of the tests was unsuccessful.");
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// For each IP address computes percent of availability and prints it 
        /// </summary>
        private static void Print()
        {
            foreach (var ipAddress in IPAddressAvailaibiltyDict.Keys)
            {
                AvailaibilityStat stat = IPAddressAvailaibiltyDict[ipAddress];
                Console.Write($"{ipAddress}: ");
                if (stat.tests == 0)
                    Console.WriteLine("No tests were done for this IP address." +
                        $"This could be because the value {ipAddress} can not be interpreted as an IP address.");
                else
                {
                    double percent = 100.0 * stat.successfulTests  / stat.tests;
                    Console.WriteLine($"{Math.Round(percent, 5)} %");
                }
            }
        }

           

    }
}
