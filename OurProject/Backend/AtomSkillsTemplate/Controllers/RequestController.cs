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
using AtomSkillsTemplate.Helpers;
using AtomSkillsTemplate.Models.ClaimsForTable;
using AtomSkillsTemplate.NewModels;

namespace AtomSkillsTemplate.Controllers
{
    [ApiController]
    [Route("api/request")]
    [AuthorizationHelper.CustomAuthorizationAttribute("Начальник;Администратор")]
    public class RequestController : Controller
    {
        private IRequestRepository requestRepository { get; set; }
        public RequestController(IRequestRepository requestRepository)
        {
            this.requestRepository = requestRepository;
        }

        [HttpGet]

        public async Task<ActionResult<Request>> GetRequests()
        {
            var reqeustDto = await requestRepository.GetRequest();
            if (reqeustDto == null)
            {
                return NotFound();
            }
            return Ok(reqeustDto);
        }



        
        [HttpPost("last")]

        public async Task<ActionResult<Request>> GetLastRequests(CountDto Count)
        {
            var reqeustDto = await requestRepository.GetLastRequest(Count.Count);
            if (reqeustDto == null)
            {
                return NotFound();
            }
            return Ok(reqeustDto);
        }

        [HttpPost("saveMachineRequest")]

        public async Task<ActionResult<MachineRequestDto>> SaveMachineRequest(MachineRequestDto MachineRequest)
        {
            var machineRequest = await requestRepository.SaveMachineRequest(MachineRequest);
            if (machineRequest == null)
            {
                return NotFound();
            }
            return Ok(machineRequest);
        }


        [HttpPost("productsForPosition")]

        public async Task<ActionResult<ProductForPosition>> GetProductsRequest(Request request)
        {
            var products = await requestRepository.GetProductsRequest(request);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }



        [HttpGet("priorities")]

        public async Task<ActionResult<List<Priority>>> GetPriorities()
        {
            var personDTOs = await requestRepository.GetPriorities();
            if (personDTOs == null)
            {
                return NotFound();
            }
            return Ok(personDTOs);
        }



        [HttpPost("allClaims")]

        public async Task<ActionResult<ClaimsForTableModel>> GetRequestsForTable(PersonDTO roleUser)
        {
            var claims = await requestRepository.GetRequestForTable(roleUser);
            if (claims == null)
            {
                return NotFound();
            }
            return Ok(claims);
        }
        [HttpGet("states")]

        public async Task<ActionResult<List<State>>> GetStates()
        {
            var states = await requestRepository.GetStates();
            if (states == null)
            {
                return NotFound();
            }
            return Ok(states);
        }


        [HttpGet("countRequest")]

        public async Task<ActionResult<long>> GetCountRequest()
        {
            var count = await requestRepository.GetCountRequest();
            if (count == null)
            {
                return NotFound();
            }
            return Ok(count);
        }



        [HttpPost("saveExecutor")]

        public async Task<ActionResult<ClaimsForTableModel>> SaveExecutor(ClaimsForTableModel claim)
        {
            var claims = await requestRepository.SaveExecutor(claim);
            if (claims == null)
            {
                return NotFound();
            }
            return Ok(claims);
        }



        [HttpPost("toWork")]

        public async Task<ActionResult<RequestDTO>> toWork(RequestDTO claim)
        {
            var claims = await requestRepository.ToWork(claim);
            if (claims == null)
            {
                return NotFound();
            }
            return Ok(claims);
        }




        [HttpPost("acceptClaim")]

        public async Task<ActionResult<RequestDTO>> AcceptClaim(RequestDTO claim)
        {
            var claims = await requestRepository.AcceptClaim(claim);
            if (claims == null)
            {
                return NotFound();
            }
            return Ok(claims);
        }


        [HttpPost("cancelClaim")]

        public async Task<ActionResult<ClaimsForTableModel>> CancelClaim(ClaimsForTableModel claim)
        {
            var claims = await requestRepository.CancelClaim(claim);
            if (claims == null)
            {
                return NotFound();
            }
            return Ok(claims);
        }





        [HttpPut("save")]

        public async Task<ActionResult<RequestDTO>> SaveRequest(RequestDTO request)
        {
            var person = UserHelper.GetUserFromRequest(Request);
            try
            {
                var newIngredient = await requestRepository.SaveRequest(request);

                if (newIngredient == null)
                {
                    return NoContent();
                }

                return Ok(newIngredient);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return Ok(null);
        }




    }
}
