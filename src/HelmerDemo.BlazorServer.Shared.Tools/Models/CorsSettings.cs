namespace HelmerDemo.BlazorServer.Shared.Tools.Models
{
    public class CorsSettings
    {
        /// <summary>
        /// Allowed CORS origins
        /// </summary>
        public string[] AllowedOrigins { get; set; }

        /// <summary>
        /// Allowed CORS Headers
        /// </summary>
        public string[] AllowedHeaders { get; set; }

        /// <summary>
        /// Allowed CORS Methods
        /// </summary>
        public string[] AllowedMethods { get; }
    }
}
