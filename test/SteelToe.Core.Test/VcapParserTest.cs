using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteelToe.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SteelToe.Core.Test
{
    public class VcapParserTest
    {
        [Fact]
        public void TestMarketplaceServiceParsing()
        {
            var json = @"
                {'elephantsql': 
                    [
                        {
                            'name' : 'elephantsql-c6c0',
                            'label' : 'elephantsql',
                            'tags' : [ 'postgres', 'postgresql', 'relational' ],
                            'plan' : 'turtle',
                            'credentials' : {
                                'uri' : 'http://foo.bar/baz'
                            }
                        },
                        {
                            'name' : 'elephantsql-two',
                            'label' : 'elephantsql',
                            'tags' : ['postgres', 'postgresql', 'relational'],
                            'plan' : 'not-so-turtle',
                            'credentials' : {
                                'uri' : 'http://bar.baz/foo'
                            }
                        }
                    ],
                 'sendgrid' :
                    [
                        {
                            'name' : 'mysendgrid',
                            'label' : 'sendgrid',
                            'tags' : ['smtp'],
                            'plan' : 'free',
                            'credentials' : {
                                'hostname' : 'smtp.sendgrid.net',
                                'username' : 'foo',
                                'password' : 'bar'
                            }
                        }
                    ]
                }";

            VcapParser parser = new VcapParser();
            var results = parser.ParseConfigurationForVcap(json);
            Assert.NotNull(results);
            Assert.Equal(3, results.Count);
            Assert.True(results.ContainsKey("elephantsql-c6c0"));
            var elephantSql = results["elephantsql-c6c0"];

            Assert.Equal("turtle", elephantSql.Plan);
            Assert.Equal("elephantsql", elephantSql.Label);
            Assert.Equal(false, elephantSql.UserProvided);
            Assert.Equal(3, elephantSql.Tags.Count);

            Assert.Equal("http://foo.bar/baz", (string)elephantSql.Credentials["uri"]);
            Assert.Equal("smtp.sendgrid.net", results["mysendgrid"].Credentials["hostname"]);           
        }

        [Fact]
        public void TestParserHandlesUserProvidedServices()
        {
            var vcapRaw = @"
            {      
                'user-provided' :
                 [
                    {
                        'name' : 'my-ups',
                        'label' : 'user-provided',
                        'tags' : [],                  
                        'credentials' : {
                            'hostname' : 'host.name',
                            'username' : 'foo',
                            'password' : 'bar',
                            'uri' : 'http://this/is/a/uri'
                        }
                    } 
                 ]
             }";
            VcapParser parser = new VcapParser();
            var results = parser.ParseConfigurationForVcap(vcapRaw);
            Assert.NotNull(results);
            Assert.True(results.ContainsKey("my-ups"));
            Assert.Equal(1, results.Count);
            var myUps = results["my-ups"];
            Assert.True(myUps.UserProvided);
            Assert.Equal(String.Empty, myUps.Plan);
            Assert.Equal("user-provided", myUps.Label);
            Assert.Equal("http://this/is/a/uri", (string)myUps.Credentials["uri"]);
        }

        [Fact]
        public void TestVcapParserThrowsExceptionWithBadJson()
        {
            var badJson = @"{ 'this' : won't parse }";
            VcapParser parser = new VcapParser();
            Boolean caught = false;
            try {
                var results = parser.ParseConfigurationForVcap(badJson);
            } catch (Exception ex) {
                caught = true;
            }
            Assert.True(caught);            
        }

        [Fact]
        public void TestVcapParserThrowsExceptionWithNullOrEmptyJson()
        {
            var missingJson = String.Empty;
            VcapParser parser = new VcapParser();
            Boolean caught = false;
            try
            {
                var results = parser.ParseConfigurationForVcap(missingJson);
            }
            catch (Exception ex)
            {
                caught = true;
            }
            Assert.True(caught);
        }
    }
}
