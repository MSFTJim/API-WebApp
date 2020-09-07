using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server.Kestrel.Https;

namespace AppAPI
{
    public class Program
    {
        
        public static void Main(string[] args)
        {

             var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: false)
                .AddUserSecrets<Program>();

                EarlyConfiguration = config.Build();               
            
             CreateHostBuilder(args)
                .Build()
                .Run();
            
        }

        public static IConfigurationRoot EarlyConfiguration { get; set; }
        public static IHostBuilder CreateHostBuilder(string[] args)
        { 
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    {     
                        string DevEnvironmentCheck = EarlyConfiguration["ASPNETCORE_ENVIRONMENT"];
                        string CertPassword = "";
                        if (DevEnvironmentCheck.Equals("Development"))
                            {
                                CertPassword = EarlyConfiguration["Kestrel:Certificates:Development:Password"];                   
                                webBuilder.UseKestrel(opts =>
                                    {  opts.ListenLocalhost(5001, opts => opts.UseHttps("localhost.pfx",CertPassword)); 
                                        });
                            };
                        
                        webBuilder.UseStartup<Startup>();       
                    });
        }       

        
    } // end program class
} // end namespace
