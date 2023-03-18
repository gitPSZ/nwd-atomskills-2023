using AtomSkillsTemplate.Connection;
using AtomSkillsTemplate.Connection.Interface;
using AtomSkillsTemplate.Models.DTOs;
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
        public IEmailService mailService;
        public List<Request> cachedRequests = new List<Request>();
        public ReloadRequestsService(IConnectionFactory connectionFactory, IEmailService mailService)
        {
            this.mailService = mailService;
            this.connectionFactory = connectionFactory;
            _ = Task.Run(async () =>
            {
                try
                {
                    await LoadMachines();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка при загрузке списка оборудований: " + e.ToString());          
                }
                
                while (true)
                {
                    try
                    {
                        await CheckRequests();
                        await CheckNewRequests();
                        
                        await LoadMachineStatus();
                    }
                    catch (System.Exception e )
                    {
                        Console.WriteLine(e.ToString());
                    }
                    await Task.Delay(interval);
                }
            });
        }
        private async Task CheckNewRequests()
        {
            using var conn = connectionFactory.GetConnection();
            var requests = (await conn.QueryAsync<Request>($"select * from {DBHelper.Schema}.{DBHelper.Requests}")).ToList();

            if (cachedRequests.Count() == 0)
            {
                cachedRequests = requests.ToList();
                return;
            }
//#if DEBUG
//            requests.Add(new Request 
//            {
//                ContractorName = "contractor",
//                Description = "description",
//                Id = 123
//            });
//#endif
            if (requests.Count() <= cachedRequests.Count())
            {
                cachedRequests = requests.ToList();
                return;
            }
            var cachedIDs = cachedRequests.Select(o => o.Id);
            var newRequests = requests.Where(o => cachedIDs.Contains(o.Id) == false).ToList();

            var textToSend = "Поступили новые заявки: \n";

            foreach(var newRequest in newRequests)
            {
                textToSend += $"ID: {newRequest.Id}, описание: {newRequest.Description}, контрагент: {newRequest.ContractorName}";
            }
            var peopleToSend = await conn.QueryAsync<PersonDTO>($"select * from {DBHelper.Schema}.{DBHelper.People} where role_id = 1");
            foreach(var person in peopleToSend)
            {
                try
                {
                    await mailService.SendEmailAsync(person.Email, "Поступление заявок", textToSend);
                    Console.WriteLine("Почтовое уведомление успешно отправлено");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Возникла ошибка при отправлении уведомлений о поступлении новых сообщений");
                }
            }
            cachedRequests = requests;
        }
        private async Task LoadMachineStatus()
        {
            using var conn = connectionFactory.GetConnection();
            var ports = await conn.QueryAsync<long>($"select port from {DBHelper.Schema}.{DBHelper.Machines}");
            var states = await conn.QueryAsync<MachineState>($"select * from {DBHelper.Schema}.{DBHelper.MachineState}");

            var client = new HttpClient();
            foreach(var port in ports)
            {
                var result = await client.GetAsync($"http://localhost:{port}/status");
                var jsonString = await result.Content.ReadAsStringAsync();
                var status = JsonConvert.DeserializeObject<MachineStatus>(jsonString);

                await conn.QueryAsync($"update {DBHelper.Schema}.{DBHelper.Machines} set id_state= :idStatus where port = :port",
                    new { idStatus = states.FirstOrDefault(o => o.Code == status.State.Code.ToLower()).Id, port = port }); ;
            }
        }
        private async Task LoadMachines()
        {
            using var conn = connectionFactory.GetConnection();
            var client = new HttpClient();
            var result = await client.GetAsync("http://localhost:1040/mnf/machines");
            var jsonString = await result.Content.ReadAsStringAsync();
            var machines = JsonConvert.DeserializeObject<MachineDTO>(jsonString);

            List<string> activeIDs = new List<string>();
            activeIDs.AddRange(machines.Milling.Select(o => o.Key));
            activeIDs.AddRange(machines.Lathe.Select(o => o.Key));
            foreach (var millingMachine in machines.Milling)
            {
                var sqlStringContractors = $"insert into {DBHelper.Schema}.{DBHelper.Machines}(id, machine_type, port) values(:id, :machine_type, :port) " +
                   $" on conflict(id) do update set machine_type=:machine_type, port=:port";
                var contractorParams = new { id = millingMachine.Key, machine_type = "milling", port = millingMachine.Value};
                await conn.QueryAsync(sqlStringContractors, contractorParams);
            }

            foreach (var latheMachine in machines.Lathe)
            {
                var sqlStringContractors = $"insert into {DBHelper.Schema}.{DBHelper.Machines}(id, machine_type, port) values(:id, :machine_type, :port) " +
                   $" on conflict(id) do update set machine_type=:machine_type, port=:port";
                var contractorParams = new { id = latheMachine.Key, machine_type = "milling", port = latheMachine.Value };
                await conn.QueryAsync(sqlStringContractors, contractorParams);
            }

            
            //var parameters = new DynamicParameters();
            //parameters.Add("@IDs", activeIDs);
            //await conn.QueryAsync($"update {DBHelper.Schema}.{DBHelper.Machines} set is_active = '0' where id in @IDs = false", parameters);

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
    public class MachineDTO
    {
        public Dictionary<string, int> Milling{ get; set; }
        public Dictionary<string, int> Lathe { get; set; }
    }
    public class MachineStatus
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public MachineStatus State { get; set; }
        public DateTime BeginDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
    public class MachineState
    {
        public long Id{ get; set; }
        public string Code { get; set; }
        public string Caption { get; set; }
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
