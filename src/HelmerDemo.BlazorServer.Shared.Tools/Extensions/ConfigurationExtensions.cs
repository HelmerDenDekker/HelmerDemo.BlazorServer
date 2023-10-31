using Microsoft.Extensions.Configuration;

namespace HelmerDemo.BlazorServer.Shared.Tools.Extensions
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string GetRequiredValue(this IConfiguration configuration, string name) =>
            configuration[name] ?? throw new InvalidOperationException($"Configuration missing value for: {(configuration is IConfigurationSection s ? s.Path + ":" + name : name)}");

        /// <summary>
        /// Gets the connection string to a database
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string GetDatabaseConnectionString(this IConfiguration configuration, string name) =>
            configuration.GetConnectionString(name) ?? throw new InvalidOperationException($"Configuration missing value for: {(configuration is IConfigurationSection s ? s.Path + ":ConnectionStrings:" + name : "ConnectionStrings:" + name)}");
    }
}