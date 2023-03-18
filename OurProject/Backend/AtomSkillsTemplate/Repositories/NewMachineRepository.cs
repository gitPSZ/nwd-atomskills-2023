using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AtomSkillsTemplate.Connection;
using AtomSkillsTemplate.Connection.Interface;
using AtomSkillsTemplate.Helpers;
using AtomSkillsTemplate.Models;
using AtomSkillsTemplate.Models.DTOs;
using AtomSkillsTemplate.NewModels;
using AtomSkillsTemplate.Repositories.Contracts;
using AtomSkillsTemplate.Services;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Shared.authorization;

namespace AtomSkillsTemplate.Repositories
{
    public class NewMachineRepository : IMachineRepository
    {
        private IConnectionFactory connectionFactory { get; set; }

        private string schemaName = "\"atom\"";
        private string tableName = "\"machine\"";

        public NewMachineRepository(IConnectionFactory factory)
        {
            connectionFactory = factory;
        }


        public async Task<IEnumerable<Machine>> GetMachineDTOs()
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var machine = await connection.QueryAsync<Machine>($"select m.*, s.caption as State from {schemaName}.{tableName} as m left join atom.machine_state s on s.id = m.id_state");

                return machine;

            }
        }

        public async Task<Machine> SetRepair(Machine machine)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var client = new HttpClient();
                var result = await client.PostAsync($@"http://localhost:{machine.Port}/set/repairing", new StringContent(JsonConvert.SerializeObject(new ProductID
                {
                   
                })));
                await connection.QueryAsync($"update {DBHelper.Schema}.{DBHelper.Machines} set id_state = 4 where id = :machine_id", new {machine_id = machine.Id});
                var jsonString = await result.Content.ReadAsStringAsync();
                var requestPositions = JsonConvert.DeserializeObject<List<int>>(jsonString);
                return machine;

            }
        }
    }
}
