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
    public sealed class IPAddressTest
    {
        [XmlElement("IPAddress")]
        public string IPAddress { get; set; }

        [XmlElement("RequestTime")]
        public DateTime RequestTime { get; set; }

        [XmlElement("Success")]
        public bool Success { get; set; }

        public IPAddressTest(string ipAddress, DateTime requestTime, bool success)
        {
            this.IPAddress = ipAddress;
            this.RequestTime = requestTime;
            this.Success = success;
        }

        public IPAddressTest() { }

        public string ToXML()
        {
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
