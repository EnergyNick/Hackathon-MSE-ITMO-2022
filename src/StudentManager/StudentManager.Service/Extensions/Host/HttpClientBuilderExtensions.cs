using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace StudentManager.Service.Extensions
{
    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddRetryPolicy(this IHttpClientBuilder clientBuilder, int retryCount)
        {
            return clientBuilder.AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .Or<TimeoutRejectedException>()
                    .WaitAndRetryAsync(retryCount, 
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
        }

        public static IHttpClientBuilder AddTimeoutPolicy(this IHttpClientBuilder clientBuilder, int seconds)
        {
            return clientBuilder.AddPolicyHandler(
                Policy.TimeoutAsync<HttpResponseMessage>(seconds));
        }
    }
}