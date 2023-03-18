using System.Collections.Generic;
using System.Threading.Tasks;
using AtomSkillsTemplate.Models;
using AtomSkillsTemplate.Models.DTOs;
using Microsoft.AspNetCore.Http;

namespace AtomSkillsTemplate.Repositories.Contracts
{
    public interface ITypeRequestRepository
    {
        public Task<IEnumerable<TypeRequestDTO>> GetTypeRequest();

    }
}
