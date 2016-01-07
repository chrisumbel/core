using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SteelToe.Core.Configuration;
using Newtonsoft.Json.Linq;

namespace SteelToe.Core.Sample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()

                // TODO - this is ugly right now, but when you add vcap as a JSON file,
                // it comes back with nulls, empty strings, and other non-parseable nonsense.
                // This is the only way (yet) to keep VCAP as a string that can be manually
                // parsed.
                .AddInMemoryCollection(new Dictionary<String, String>
                {
                    {
                        "VCAP_SERVICES", json
                    }
                }) 
                .AddJsonFile("appsettings.json")                
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Console.WriteLine("Set up configuration.");
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddOptions();            
            services.AddCloudFoundry(Configuration);        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);

        private string json = @"
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

    }
}
