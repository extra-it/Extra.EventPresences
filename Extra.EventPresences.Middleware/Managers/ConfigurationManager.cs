using Extra.EventPresences.Middleware.Managers.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Middleware.Managers
{
    public class ConfigurationManager : iConfigurationManager
    {
        private static IConfiguration config;
        private static readonly object lockObject = new object();
        public static IConfiguration GetInstance()
        {
            if (config == null)
            {
                lock (lockObject)
                {
                    string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                    UriBuilder uri = new UriBuilder(codeBase);
                    string path = Uri.UnescapeDataString(uri.Path);
                    string currentpath = Path.GetDirectoryName(path);
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    config = new ConfigurationBuilder()
                      .SetBasePath(currentpath)
                      .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                      .Build();
                }
            }

            return config;
        }

        public string AuthUsername()
        {
            var Section = GetInstance().GetSection("Authentication");
            return Section.GetValue<string>("Username");
        }
        public string AuthPassword()
        {
            var Section = GetInstance().GetSection("Authentication");
            return Section.GetValue<string>("Password");
        }
    }
}
