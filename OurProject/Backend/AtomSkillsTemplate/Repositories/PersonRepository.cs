using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using AtomSkillsTemplate.Connection;
using AtomSkillsTemplate.Connection.Interface;
using AtomSkillsTemplate.Helpers;
using AtomSkillsTemplate.Models;
using AtomSkillsTemplate.Models.DTOs;
using AtomSkillsTemplate.Repositories.Contracts;

using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Shared.authorization;

namespace AtomSkillsTemplate.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private IConnectionFactory connectionFactory { get; set; }

        private string schemaName = "\"atom\"";
        private string tableName = "\"person\"";

        public PersonRepository(IConnectionFactory factory)
        {
            connectionFactory = factory;
        }

        public async Task<IEnumerable<PersonDTO>> GetPersonDTOs()
        {
            
            using (var connection = connectionFactory.GetConnection())
            {
                var persons = await connection.QueryAsync<PersonDTO>($"select * from {schemaName}.{tableName}");

                var roles = await GetRoles();
                foreach (var person in persons)
                {
                    try
                    {
                        if(roles.FirstOrDefault(o=>o.ID == person.RoleId) != null)
                        {
                            person.RoleName = roles.FirstOrDefault(o => o.ID == person.RoleId).RoleName;

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                return persons;

            }
        }

        public async Task<IEnumerable<PersonDTO>> GetExecutors()
        {

            using (var connection = connectionFactory.GetConnection())
            {
                var persons = await connection.QueryAsync<PersonDTO>($"select id, nameclient, role_id from {schemaName}.{tableName} where role_id=2");

                var roles = await GetRoles();
                foreach (var person in persons)
                {
                    try
                    {
                        person.RoleName = roles.FirstOrDefault(o => o.ID == person.RoleId).RoleName;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                return persons;

            }
        }
        public async Task<bool> UpdateEmail(PersonDTO person, string email)
        {

            using (var connection = connectionFactory.GetConnection())
            {
                await connection.QueryAsync($"update {DBHelper.Schema}.{DBHelper.People} set email = :email where id = :id", new { email = email, id = person.ID });
                return true;

            }
        }
        public async Task<PersonDTO> GetPersonInfo(PersonDTO personInfo)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var persons = await connection.QueryAsync<PersonDTO>(
                    $"select * from {schemaName}.{tableName} where login=:login", new {login = personInfo.Login});

                var roles = await GetRoles();
                foreach (var person in persons)
                {
                    person.RoleName = roles?.FirstOrDefault(o => o.ID == person.RoleId)?.RoleCaption;
                }

                return persons.FirstOrDefault();

            }
        }
        public async Task<IEnumerable<Role>> GetRoles()
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var roles = await connection.QueryAsync<Role>(
                    $"select * from {schemaName}.role");
                return roles;

            }
        }


        public async Task<long?> Registration(PersonDTO personInfo)
        {
            var salt = AsymmetricCypherHelper.GenerateSalt();
            var hashedPassword = AsymmetricCypherHelper.Hash(personInfo.Password + salt);
            using (var connection = connectionFactory.GetConnection())
            {
                var personId = await connection.QueryFirstOrDefaultAsync<long?>(
                    $@"insert into {schemaName}.{tableName}(nameclient, role_id, login, password, salt) 
                    values (:nameclient, :role_id, :login, :password, :salt) returning id", new
                    {
                        nameclient = personInfo.NameClient,
                        role_id = 1,
                        login = personInfo.Login,
                        password = hashedPassword,
                        salt = salt

                    });
                if (personId == null)
                {
                    return null;
                }

                personInfo.ID = Convert.ToInt32(personId);
                return personInfo.ID;
            }
        }
        public async Task<bool> UpdateRole(long personID, long roleID)
        {
            using (var connection = connectionFactory.GetConnection())
            {

                await connection.ExecuteAsync(
                    $@"update {schemaName}.{tableName} set role_id = :roleID where id=:personID",
                    new { personID = personID , roleID = roleID});
                
            }

            return true;
        }
        public async Task<bool> PersonGetReply(PersonDTO personInfo)
        {
            using (var connection = connectionFactory.GetConnection())
            {

                var personId = await connection.QueryFirstOrDefaultAsync<long?>(
                    $@"select count(*) from {schemaName}.{tableName} where login=:login",
                    new {login = personInfo.Login});
                if (personId == 0 )
                {
                    return true;
                }
                else
                {
                    return false;
                }

                return false;
            }
        }
    }
}
