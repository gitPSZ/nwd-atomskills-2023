using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtomSkillsTemplate.Connection.Interface;
using AtomSkillsTemplate.Helpers;
using AtomSkillsTemplate.Models.DTOs;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AtomSkillsTemplate.Controllers
{

    [ApiController]
    [Route("api/agileInterface")]
    public class AgileInterfaceController: Controller

    {
        private string schemaName = "\"as2023\"";

        private IConnectionFactory connectionFactory;
        public AgileInterfaceController(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        [HttpGet("navigationButtons")]

        public async Task<IEnumerable<NavigationButton>> GetNavigationButtons()
        {
            var person = UserHelper.GetUserFromRequest(Request);
            if (person == null)
            {
                return null;
            }

            using (var conn = connectionFactory.GetConnection())
            {
                var result = await conn.QueryAsync<NavigationButton>($"select * from {schemaName}.navigation_buttons where role_id = :roleID", new { roleID = person.RoleId });
                ;
                return result;
            }
        }
    }
}
