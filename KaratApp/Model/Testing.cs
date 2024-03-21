using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KaratApp.Model
{
    [Serializable()]
    public class Testing
    {
        [XmlArray("IPAddressTests")]
        [XmlArrayItem("IPAddressTest", typeof(IPAddressTest))]
        public IPAddressTest[] IPAddressTests { get; set; }

        public Testing(IPAddressTest[] IPAddressTests)
        {
            this.IPAddressTests = IPAddressTests;
        }

        public Testing() { }
    }
}
