using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtomSkillsTemplate.Connection.Interface;
using AtomSkillsTemplate.Helpers;
using AtomSkillsTemplate.Models;
using AtomSkillsTemplate.Models.DTOs;
using AtomSkillsTemplate.Repositories.Contracts;

using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace AtomSkillsTemplate.Repositories
{
    public class TypeRequestRepository : ITypeRequestRepository
    {
        private IConnectionFactory connectionFactory { get; set; }

        private string schemaName = "\"as2023\"";
        private string tableName = "\"type_claims\"";

        public TypeRequestRepository(IConnectionFactory factory)
        {
            connectionFactory = factory;
        }

        public async Task<IEnumerable<TypeRequestDTO>> GetTypeRequest()
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var typerequest = await connection.QueryAsync<TypeRequestDTO>($"select * from {schemaName}.{tableName}");
                return typerequest;

            }
        }

    }
}
