using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteelToe.Core.Models
{
    public class BoundService
    {
        public String Name { get; set; }
        public String Label { get; set; }
        public IList<String> Tags { get; set; }
        public String Plan { get; set; }
        public Dictionary<String, Object> Credentials { get; set; }
        public Boolean UserProvided { get; set; }
    }
}
