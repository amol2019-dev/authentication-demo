using AdvikEnterprises.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AdvikEnterprises.Attributes
{
    //https://www.devtrends.co.uk/blog/dependency-injection-in-action-filters-in-asp.net-core

    public class JwtFilter : IAuthorizationFilter
    {
        private IUserService _userService;
        public JwtFilter(IUserService userService)
        {
            _userService = userService;
        }

        private const string DefaultRoles = "";
        public string Roles { get; set; } = DefaultRoles;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                //if (string.IsNullOrWhiteSpace(Roles))
                //{
                //    context.Result = new UnauthorizedResult();
                //    return;
                //}

                var header = context.HttpContext.Request.Headers.ContainsKey("CustomAuthorization");
                if (!header)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                string token = context.HttpContext.Request.Headers["CustomAuthorization"];
                if (string.IsNullOrWhiteSpace(token) || !token.StartsWith("Bearer"))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                validateToken(token);
            }
            catch (Exception ex)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        private void validateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var param = new TokenValidationParameters
            {
                ValidAudience = "BV",
                ValidIssuer = "www.ups.com",
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING")),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            SecurityToken validatedToken;
            tokenHandler.ValidateToken(token.Substring(7), param, out validatedToken);


            if (validatedToken != null && !string.IsNullOrWhiteSpace(Roles))
            {
                var role = ((System.IdentityModel.Tokens.Jwt.JwtSecurityToken)validatedToken).Claims.FirstOrDefault(x => x.Type == "role").Value;
                if (!Roles.Split(',').Any(x => x.Trim() == role))
                {
                    throw new Exception("Role not matching");
                }
            }
        }
    }

    public class JwtFilterFactory : Attribute, IFilterFactory
    {
        public string Roles { get; set; }
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetRequiredService<JwtFilter>();

            if (!string.IsNullOrWhiteSpace(Roles))
            {
                filter.Roles = Roles;
            }

            return filter;
        }
    }
}

//public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken);
//var claims = new List<Claim>
//    {
//        new Claim(ClaimTypes.Name, ""),
//        new Claim(ClaimTypes.Role, "")
//    };

//var identity = new ClaimsIdentity(claims, Scheme.Name);
//var principal = new GenericPrincipal(identity, new[] { validatedToken.Value.Item2 });
//var ticket = new AuthenticationTicket(principal, Scheme.Name);
//return AuthenticateResult.Success(ticket);