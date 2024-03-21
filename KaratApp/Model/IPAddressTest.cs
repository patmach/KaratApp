using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KaratApp.Model
{
    [Serializable()]
    public class IPAddressTest
    {
        [XmlElement("IPAddress")]
        public string IPAddress { get; set; }

        [XmlArray("Tests")]
        [XmlArrayItem("Test", typeof(TestResult))]
        public TestResult[] Tests { get; set; }

        public IPAddressTest(string ipAddress, TestResult[] tests) 
        {
            this.IPAddress = ipAddress;
            this.Tests = tests;
        }

        public IPAddressTest() { }


    }
}
