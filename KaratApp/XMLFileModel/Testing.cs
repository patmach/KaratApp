using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KaratApp.Model
{
    /// <summary>
    /// Class represnting root element in the XML file. Contains all the IP address tests
    /// </summary>
    [Serializable()]
    public sealed class Testing
    {
        /// <summary>
        /// List of results of IP address tests by ping
        /// </summary>
        [XmlArray("IPAddressTests")]
        [XmlArrayItem("IPAddressTest", typeof(IPAddressTest))]
        public IPAddressTest[] IPAddressTests { get; set; }

        /// <summary>
        /// Constructor assigning the array of IP address tests
        /// </summary>
        /// <param name="IPAddressTests">List of results of IP address tests by ping</param>
        public Testing(IPAddressTest[] IPAddressTests)
        {
            this.IPAddressTests = IPAddressTests;
        }

        /// <summary>
        /// Parameterless constructor XMLSerializer works with
        /// </summary>
        public Testing() { }
    }
}
