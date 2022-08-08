using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using TauCode.Data;

namespace TauCode.WebApi.Client;

public class ServiceClient : IServiceClient
{
    #region Constructor

    public ServiceClient(HttpClient httpClient)
    {
        this.HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    #endregion

    #region Private

    private Task<HttpResponseMessage> SendRequest(ServiceRequest request, CancellationToken cancellationToken)
    {
        var uri = request.Route;

        var finalUri = WebApiClientHelper.AddQueryParams(uri, request.QueryParameters);
        var message = new HttpRequestMessage(request.HttpMethod, finalUri);

        if (request.Body != null)
        {
            var json = JsonConvert.SerializeObject(request.Body, WebApiClientHelper.JsonSerializerSettings);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        if (request.Headers != null)
        {
            foreach (var pair in request.Headers)
            {
                var headerName = pair.Key;
                var headerValue = pair.Value;

                if (headerValue != null)
                {
                    message.Headers.Add(headerName, headerValue);
                }
            }
        }

        return this.HttpClient.SendAsync(message, cancellationToken);
    }

    private static string EvaluateSegmentMatch(Match match, object segments)
    {
        if (segments == null)
        {
            throw new ArgumentException("Route expects 'segments' object, but it was null.", nameof(segments));
        }

        var name = match.Result("$1");
        var property = segments.GetType().GetProperty(name);
        if (property == null)
        {
            throw new ArgumentException(
                $"Property '{name}', which is required by the route, not found in 'segments' object.",
                nameof(segments));
        }

        var value = property.GetValue(segments);
        if (value == null)
        {
            throw new ArgumentException($"'segments' object has '{name}' value equal to 'null'.", nameof(segments));
        }

        var stringValue = WebApiClientHelper.SerializeValueToJson(value);
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

    #endregion

    #region IServiceClient Members

    public HttpClient HttpClient { get; }

    public Task<HttpResponseMessage> SendAsync(
        HttpMethod method,
        string routeTemplate,
        object segments,
        object queryParams,
        object body,
        IDictionary<string, string> headers,
        CancellationToken cancellationToken)
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
            Headers = headers,
        };

        var response = this.SendRequest(request, cancellationToken);
        return response;
    }

    #endregion
}