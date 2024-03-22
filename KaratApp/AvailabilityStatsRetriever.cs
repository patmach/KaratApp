using KaratApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KaratApp
{
    internal sealed class AvailabilityStatsRetriever
    {
        
        private static Dictionary<string, AvailaibilityStat> IPAddressAvailaibiltyDict = new Dictionary<string, AvailaibilityStat>();

        public static void RetrieveAndPrint(string[] ipAddresses, string pathToXMLFile)
        {
            CreateDictionary(ipAddresses);
            ComputeFromXMLFile(pathToXMLFile);
            Print();
        }

        private static void CreateDictionary(string[] ipAddresses)
        {
            foreach (var ipAddress in ipAddresses)
            {
                IPAddressAvailaibiltyDict.Add(ipAddress, 
                    new AvailaibilityStat
                    { 
                        successfulTests =0,
                        tests =0
                    });
            }
        }

        private static Dictionary<string, AvailaibilityStat> ComputeFromXMLFile(string path)
        {
            using(XmlReader reader  = XmlReader.Create(path))
            {
                try
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
                catch(Exception ex)
                {
                    var debug = 1;
                }
            }
            return IPAddressAvailaibiltyDict;
        }

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
