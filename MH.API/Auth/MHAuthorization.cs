using MH.Services.Interfaces;
using System.Net;
using MH.Common.Helpers;
using MH.Common.Models;
using MH.Common.Constants;

namespace MH.API.Auth
{
    public class MHAuthorization
    {
        private readonly RequestDelegate _next;

        public MHAuthorization(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext httpContext,
            IUserSessionService userSessionService,
            ISecurityService securityService,
            IHttpContextAccessor httpContextAccessor
        )
        {
            string url = httpContext.Request.Path;

            string extension = Path.GetExtension(url);
            if (!string.IsNullOrEmpty(extension))
            {
                await _next(httpContext);
                return;
            }

            url = string.Join('/', url.Split('/').Take(4));
            if (PublicUrls.Any(x => x == url.ToLower()))
            {
                await _next(httpContext);
                return;
            }

            var token = httpContextAccessor.HttpContext.Request.GetTokenFromRequest();
            if (token is null)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsJsonAsync("Unauthorize");
                return;
            }

            try
            {
                var userId = httpContextAccessor.TokenValue<int>(JwtClaim.UserId);

                var responseMessage = await userSessionService.GetUserSessionBySystemUserId(userId);
                var session = responseMessage?.ResponseObj as UserSession;
                if (session is null || token != session.Token)
                {
                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await httpContext.Response.WriteAsJsonAsync("Any active session not found.");
                    return;
                }

                TimeSpan ts = DateTime.Now - session.SessionEnd.Value;
                if (Math.Abs(ts.Minutes) > CommonConstant.SessionExpired)
                {
                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await httpContext.Response.WriteAsJsonAsync("Session expired.");
                    return;
                }

                if (!await securityService.CheckPermission(url))
                {
                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await httpContext.Response.WriteAsJsonAsync("You have no permission.");
                    return;
                }

                DateTime dateTime = DateTime.Now.AddMinutes(CommonConstant.SessionExpired);
                session.SessionEnd = dateTime;
                await userSessionService.SaveUserSession(session);
                await _next(httpContext);
            }
            catch (Exception)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsJsonAsync("Internal server error.");
            }
        }

        private readonly static IList<string> PublicUrls = new List<string>()
        {
            "/api/security/login",
            "/api/security/register",
            "/api/security/sendemailforgotpassword",
            "/api/security/updatepassword",
            "/api/security/ispermitregistration",
            "/api/receivablereports/downloadfile",
            "/api/imareports/downloadfile",
            "/negotiate"
        };
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MHAuthorizationExtensions
    {
        public static IApplicationBuilder UseMHAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MHAuthorization>();
        }
    }
}
