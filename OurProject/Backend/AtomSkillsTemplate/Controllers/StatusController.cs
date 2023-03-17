using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtomSkillsTemplate.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AtomSkillsTemplate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private IConfiguration config;
        public StatusController(IConfiguration config)
        {
            this.config = config;
        }

        [HttpPost]
        public ServerStatus Get()
        {
            return new ServerStatus
            {
                ID = 0,
                StatusString = "up and running"
            };
        }
        [HttpGet]
        public ActionResult GetRedirected()
        {
            return Redirect(config["FrontendURL"]);
        }
        [AuthorizationHelper.CustomAuthorizationAttribute("role1")]
        [HttpGet("unauthorized")]
        public ServerStatus GetUnauthorized()
        {
            return new ServerStatus
            {
                ID = 0,
                StatusString = "up and running"
            };
        }
    }

    public class ServerStatus
    {
        public int ID { get; set; }
        public string StatusString { get; set; }
    }
}
