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

namespace AtomSkillsTemplate.Controllers
{
    [ApiController]
    [Route("api/persons")]
    [AuthorizationHelper.CustomAuthorizationAttribute("Начальник")]
    public class PersonController : Controller
    {
        private IPersonRepository personRepository { get; set; }
        private IConfiguration configuration { get; set; }
        public PersonController(IPersonRepository personRepository, IConfiguration configuration)
        {
            this.personRepository = personRepository;
            this.configuration = configuration;
        }
        [AllowAnonymous]
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

        [HttpGet]
        public async Task<ActionResult<PersonDTO>> GetRations()
        {
            var personDTOs = await personRepository.GetPersonDTOs();
            if (personDTOs == null)
            {
                return NotFound();
            }
            return Ok(personDTOs.OrderBy(o=>o.NameClient));
        }
        [HttpGet("actualCurrent")]
        public async Task<PersonDTO> GetUser()
        {
            var personDTOs = await personRepository.GetPersonDTOs();
            
            return personDTOs.FirstOrDefault(i=>i.ID == UserHelper.GetUserFromRequest(Request).ID);
        }
        [HttpGet("getexecutors")]
        public async Task<ActionResult<PersonDTO>> GetExecutors()
        {
            var personDTOs = await personRepository.GetExecutors();
            if (personDTOs == null)
            {
                return NotFound();
            }
            return Ok(personDTOs.OrderBy(o => o.NameClient));
        }
        [AllowAnonymous]
        [HttpPost("auth")]

        public async Task<ActionResult<PersonDTO>> GetUser(PersonDTO personInfo)
        {
            personInfo.Password = personInfo.Password;
            var personDTOs = await personRepository.GetPersonInfo(personInfo);
           
            if (personDTOs == null)
            {
                return NotFound();
            }
            return Ok(personDTOs);
        }

        [AllowAnonymous]
        [HttpGet("sendmail")]

        public async Task<ActionResult<PersonDTO>> SendMail()
        {

           
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync("AtomSkills2023@mail.ru", "Тема письма", "Тест письма: тест!");
                return RedirectToAction("Index");
                    
            return Ok("ok");
        }
        [AllowAnonymous]
        [HttpPut("registration")]

        public async Task<ActionResult<long?>> Registration(PersonDTO personInfo)
        {
            personInfo.Password = personInfo.Password;
            var personDTOs = await personRepository.Registration(personInfo);

            if (personDTOs == null)
            {
                return NotFound();
            }
            return Ok(personDTOs);
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<string> GetToken(PersonDTO personInfo)
        {
            var person = await personRepository.GetPersonInfo(personInfo);

            if (person == null)
            {
                return "";
            }

            var password = personInfo.Password + person.Salt;
            var encryptedPassword = AsymmetricCypherHelper.Hash(password);

            if (person.Password == encryptedPassword)
            {
                var token = new Token()
                {
                    ActiveUntil = DateTime.Now.AddDays(1),
                    User = person
                };
                var tokenJson = JsonConvert.SerializeObject(token);

                var tokenString = Convert.ToBase64String(Encoding.Unicode.GetBytes(tokenJson));

                var sign = AsymmetricCypherHelper.Hash(tokenString + configuration["PrivateKEY"]);
                string signedToken = $"{tokenString}.{sign}";

                return JsonConvert.SerializeObject(signedToken);
            }

            

            return "";
        }
        [HttpGet("role")]

        public async Task<Role> GetRole()
        {
            var person = UserHelper.GetUserFromRequest(Request);
            return (await personRepository.GetRoles()).FirstOrDefault(o => o.ID == person.RoleId);
        }
        [HttpPost("email")]

        public async Task<bool> UpdateEmail(PersonDTO personWithEmail)
        {
            var person = UserHelper.GetUserFromRequest(Request);
            return await personRepository.UpdateEmail(person, personWithEmail.Email);
            
        }
        [HttpGet("current")]

        public async Task<PersonDTO> GetCurrentUser()
        {
            return UserHelper.GetUserFromRequest(Request);
        }
        [AllowAnonymous]
        [HttpPost("checkReply")]

        public async Task<bool> PersonGetReply(PersonDTO personInfo)
        {
            var person = await personRepository.PersonGetReply(personInfo);
            return person;
        }

    }

    public class Token
    {
        public DateTime ActiveUntil { get; set; }
        public PersonDTO User { get; set; }
    }
}
