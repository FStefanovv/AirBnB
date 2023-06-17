using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Neo4J
{
    public class Neo4jSettings
    {
        public Uri Neo4jConnection { get; set; }

        public string Neo4jUser { get; set; }

        public string Neo4jPassword { get; set; }

        public string Neo4jDatabase { get; set; }

    }
}
