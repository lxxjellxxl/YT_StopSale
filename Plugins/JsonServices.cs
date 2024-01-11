
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugins.Interfaces;
using System.Text.Json;


namespace Plugins
{
    public class JsonServices : IJsonServices
    {
        public void UpdateValue(string[] nestedKeys, object newValue, string jsonFilePath)
        {
            // Read the JSON content from the file
            string jsonContent = File.ReadAllText(jsonFilePath);

            // Parse the JSON content into a JObject
            JObject jsonObject = JObject.Parse(jsonContent);

            // Traverse the JSON structure to find the nested property
            JToken currentToken = jsonObject;
            foreach (string key in nestedKeys)
            {
                if (currentToken is JObject nestedObject && nestedObject.TryGetValue(key, out JToken nestedValue))
                {
                    currentToken = nestedValue;
                }
                else
                {
                    // Handle the case where the key is not found (optional)
                    Console.WriteLine($"Key '{key}' not found in the JSON.");
                    return;
                }
            }

            // Update the nested property with the new value
            currentToken.Replace(JToken.FromObject(newValue));

            // Convert the modified JObject back to a formatted JSON string
            string updatedJson = jsonObject.ToString(Formatting.Indented);

            // Write the updated JSON back to the file
            File.WriteAllText(jsonFilePath, updatedJson);
        }
    }
}
