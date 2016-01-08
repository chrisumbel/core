using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.OptionsModel;
using SteelToe.Core.Models;

namespace SteelToe.Core.Configuration
{
    public static class CloudFoundryServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudFoundry(this IServiceCollection serviceCollection,
            IConfigurationRoot configuration)
        {
            string vcapRaw = configuration.GetSection("VCAP_SERVICES").Value;
            Dictionary<String, BoundService> results = new Dictionary<string, BoundService>();
            VcapParser parser = new VcapParser();
            
            if (!string.IsNullOrEmpty(vcapRaw))
            {
                try
                {
                    results = parser.ParseConfigurationForVcap(vcapRaw);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("VCAP_SERVICES", "Could not parse VCAP_SERVICES environment variable or other substitute configuration. Cannot enable cloud foundry configuration.", ex);
                }
            }
           
            serviceCollection.Configure<CloudFoundryBoundServiceOptions>(options =>
            {
                options.BoundServices = results;
            });
            
            return serviceCollection;
        }
    }
}
