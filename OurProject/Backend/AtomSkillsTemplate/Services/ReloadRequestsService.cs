using AtomSkillsTemplate.Connection;
using AtomSkillsTemplate.Connection.Interface;
using AtomSkillsTemplate.NewModels;
using AtomSkillsTemplate.Services.Interfaces;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AtomSkillsTemplate.Services
{
    /// <summary>
    /// Запускается при инициализации приложения при помощи инъекции в Startup.cs Configure(...)
    /// </summary>
    public class ReloadRequestsService : IReloadRequestsService
    {
        int interval = 10000;
        public IConnectionFactory connectionFactory;
        public ReloadRequestsService(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await CheckRequests();
                    }
                    catch (System.Exception e )
                    {
                        Console.WriteLine(e.ToString());
                    }
                    await Task.Delay(interval);
                }
            });
        }
        private async Task CheckRequests()
        {
            using var conn = connectionFactory.GetConnection();
            var client = new HttpClient();
            var result = await client.GetAsync("http://localhost:1040/crm/requests");
            var jsonString = await result.Content.ReadAsStringAsync();
            var requests = JsonConvert.DeserializeObject<List<RequestDTO>>(jsonString);

            var contractors = requests.Select(o => o.Contractor);
            foreach(var contractor in contractors)
            {
                var sqlStringContractors = $"insert into {DBHelper.Schema}.{DBHelper.Contractors}(id, inn, caption) values(:id, :inn, :caption) " +
                    $" on conflict(id) do update set inn=:inn, caption=:caption";
                var contractorParams = new { id = contractor.Id, inn = contractor.Inn, caption = contractor.Caption };
                await conn.QueryAsync(sqlStringContractors, contractorParams);
            }

            foreach(var request in requests)
            {
                var sqlString = $"insert into {DBHelper.Schema}.{DBHelper.Requests}(id, number, date, release_date, description, id_contractor, state_code, state_caption)" +
                $" values(:id, :number, :date, :release_date, :description, :id_contractor, :state_code, :state_caption) " +
                $" on conflict(id) do update set number=:number, date=:date, release_date = :release_date, description = :description, id_contractor = :id_contractor, state_code = :state_code, state_caption = :state_caption";

                var requestParams = new
                {
                    id = request.Id,
                    number = request.Number,
                    date = request.Date,
                    release_date = request.ReleaseDate,
                    id_contractor = request.Contractor.Id,
                    state_code = request.State.Code,
                    state_caption = request.State.Caption
                };

                await conn.QueryAsync(sqlString, requestParams);
            }
            
        }
    }
    public class RequestDTO
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public Contractor Contractor { get; set; }
        public StateDTO State { get; set; }
    }
    public class StateDTO
    {
        public string Code { get; set; }
        public string Caption{ get; set; }
    }
}
