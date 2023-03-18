using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Shared.authorization;

namespace AtomSkillsTemplate.Helpers
{
    public class AuthorizationHelper
    {
        public class CustomAuthorizationAttribute : TypeFilterAttribute
        {
            public CustomAuthorizationAttribute(string athorizedRolesString) : base(typeof(CustomAuthorizationFilter))
            {
                Arguments = new object[]{ athorizedRolesString };
            }
        }

        public class CustomAuthorizationFilter : IAuthorizationFilter
        {
            private string[] athorizedRoles;
            private IConfiguration config;
            public CustomAuthorizationFilter(string athorizedRolesString, IConfiguration configuration)
            {
                this.config = configuration;
                this.athorizedRoles = athorizedRolesString.Split(';');
            }

            public async void OnAuthorization(AuthorizationFilterContext context)
            {
                try
                {
                    
                    if(context.ActionDescriptor.EndpointMetadata.FirstOrDefault(o=>o is AllowAnonymousAttribute) != null) 
                    {
                        return;
                    }
                    if (context.HttpContext.Request.Headers.ContainsKey("token") == false)
                    {
                        context.Result = new UnauthorizedResult();
                        context.HttpContext.Response.StatusCode =
                            (int)HttpStatusCode.Unauthorized;
                        return;
                    }

                    var user = UserHelper.GetUserFromRequest(context.HttpContext.Request);


                    var token = context.HttpContext.Request.Headers["token"];

                    var tokenString = token.FirstOrDefault();

                    var array = tokenString.Split('.');
                    var tokenValues = array[0];
                    var tokenSign = array[1];

                    if (AsymmetricCypherHelper.Hash(tokenValues + config["PrivateKEY"]) == tokenSign &&
                        UserHelper.GetTokenFromRequest(context.HttpContext.Request).ActiveUntil > DateTime.Now &&
                        user != null && athorizedRoles.Contains(user.RoleName))
                    {
                        return;
                    }

                    context.Result = new UnauthorizedResult();
                    context.HttpContext.Response.StatusCode =
                        (int)HttpStatusCode.Unauthorized;
                }
                catch (Exception e)
                {

                    context.Result = new UnauthorizedResult();
                    context.HttpContext.Response.StatusCode =
                        (int)HttpStatusCode.Unauthorized;
                }
                

            }
        }
    }
}
