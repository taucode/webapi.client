using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using TauCode.Data;

namespace TauCode.WebApi.Client
{
    public class ServiceClient : IServiceClient
    {
        #region Constructor

        public ServiceClient(HttpClient httpClient)
        {
            this.HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        #endregion

        #region Private

        private Task<HttpResponseMessage> SendRequest(ServiceRequest request)
        {
            var uri = request.Route;

            var querySb = new StringBuilder();
            var added = false;

            if (request.QueryParameters.Count > 0)
            {
                foreach (var pair in request.QueryParameters)
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

            if (added)
            {
                querySb.Insert(0, '?');
            }

            var finalUri = uri + querySb;

            var message = new HttpRequestMessage(request.HttpMethod, finalUri);

            if (request.Body != null)
            {
                var json = JsonConvert.SerializeObject(request.Body);
                message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return this.HttpClient.SendAsync(message);
        }

        private static string EvaluateSegmentMatch(Match match, object segments)
        {
            if (segments == null)
            {
                throw new ArgumentException("Route expects 'segments' object, but it was null", nameof(segments));
            }

            var name = match.Result("$1");
            var property = segments.GetType().GetProperty(name);
            if (property == null)
            {
                throw new ArgumentException($"Property '{name}', which is required by the route, not found in 'segments' object", nameof(segments));
            }

            var value = property.GetValue(segments);
            if (value == null)
            {
                throw new ArgumentException($"'segments' object has '{name}' value equal to 'null'", nameof(segments));
            }

            var stringValue = JsonUtility.SerializeValue(value);
            return stringValue;
        }

        private static string SubstituteRouteParams(string routeTemplate, object segments)
        {
            var substitution = Regex.Replace(routeTemplate, @"\{(\w+)\}", m => EvaluateSegmentMatch(m, segments));
            return substitution;
        }

        private static IDictionary<string, object> BuildQueryParameters(object queryParams)
        {
            if (queryParams == null)
            {
                return new Dictionary<string, object>();
            }

            return new ValueDictionary(queryParams);
        }

        private static string SerializeValue(object value)
        {
            if (value is string s)
            {
                return s; // JSON will escape '\\' and '"', we don't want it.
            }

            return JsonUtility.SerializeValue(value);
        }

        #endregion

        #region IServiceClient Members

        public HttpClient HttpClient { get; private set; }

        public Task<HttpResponseMessage> SendAsync(
            HttpMethod method,
            string routeTemplate,
            object segments = null,
            object queryParams = null,
            object body = null)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (routeTemplate == null)
            {
                throw new ArgumentNullException(nameof(routeTemplate));
            }

            var request = new ServiceRequest(method)
            {
                Route = SubstituteRouteParams(routeTemplate, segments),
                QueryParameters = BuildQueryParameters(queryParams),
                Body = body,
            };

            var response = this.SendRequest(request);
            return response;
        }

        #endregion
    }
}
