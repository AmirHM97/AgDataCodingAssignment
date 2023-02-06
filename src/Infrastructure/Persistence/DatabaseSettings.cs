using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgDataCodingAssignment.Persistence
{
    public class RavenSettings
    {
        public string[] Urls
        {
            get; set;
        }
        public string DatabaseName{ get; set; }
        public string CertFilePath { get; set; }
        public string CertPassword { get; set; }
    }
}
