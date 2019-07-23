using Newtonsoft.Json;

namespace TauCode.WebApi.Client
{
    /// <summary>
    /// Utilities for JSON serializing / deserializing.
    /// </summary>
    public static class JsonUtility
    {
        /// <summary>
        /// Serialize a single value (e.g. DateTime) using the JSON serializer.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <returns>The serialized value</returns>
        public static string SerializeValue(object value)
        {
            // Get JSON string for value
            var jsonString = JsonConvert.SerializeObject(value);

            // Remove unwanted quotations in the start and end of the string
            jsonString = jsonString.Trim('\"');

            return jsonString;
        }
    }
}
