using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KaratApp.Model
{
    [Serializable()]
    public class TestResult
    {
        [XmlIgnore]
        public string IPAddress { get; set; }

        [XmlElement("RequestTime")]
        public DateTime RequestTime { get; set; }

        [XmlElement("Success")]
        public bool Success { get; set; }

        public TestResult(string ipAddress, DateTime requestTime, bool success) 
        { 
            this.IPAddress = ipAddress;
            this.RequestTime = requestTime;
            this.Success = success;
        }

        public TestResult() { }

        public string ToXML()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Test>");
            sb.Append($"<{nameof(RequestTime)}>").Append(RequestTime.ToString()).Append($"</{nameof(RequestTime)}>");
            sb.Append($"<{nameof(Success)}>").Append(Success).Append($"</{nameof(Success)}>");
            sb.Append("</Test>");
            return sb.ToString();
        }
    }
}
