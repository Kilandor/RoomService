using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RoomService
{
    public class RoomServiceConfig
    {
        public List<string> Parameters { get; set; }
        public List<string> OnLoad { get; set; }
        public List<string> OnPlayerJoined { get; set; }
        public List<string> OnPlayerLeft { get; set; }
        public List<string> OnRoundStart { get; set; }
        public List<string> OnRoundEnd { get; set; }
        public List<string> OnPlayerImproved { get; set; }
        public List<string> OnPlayerFinished { get; set; }
    }

    public static class RoomServiceConfigLoader
    {
        /// <summary>
        /// Loads the RoomService configuration from the specified JSON file.
        /// </summary>
        /// <param name="path">Path to the JSON configuration file.</param>
        /// <returns>A RoomServiceConfig object populated with the configuration data, or null if an error occurs.</returns>
        public static RoomServiceConfig LoadConfig(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("Configuration path cannot be null or empty.");
                return null;
            }

            if (!File.Exists(path))
            {
                Console.WriteLine($"The configuration file was not found: {path}");
                return null;
            }

            try
            {
                // Read the JSON file
                string json = File.ReadAllText(path);

                // Deserialize the JSON into a RoomServiceConfig object
                return JsonConvert.DeserializeObject<RoomServiceConfig>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the configuration: {ex.Message}");
                return null;
            }
        }
    }
}
