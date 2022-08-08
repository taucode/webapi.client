using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Text;
using System.Web;

namespace TauCode.WebApi.Client;

public static class WebApiClientHelper
{
    internal static readonly JsonSerializerSettings JsonSerializerSettings;

    static WebApiClientHelper()
    {
        JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver
            {
                NamingStrategy =
                {
                    ProcessDictionaryKeys = false,
                },
            },
        };
    }

    /// <summary>
    /// Serialize a single value (e.g. DateTime) using the JSON serializer.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    /// <returns>The serialized value</returns>
    public static string SerializeValueToJson(object value)
    {
        // Get JSON string for value
        var jsonString = JsonConvert.SerializeObject(value, JsonSerializerSettings);

        // Remove unwanted quotations in the start and end of the string
        jsonString = jsonString.Trim('\"');

        return jsonString;
    }

    private static string SerializeValue(object value)
    {
        if (value is string s)
        {
            return s; // JSON will escape '\\' and '"', we don't want it.
        }

        return SerializeValueToJson(value);
    }

    public static string QueryParamsToString(IDictionary<string, object> queryParams)
    {
        if (queryParams == null)
        {
            throw new ArgumentNullException(nameof(queryParams));
        }

        if (queryParams == null)
        {
            throw new ArgumentNullException(nameof(queryParams));
        }

        var querySb = new StringBuilder();
        var added = false;

        if (queryParams.Count > 0)
        {
            foreach (var pair in queryParams)
            {
                var name = pair.Key;
                var value = pair.Value;

                if (value == null)
                {
                    continue; // won't add nulls to query string
                }

                if (value is IEnumerable collection && !(value is string))
                {
                    foreach (var collectionEntry in collection)
                    {
                        if (collectionEntry == null)
                        {
                            continue; // won't add nulls to query string
                        }

                        if (added)
                        {
                            querySb.Append("&");
                        }

                        var serializedCollectionEntry = SerializeValue(collectionEntry);
                        var escapedCollectionEntry = HttpUtility.UrlEncode(serializedCollectionEntry);
                        querySb.AppendFormat($"{name}={escapedCollectionEntry}");
                        added = true;
                    }
                }
                else
                {
                    if (added)
                    {
                        querySb.Append("&");
                    }

                    var serializedValue = SerializeValue(value);
                    var escapedValue = HttpUtility.UrlEncode(serializedValue);
                    querySb.AppendFormat($"{name}={escapedValue}");
                    added = true;
                }
            }
        }

        return querySb.ToString();
    }

    public static string AddQueryParams(string url, IDictionary<string, object> queryParams)
    {
        if (url == null)
        {
            throw new ArgumentNullException(nameof(url));
        }

        if (url.Length == 0)
        {
            throw new ArgumentException($"'{nameof(url)}' cannot be empty.");
        }

        var queryParamsString = QueryParamsToString(queryParams);

        if (queryParamsString.Length > 0)
        {
            var sbFullUrl = new StringBuilder(url);
            if (url.Contains('?'))
            {
                var lastChar = url[^1];

                if (lastChar == '&' || lastChar == '?')
                {
                    sbFullUrl.Append(queryParamsString);
                }
            }
            else
            {
                sbFullUrl.Append('?');
                sbFullUrl.Append(queryParamsString);
            }

            return sbFullUrl.ToString();
        }
        else
        {
            return url;
        }
    }
}