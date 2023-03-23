using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Model
{
    public class XWSDatabaseSettings : IXWSDatabaseSettings
    {
        public string ConnectionString { get; set; } 

        public string DatabaseName { get; set; } 

        public string FlightsCollectionName { get; set; }

        public string UsersCollectionName { get; set; }
    }

    public interface IXWSDatabaseSettings
    {
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }

        string FlightsCollectionName { get; set; }

        string UsersCollectionName { get; set; }
    }
}
