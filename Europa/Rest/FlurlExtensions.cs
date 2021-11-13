using Flurl;
using Flurl.Http;

namespace Europa.Rest
{
    public static class FlurlExtensions
    {
        public static IFlurlRequest WithAuthorization(this IFlurlRequest url, string token)
        {
            return !string.IsNullOrEmpty(token) ? url.WithHeader("Authorization", token) : url;
        }

        public static IFlurlRequest WithAuthorization(this Url url, string token)
        {
            return WithAuthorization(new FlurlRequest(url), token);
        }
    }
}