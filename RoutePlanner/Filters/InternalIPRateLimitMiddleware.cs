using RoutePlanner.Data;
using AspNetCoreRateLimit;
using Microsoft.Extensions.Options;

namespace RoutePlanner.Filters
{
    public class InternalIPRateLimitMiddleware : IpRateLimitMiddleware
    {
        public InternalIPRateLimitMiddleware(RequestDelegate next, IProcessingStrategy strategy, IOptions<IpRateLimitOptions> options, IRateLimitCounterStore counterStore, IIpPolicyStore policyStore, IRateLimitConfiguration config, ILogger<IpRateLimitMiddleware> logger) : base(next, strategy, options, policyStore, config, logger)
        {
        }

        public override Task ReturnQuotaExceededResponse(HttpContext httpContext, RateLimitRule rule, string retryAfter)
        {
            InternalApiResponse internalApiResponse = new(InternalApiResponse.StatusCode.InternalLimitReached, null);
            internalApiResponse.ErrorMessage = $"Przekroczono ustalony limit żądań ({rule.Limit} na {rule.Period}). Spróbuj ponownie za {rule.Period}.";

            httpContext.Response.Headers["Retry-After"] = retryAfter;
            httpContext.Response.StatusCode = 429;
            httpContext.Response.ContentType = "application/json";

            return httpContext.Response.WriteAsync(internalApiResponse.GetInternalResponseJson());
        }
    }
}
