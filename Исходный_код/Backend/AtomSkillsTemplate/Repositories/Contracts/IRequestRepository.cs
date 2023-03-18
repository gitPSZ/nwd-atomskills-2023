using System.Collections.Generic;
using System.Threading.Tasks;
using AtomSkillsTemplate.Models;
using AtomSkillsTemplate.Models.ClaimsForTable;
using AtomSkillsTemplate.Models.DTOs;
using AtomSkillsTemplate.NewModels;
using Microsoft.AspNetCore.Http;

namespace AtomSkillsTemplate.Repositories.Contracts
{
    public interface IRequestRepository
    {
        public Task<IEnumerable<Request>> GetRequest();
        public Task<IEnumerable<Request>> GetLastRequest(long count);

        public Task<MachineRequestDto> SaveMachineRequest(MachineRequestDto MachineRequest);
        public Task<IEnumerable<ProductForPosition>> GetProductsRequest(Request request);
        
        public Task<IEnumerable<ClaimsForTableModel>> GetRequestForTable(PersonDTO roleUser);
        public Task<RequestDTO> SaveRequest(RequestDTO personInfo);
        public Task<ClaimsForTableModel> SaveExecutor(ClaimsForTableModel personInfo);
        public Task<RequestDTO> ToWork(RequestDTO claim);
        public Task<ClaimsForTableModel> CancelClaim(ClaimsForTableModel claim);
        public Task<RequestDTO> AcceptClaim(RequestDTO claim);
        public Task<RequestDTO> CancelClaim(RequestDTO claim);
        public Task<IEnumerable<Priority>> GetPriorities();
        public Task<IEnumerable<State>> GetStates();
        public Task<long> GetCountRequest();
        

    }
}
