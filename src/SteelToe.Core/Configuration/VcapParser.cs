using Microsoft.Extensions.Configuration;
using SteelToe.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace SteelToe.Core.Configuration
{
    public class VcapParser
    {
        public Dictionary<String, BoundService> ParseConfigurationForVcap(String rawVcap)
        {
            Dictionary<String, BoundService> results = new Dictionary<string, BoundService>();
            JObject root = (JObject)JsonConvert.DeserializeObject(rawVcap);

            foreach (KeyValuePair<string, JToken> property in root)
            {
                JArray contents = (JArray)property.Value;
                foreach (JObject service in contents)
                {
                    string key = (string)service["name"];
                    results.Add(key, new BoundService
                    {
                        Name = key,
                        Label = (string)service["label"],
                        Plan = service["plan"] != null ? (string)service["plan"] : String.Empty,
                        Tags = BuildTags(service),
                        UserProvided = property.Key.ToLower().Equals("user-provided") ? true : false,
                        Credentials = BuildCredentials(service)
                    });
                }
            }
            return results;
        }
        private IList<String> BuildTags(JObject service)
        {            
            if (service["tags"] != null)
            {
                JArray tags = (JArray)service["tags"];
                return tags.Select(t => (string)t).ToList();  
            } else
            {
                return Enumerable.Empty<String>().ToList();
            }
        }

        // TODO - this doesn't YET support infinite-depth credentials, it assumes that credentials
        // is a flat string-string map. Need to fix that soon...
        private Dictionary<String, Object> BuildCredentials(JObject service)
        {
            Dictionary<String, Object> results = new Dictionary<string, object>();
            JObject credentials = (JObject)service["credentials"];
            foreach (KeyValuePair<String, JToken> kvp in credentials)
            {               
                results.Add(kvp.Key, (string)kvp.Value);               
            }
            return results;
        }
    }
}
