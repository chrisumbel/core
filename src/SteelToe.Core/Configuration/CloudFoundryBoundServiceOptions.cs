using SteelToe.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteelToe.Core.Configuration
{
    public class CloudFoundryBoundServiceOptions
    {
        public IDictionary<String, BoundService> BoundServices { get; set; }
    }
}
