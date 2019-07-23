using System.Net.Http;
using System.Threading.Tasks;

namespace TauCode.WebApi.Client
{
    public interface IServiceClient
    {
        Task<HttpResponseMessage> SendAsync(HttpMethod method, string routeTemplate, object segments = null, object queryParams = null, object body = null);
    }
}
