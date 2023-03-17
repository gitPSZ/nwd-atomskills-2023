using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtomSkillsTemplate.Models;
using AtomSkillsTemplate.Models.DTOs;
using AtomSkillsTemplate.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using AtomSkillsTemplate.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Shared.authorization;
using AtomSkillsTemplate.Services;
using AtomSkillsTemplate.NewModels;

namespace AtomSkillsTemplate.Controllers
{
    [ApiController]
    [Route("api/machine")]
    [AuthorizationHelper.CustomAuthorizationAttribute("Начальник")]
    public class MachineController : Controller
    {
        private IMachineRepository machineRepository { get; set; }
        private IConfiguration configuration { get; set; }
        public MachineController(IMachineRepository machineRepository, IConfiguration configuration)
        {
            this.machineRepository = machineRepository;
            this.configuration = configuration;
        }

        [HttpGet("all")]
        public async Task<ActionResult<Machine>> GetMachine()
        {
            var machineDTOs = await machineRepository.GetMachineDTOs();
            if (machineDTOs == null)
            {
                return NotFound();
            }
            return Ok(machineDTOs.OrderBy(o => o.MachineType));
        }


    }
}
