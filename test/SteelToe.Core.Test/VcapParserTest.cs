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
