using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Access.Utility.ConfigParser
{
    public class ConfigParser : IConfigParser
    {
        public IConfiguration Configuration { get; }

        public ConfigParser(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public long? JwtExpiryInSeconds => Convert.ToInt64(Configuration["Jwt:ExpiryInSeconds"]);

        public IEnumerable<string> JwtAudiences => Configuration.GetSection("Jwt:Audiences").GetChildren().Select(x => x.Value);

        public string ClientAESKey => Configuration.GetSection("Encryption")["ClientAESKey"];

        public TimeSpan TransactionTimeoutInSeconds => TimeSpan.FromSeconds(Convert.ToDouble(Configuration.GetSection("Transactions")["TimeoutInSeconds"]));
    }
}
