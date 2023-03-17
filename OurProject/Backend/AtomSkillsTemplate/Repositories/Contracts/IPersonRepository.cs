using System.Collections.Generic;
using System.Threading.Tasks;
using AtomSkillsTemplate.Models;
using AtomSkillsTemplate.Models.DTOs;
using Microsoft.AspNetCore.Http;

namespace AtomSkillsTemplate.Repositories.Contracts
{
    public interface IPersonRepository
    {
        public Task<IEnumerable<PersonDTO>> GetPersonDTOs();
        public Task<IEnumerable<PersonDTO>> GetExecutors();
        public Task<PersonDTO> GetPersonInfo(PersonDTO personInfo);
        public Task<long?> Registration(PersonDTO personInfo);
        public Task<bool> PersonGetReply(PersonDTO personInfo);
        public Task<IEnumerable<Role>> GetRoles();
        public Task<bool> UpdateRole(long personID, long roleID);
        public Task<bool> UpdateEmail(PersonDTO person, string email);
    }
}
