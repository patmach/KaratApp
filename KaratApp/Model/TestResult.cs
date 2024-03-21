using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KaratApp.Model
{
    [Serializable()]
    public class TestResult
    {
        [XmlElement("RequestTime")]
        public DateTime requestTime { get; set; }

        [XmlElement("ResponseTime")]
        public DateTime responseTime { get; set; }

        [XmlElement("Success")]
        public bool success { get; set; }

        public TestResult() { }
    }
}
