using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AtomSkillsTemplate.Controllers;
using AtomSkillsTemplate.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AtomSkillsTemplate.Helpers
{
    public static class UserHelper
    {
        public static PersonDTO GetUserFromRequest(HttpRequest request)
        {
            try
            {
                var token = GetTokenFromRequest(request);
                if (token == null)
                {
                    return null;
                }
                return token.User;

            }
            catch (Exception e)
            {
                return null;
            }

        }
        public static Token GetTokenFromRequest(HttpRequest request)
        {
            try
            {
                if (request.Headers.ContainsKey("token") == false)
                {
                    return null;
                }

                var token = request.Headers["token"];

                var tokenArray = token.FirstOrDefault().Split('.');
                var token64String = tokenArray[0];

                var jsonToken = Encoding.Unicode.GetString(Convert.FromBase64String(token64String)); ;
                var tokenObject = JsonConvert.DeserializeObject<Token>(jsonToken);
                return tokenObject;
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
