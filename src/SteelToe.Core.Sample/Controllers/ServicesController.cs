using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SteelToe.Core.Configuration;
using Microsoft.Extensions.OptionsModel;

namespace SteelToe.Core.Sample.Controllers
{
    [Route("api/[controller]")]
    public class ServicesController : Controller
    {
        private CloudFoundryBoundServiceOptions options;

        public ServicesController(IOptions<CloudFoundryBoundServiceOptions> optionsAccessor)
        {
            options = optionsAccessor.Value;
        }

        // GET: api/services
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return options.BoundServices.Select(bs => bs.Key).ToArray();            
        }
    }
}
