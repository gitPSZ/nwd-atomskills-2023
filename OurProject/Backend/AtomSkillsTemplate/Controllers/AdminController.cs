using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtomSkillsTemplate.Helpers;
using AtomSkillsTemplate.Models.DTOs;
using AtomSkillsTemplate.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AtomSkillsTemplate.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [AuthorizationHelper.CustomAuthorizationAttribute("Администратор")]
    public class AdminController : Controller
    {
        private IPersonRepository personRepository { get; set; }
        private IConfiguration configuration { get; set; }
        public AdminController(IPersonRepository personRepository, IConfiguration configuration)
        {
            this.personRepository = personRepository;
            this.configuration = configuration;
        }
        [HttpGet("all")]
        public async Task<ActionResult<PersonDTO>> GetUsers()
        {
            var personDTOs = await personRepository.GetPersonDTOs();
            if (personDTOs == null)
            {
                return NotFound();
            }
            return Ok(personDTOs.OrderBy(o => o.NameClient));
        }
        [HttpGet("roles")]
        public async Task<ActionResult<Role>> GetRoles()
        {
            var roles = await personRepository.GetRoles();
            if (roles == null)
            {
                return NotFound();
            }
            return Ok(roles.OrderBy(o => o.RoleCaption));
        }
        [HttpPost("changeRole/{personID:long}")]
        public async Task<bool> UpdateRole(long personID, [FromBody]long roleId)
        {
            try
            {
                return await personRepository.UpdateRole(personID, roleId);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
