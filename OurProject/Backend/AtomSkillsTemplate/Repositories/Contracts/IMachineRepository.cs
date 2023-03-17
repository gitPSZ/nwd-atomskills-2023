using System.Collections.Generic;
using System.Threading.Tasks;
using AtomSkillsTemplate.Models;
using AtomSkillsTemplate.Models.DTOs;
using AtomSkillsTemplate.NewModels;
using AtomSkillsTemplate.Services;
using Microsoft.AspNetCore.Http;

namespace AtomSkillsTemplate.Repositories.Contracts
{
    public interface IMachineRepository
    {
        public Task<IEnumerable<Machine>> GetMachineDTOs();

    }
}
