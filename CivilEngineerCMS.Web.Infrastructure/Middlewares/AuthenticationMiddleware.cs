//namespace CivilEngineerCMS.Web.Infrastructure.Middlewares
//{
//    using Microsoft.AspNetCore.Http;

//    using System.Security.Claims;
//    using System.Text;

//    public class AuthenticationMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public AuthenticationMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            var authHeader = context.Request.Headers.TryGetValue("Authorization", out var authorizationToken);
//            if (authHeader != null && authHeader.ToString().StartsWith("basic", StringComparison.OrdinalIgnoreCase))
//            {
//                var token = authHeader.ToString().Substring("Basic ".Length).Trim();
//                System.Console.WriteLine(token);
//                var credentialstring = Encoding.UTF8.GetString(Convert.FromBase64String(token));
//                var credentials = credentialstring.Split(':');
//                if (credentials[0] == "admin" && credentials[1] == "admin")
//                {
//                    var claims = new[] { new Claim("name", credentials[0]), new Claim(ClaimTypes.Role, "Admin") };
//                    var identity = new ClaimsIdentity(claims, "Basic");
//                    context.User = new ClaimsPrincipal(identity);
//                }
//            }
//            else
//            {
//                context.Response.StatusCode = 401;
//                context.Response.Headers.SetCommaSeparatedValues("WWW-Authenticate", "Basic realm=\"dotnetthoughts.net\"");
//            }
//            await _next(context);
//        }

//        //public async Task Invoke(HttpContext context)
//        //{
//        //    string authHeader = context.Request.Headers["Authorization"];
//        //    if (authHeader != null && authHeader.StartsWith("Basic"))
//        //    {
//        //        //Extract credentials
//        //        string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
//        //        Encoding encoding = Encoding.GetEncoding("iso-8859-1");
//        //        string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

//        //        int seperatorIndex = usernamePassword.IndexOf(':');

//        //        var username = usernamePassword.Substring(0, seperatorIndex);
//        //        var password = usernamePassword.Substring(seperatorIndex + 1);

//        //        if (username == "test" && password == "test")
//        //        {
//        //            await _next.Invoke(context);
//        //        }
//        //        else
//        //        {
//        //            context.Response.StatusCode = 401; //Unauthorized
//        //            return;
//        //        }
//        //    }
//        //    else
//        //    {
//        //        // no authorization header
//        //        context.Response.StatusCode = 401; //Unauthorized
//        //        return;
//        //    }
//        //}
//    }
//}
