using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KaratApp.Model
{
    /// <summary>
    /// Class representing result of one IP Address test by ping
    /// </summary>
    [Serializable()]
    public sealed class IPAddressTest
    {
        /// <summary>
        /// pinged IP address
        /// </summary>
        [XmlElement("IPAddress")]
        public string IPAddress { get; set; }

        /// <summary>
        /// Time when the request was sent
        /// </summary>
        [XmlElement("RequestTime")]
        public DateTime RequestTime { get; set; }

        /// <summary>
        /// Result of the ping test
        /// </summary>
        [XmlElement("Success")]
        public bool Success { get; set; }

        /// <summary>
        /// Constructor assigning all 3 properties
        /// </summary>
        /// <param name="ipAddress">Pinged IP address</param>
        /// <param name="requestTime">Time when the request was sent</param>
        /// <param name="success">Result of the ping test</param>
        public IPAddressTest(string ipAddress, DateTime requestTime, bool success)
        {
            this.IPAddress = ipAddress;
            this.RequestTime = requestTime;
            this.Success = success;
        }

        /// <summary>
        /// Parameterless constructor XMLSerializer works with
        /// </summary>
        public IPAddressTest() { }

        /// <summary>
        /// XML representation of instance of this class
        /// </summary>
        /// <returns></returns>
        public string ToXML()
        {
            //XMLSerializer needs to work with streams or file. Thats why this method is written manually
            //Reflection not used to make this method and write to XML file faster
            StringBuilder sb = new StringBuilder();
            sb.Append($"<{nameof(IPAddressTest)}>");
            sb.Append($"<{nameof(IPAddress)}>").Append(IPAddress).Append($"</{nameof(IPAddress)}>");
            sb.Append($"<{nameof(RequestTime)}>").Append(RequestTime.ToString()).Append($"</{nameof(RequestTime)}>");
            sb.Append($"<{nameof(Success)}>").Append(Success).Append($"</{nameof(Success)}>");
            sb.Append($"</{nameof(IPAddressTest)}>");
            return sb.ToString();
        }

    }
}
