using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace ADXETools.FalconRequests
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEnvironmentConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        string FalconServiceUrl { get; }

        /// <summary>
        /// 
        /// </summary>
        string CosecUrl { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EnvironmentConfiguration : IEnvironmentConfiguration
    {
        /// <summary>
        /// Loads the configuration of the environment from the supplied sources: configuration
        /// </summary>
        /// <param name="configuration">The source of the configuration</param>
        public EnvironmentConfiguration(IConfiguration configuration)
        {
            var jVcap = JObject.Parse(configuration["VCAP_SERVICES"]);

            var falconCredentials = ((JArray)jVcap["user-provided"]).First(s => s["name"].Value<string>() == "falcon")["credentials"];
            FalconServiceUrl = falconCredentials["FALCON_SERVICE_URL"].Value<string>();
            CosecUrl = falconCredentials["COSEC"].Value<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        public string FalconServiceUrl { get; }

        /// <summary>
        /// 
        /// </summary>
        public string CosecUrl { get; }
    }
}