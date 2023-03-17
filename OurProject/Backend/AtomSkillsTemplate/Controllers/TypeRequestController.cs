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

namespace AtomSkillsTemplate.Controllers
{
    [ApiController]
    [Route("api/typerequest")]
    [AuthorizationHelper.CustomAuthorizationAttribute("Администратор;Инициатор;Оператор первой линии;Исполнитель;Сервис-Менеджер")]
    public class TypeRequestController : Controller
    {
        private ITypeRequestRepository personRepository { get; set; }
        public TypeRequestController(ITypeRequestRepository personRepository)
        {
            this.personRepository = personRepository;
        }
        [HttpGet]
        public async Task<ActionResult<TypeRequestDTO>> GetRations()
        {
            var typeRequest = await personRepository.GetTypeRequest();
            if (typeRequest == null)
            {
                return NotFound();
            }
            return Ok(typeRequest);
        }



    }
}
