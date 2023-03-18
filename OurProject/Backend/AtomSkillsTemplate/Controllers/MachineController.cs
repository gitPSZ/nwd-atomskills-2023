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
using AtomSkillsTemplate.Services.Interfaces;

namespace AtomSkillsTemplate.Controllers
{
    [ApiController]
    [Route("api/machine")]
    [AuthorizationHelper.CustomAuthorizationAttribute("Начальник")]
    public class MachineController : Controller
    {
        private IMachineRepository machineRepository { get; set; }
        private IConfiguration configuration { get; set; }
        private IMonitoringService monitoring { get; set; }
        public MachineController(IMachineRepository machineRepository, IConfiguration configuration, IMonitoringService monitoringService)
        {
            this.machineRepository = machineRepository;
            this.configuration = configuration;
            this.monitoring = monitoringService;

        }

        [HttpGet("all")]
        public async Task<ActionResult<Machine>> GetMachine()
        {
            var machineDTOs = await machineRepository.GetMachineDTOs();
            foreach (Machine machine in machineDTOs)
            {
                if ( machine.IdState == 2)
                {
                    machine.IdRequest = monitoring.GetRequestIDThatMachineWorksOn(machine.Id);
                }
            }
            if (machineDTOs == null)
            {
                return NotFound();
            }
            return Ok(machineDTOs.OrderBy(o => o.MachineType));
        }

        [HttpPost("repair")]
        public async Task<ActionResult<Machine>> SetRepair(Machine machine)
        {
            var machineDTOs = await machineRepository.SetRepair(machine);
            if (machineDTOs == null)
            {
                return NotFound();
            }
            return Ok(machineDTOs);
        }


    }
}
