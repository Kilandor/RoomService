using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoomService
{
    public class RoomServiceConfigJSON
    {
        public List<string> Parameters { get; set; }
        public List<string> OnLoad { get; set; }
        public List<string> OnUnload { get; set; }
        public List<string> OnPlayerJoined { get; set; }
        public List<string> OnPlayerLeft { get; set; }
        public List<string> OnRoundStart { get; set; }
        public List<string> OnRoundEnd { get; set; }
        public List<string> OnPlayerImproved { get; set; }
        public List<string> OnPlayerFinished { get; set; }
    }

    public class RoomServiceConfig
    {
        public Dictionary<string, string> Parameters;
        public RoomServiceConfigJSON JSON { get; set; }

        public RoomServiceConfig(RoomServiceConfigJSON json)
        {
            JSON = json;

            //Parse custom parameters.
            Parameters = new Dictionary<string, string>();
            foreach (string p in json.Parameters)
            {
                string[] parts = p.Split(";");
                if (parts.Length > 1)
                {
                    if (!Parameters.ContainsKey(parts[0]))
                    {
                        Parameters.Add(parts[0], parts[1]);
                    }
                }
            }
        }

        public void Load()
        {
            ProcessEventList("OnLoad", JSON.OnLoad);
            ProcessEventList("OnUnload", JSON.OnUnload);
            ProcessEventList("OnPlayerJoined", JSON.OnPlayerJoined);
            ProcessEventList("OnPlayerLeft", JSON.OnPlayerLeft);
            ProcessEventList("OnRoundStart", JSON.OnRoundStart);
            ProcessEventList("OnRoundEnd", JSON.OnRoundEnd);
            ProcessEventList("OnPlayerFinished", JSON.OnPlayerFinished);
            ProcessEventList("OnPlayerImproved", JSON.OnPlayerImproved);      
        }

        private static void ProcessEventList(string eventName, List<string> eventList)
        {
            if (eventList == null || eventList.Count == 0)
            {
                return;
            }

            foreach (string command in eventList)
            {
                string functionName = ExtractFunctionName(command);
                List<string> parameters = ExtractParameters(command);
                RoomService.SubscribeToEvent(eventName, functionName, parameters);
            }
        }

        private static string ExtractFunctionName(string command)
        {
            // Function name is the part before the first '('
            int openParenIndex = command.IndexOf('(');
            if (openParenIndex == -1)
                return command; // No parameters, function name is the entire string

            return command.Substring(0, openParenIndex);
        }

        private static List<string> ExtractParameters(string command)
        {
            // Parameters are between '(' and ')'
            int openParenIndex = command.IndexOf('(');
            int closeParenIndex = command.LastIndexOf(')');
            if (openParenIndex == -1 || closeParenIndex == -1 || closeParenIndex < openParenIndex)
                return new List<string>(); // No parameters

            string paramString = command.Substring(openParenIndex + 1, closeParenIndex - openParenIndex - 1);

            // Split by ';' while handling nested parameters (e.g., "[1,2]")
            var parameters = new List<string>();
            int nestedLevel = 0;
            string currentParam = "";

            foreach (char c in paramString)
            {
                if (c == ';' && nestedLevel == 0)
                {
                    parameters.Add(currentParam.Trim());
                    currentParam = "";
                }
                else
                {
                    if (c == '[') nestedLevel++;
                    if (c == ']') nestedLevel--;
                    currentParam += c;
                }
            }

            if (!string.IsNullOrEmpty(currentParam))
                parameters.Add(currentParam.Trim());

            return parameters;
        }        
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
                RoomServiceConfigJSON jsonContent = JsonConvert.DeserializeObject<RoomServiceConfigJSON>(json);

                return new RoomServiceConfig(jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the configuration: {ex.Message}");
                return null;
            }
        }
    }
}
