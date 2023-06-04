using Microsoft.Extensions.Configuration;

namespace C3Apparel.Data.Sql
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString => _configuration["ConnectionStrings:ConnectionString"];
    }
}