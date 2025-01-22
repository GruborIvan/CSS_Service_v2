using CssService.Domain.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CssService.Infrastructure.Repositories.ExternalServicesRepository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ILogger<AuthenticationRepository> _logger;
        private readonly IConfiguration _configuration;

        public AuthenticationRepository(IConfiguration configuration, ILogger<AuthenticationRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> AuthenticateToSqlServerAsync(string username, string password)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection")!;

            if (string.IsNullOrEmpty(connectionString))
            {
                return false;
            }

            // Data Source=192.168.23.7\\CIAS19;Initial Catalog=DS_GTS_24102022;User id=sa;Password=css#12345;TrustServerCertificate=True;Integrated Security=False;

            try
            {
                var parts = connectionString.Split(';');
                var dataSourceExpression = parts[0].Trim();
                var initialCatalogExpression = parts[1].Trim();

                var dataSource = dataSourceExpression.Split('=')?[1];
                var initialCatalog = initialCatalogExpression.Split('=')?[1];

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder["Data Source"] = $"{dataSource}";
                builder["Initial Catalog"] = $"{initialCatalog}";
                builder["User Id"] = username;
                builder["Password"] = password;
                builder["TrustServerCertificate"] = true;
                builder["Integrated Security"] = false;

                connectionString = builder.ConnectionString;

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    await connection.QueryAsync("SELECT 1");

                    _logger.LogInformation($"Login for user {username} has succeeded.");
                    return true;
                }
            }
            catch (Exception ex) 
            {
                _logger.LogInformation($"Login for user {username} has failed. \n {ex}");
                return false;
            }
        }
    }
}