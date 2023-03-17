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
                var sqlString = $"insert into {DBHelper.Schema}.{DBHelper.Requests}(id, number, create_date, release_date, description, id_contractor, state_code, state_caption)" +
                $" values(:id, :number,:create_date,:release_date,:description,:id_contractor,:state_code,:state_caption) " + 
                $" on conflict(id) do update set number =:number, create_date=:create_date, release_date = :release_date, description = :description, id_contractor = :id_contractor, state_code = :state_code, state_caption = :state_caption";

                var requestParams = new
                {
                    id = request.Id,
                    number = request.Number,
                    create_date = request.Date,
                    release_date = request.ReleaseDate,
                    id_contractor = request.Contractor.Id,
                    description = request.Description,
                    state_code = request.State.Code,
                    state_caption = request.State.Caption
                };

                await conn.QueryAsync(sqlString, requestParams);
            }
            await LoadAllPositions(requests.Select(o=>o.Id));
            
        }
        private async Task LoadAllPositions(IEnumerable<long> requestIDs)
        {
            using var conn = connectionFactory.GetConnection();
            var client = new HttpClient();
            
            List<RequestPositionDTO> requestPositionDTOs = new List<RequestPositionDTO>();

            foreach(var id in requestIDs)
            {
                var result = await client.GetAsync($"http://localhost:1040/crm/requests/{id}/items");
                var jsonString = await result.Content.ReadAsStringAsync();
                var requestPositions = JsonConvert.DeserializeObject<List<RequestPositionDTO>>(jsonString);
                requestPositionDTOs.AddRange(requestPositions);
                foreach(var requestPosition in requestPositions)
                {
                    requestPosition.Request.Id = id;
                }
            }

            var products = requestPositionDTOs.Select(o => o.Product);
            foreach (var product in products)
            {
                var sqlStringContractors = $"insert into {DBHelper.Schema}.{DBHelper.Products}(id, code, caption, milling_time, lathe_time) values(:id, :code, :caption, :milling_time, :lathe_time) " +
                    $" on conflict(id) do update set code = :code, caption = :caption, milling_time = :milling_time, lathe_time = :lathe_time";
                var contractorParams = new
                {
                    id = product.Id, 
                    code = product.Code,
                    caption = product.Caption,
                    milling_time = product.MillingTime,
                    lathe_time = product.LatheTime
                };
                await conn.QueryAsync(sqlStringContractors, contractorParams);
            }

            foreach (var requestPosition in requestPositionDTOs)
            {
                var sqlRequestPosition = $"insert into {DBHelper.Schema}.{DBHelper.RequestPositions}(id, product_id, request_id, quantity, quantity_exec) " +
                    $" values(:id, :product_id, :request_id, :quantity, :quantity_exec) " +
                    $" on conflict(id) do update set product_id = :product_id, request_id = :request_id, quantity = :quantity, quantity_exec = :quantity_exec";
                var requestPositionParams = new
                {
                    id = requestPosition.Id,
                    product_id = requestPosition.Product.Id,
                    request_id = requestPosition.Request.Id,
                    quantity = requestPosition.Quantity,
                    quantity_exec = requestPosition.QuantityExec,
                };
                try
                {
                    await conn.QueryAsync(sqlRequestPosition, requestPositionParams);

                }
                catch (Exception e)
                {

                }
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
    public class RequestPositionDTO
    {
        public long Id { get; set; }
        public RequestDTO Request { get; set; }
        public Product Product{ get; set; }
        public long Quantity{ get; set; }
        public long QuantityExec { get; set; }

    }
    public class StateDTO
    {
        public string Code { get; set; }
        public string Caption{ get; set; }
    }
}
