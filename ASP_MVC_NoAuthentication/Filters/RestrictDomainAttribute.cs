using ASP_MVC_NoAuthentication.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
public class RestrictDomainAttribute : Attribute, IAuthorizationFilter
{
    public IEnumerable<string> AllowedHosts { get; }
    public RestrictDomainAttribute(params string[] allowedHosts) => AllowedHosts = allowedHosts;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string host = context.HttpContext.Request.Host.Host;
        if (!AllowedHosts.Contains(host, StringComparer.OrdinalIgnoreCase))
        {
            //  Request came from an authorized host, return bad request

            InternalApiResponse internalApiResponse = new(InternalApiResponse.StatusCode.HostNotAllowed, null)
            {
                ErrorMessage = "Host nie jest dopuszczony do tych zasobów."
            };

            context.Result = new BadRequestObjectResult(internalApiResponse.GetInternalResponseJson());
        }
    }
}