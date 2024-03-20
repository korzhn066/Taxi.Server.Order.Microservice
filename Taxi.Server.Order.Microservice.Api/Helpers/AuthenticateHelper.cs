namespace Taxi.Server.Order.Microservice.Api.Helpers
{
    public class AuthenticateHelper
    {
        public static string GetUserName(HttpContext httpContext)
        {
            if (httpContext is null)
                throw new ArgumentNullException(nameof(httpContext));

            if (httpContext.User is null)
                throw new ArgumentNullException(nameof(httpContext.User));

            if (httpContext.User.Identity is null)
                throw new ArgumentNullException(nameof(httpContext.User.Identity));

            if (httpContext.User.Identity.Name is null)
                throw new ArgumentNullException(nameof(httpContext.User.Identity.Name));

            return httpContext.User.Identity.Name;
        }
    }
}
