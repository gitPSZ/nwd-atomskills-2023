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
using System.Threading.Tasks;

namespace AtomSkillsTemplate.Services
{
    public class MonitoringService : IMonitoringService
    {
        IConnectionFactory connectionFactory;
        List<RequestForMonitoring> requestRepository;
        List<MachineWrapper> machineWrappers;
        public MonitoringService(IConnectionFactory connectionFactory)  
        {
            this.connectionFactory = connectionFactory;
        }
        public async Task AddRequest(long requestID)
        {
            using var connection = connectionFactory.GetConnection();
            var request = await connection.QueryFirstOrDefaultAsync<RequestForMonitoring>($"select * from {DBHelper.Schema}.{DBHelper.Requests} where id = " + requestID);
            var requestPositions = await connection.QueryAsync<RequestPositionForMonitoring>($"select * from {DBHelper.Schema}.{DBHelper.RequestPositions}");

            foreach (var requestPosition in requestPositions)
            {
                requestPosition.QuantityLathe = requestPosition.QuantityExec;
                requestPosition.QuantityLatheInProgress = requestPosition.QuantityExec;
                requestPosition.QuantityMilling = requestPosition.QuantityExec;
                requestPosition.QuantityMillingInProgress = requestPosition.QuantityExec;
            }

            
            try
            {
                var machinesThatCanProcess = await connection.QueryAsync<Machine>(
                    $"select * from {DBHelper.Schema}.{DBHelper.Machines} where id in (select id_machine from " +
                    $" {DBHelper.Schema}.{DBHelper.MachineRequest} where id_request = :idRequest)", new { idRequest = request.Id });

                request.MachinesThatCanProcessThisGoddamnThing = new List<Machine>();

                if (machinesThatCanProcess != null && machinesThatCanProcess.Any())
                {
                    request.MachinesThatCanProcessThisGoddamnThing = machinesThatCanProcess.ToList();
                }

                request.RequestPositions = new List<RequestPositionForMonitoring>();
                var requestPositionsInRequest = requestPositions.Where(o => o.RequestId == request.Id);
                request.RequestPositions.AddRange(requestPositionsInRequest);
                requestRepository.Add(request);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка в списке позиций заказа: " + e.ToString());
            }

        }
        public async void SetupEnvironment()
        {
            using var connection = connectionFactory.GetConnection();
            var requests = await connection.QueryAsync<RequestForMonitoring>($"select * from {DBHelper.Schema}.{DBHelper.Requests} where state_code = 'IN_PRODUCTION'");
            var requestPositions = await connection.QueryAsync<RequestPositionForMonitoring>($"select * from {DBHelper.Schema}.{DBHelper.RequestPositions}");
            requestRepository = new List<RequestForMonitoring>();

            foreach(var requestPosition in requestPositions)
            {
                requestPosition.QuantityLathe = requestPosition.QuantityExec;
                requestPosition.QuantityLatheInProgress = requestPosition.QuantityExec;
                requestPosition.QuantityMilling= requestPosition.QuantityExec;
                requestPosition.QuantityMillingInProgress = requestPosition.QuantityExec;
            }

            foreach (var request in requests)
            {
                try
                {
                    var machinesThatCanProcess = await connection.QueryAsync<Machine>(
                        $"select * from {DBHelper.Schema}.{DBHelper.Machines} where id in (select id_machine from " +
                        $" {DBHelper.Schema}.{DBHelper.MachineRequest} where id_request = :idRequest)", new { idRequest = request.Id });

                    request.MachinesThatCanProcessThisGoddamnThing = new List<Machine>();

                    if (machinesThatCanProcess != null && machinesThatCanProcess.Any())
                    {
                        request.MachinesThatCanProcessThisGoddamnThing = machinesThatCanProcess.ToList();
                    }

                    request.RequestPositions = new List<RequestPositionForMonitoring>();
                    var requestPositionsInRequest = requestPositions.Where(o => o.RequestId == request.Id);
                    request.RequestPositions.AddRange(requestPositionsInRequest);
                    requestRepository.Add(request);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка в списке позиций заказа: " + e.ToString());
                }
                
            }
            StartMachines();
        }
        public async void StartMachines()
        {
            machineWrappers = new List<MachineWrapper>();
            using var connection = connectionFactory.GetConnection();
            var machines = await connection.QueryAsync<Machine>($"select * from {DBHelper.Schema}.{DBHelper.Machines}");
            foreach(var machine in machines)
            {
                var wrapper = new MachineWrapper
                {
                    Machine = machine,
                    ShouldStop = false,
                    MonitoringTask = Task.Run(async () => { await ProcessEquipment(machine); })
                };
                machineWrappers.Add(wrapper);
            }
        }
        public async Task ProcessEquipment(Machine machine)
        {
            Console.WriteLine("Начался опрос оборудования с ID = " + machine.Id);

#if DEBUG
            if(machine.Port == 1054)
            {
                return;
            }
#endif

            if(machine.IdState != 3 && machine.IdState != 4)
            {
                try
                {
                    var client = new HttpClient();
                    var result = await client.PostAsync($"http://localhost:{machine.Port}/set/waiting", new StringContent(JsonConvert.SerializeObject(new ProductID())));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка при выставлени состояния станка в ожидание " + e.ToString());
                }
            }
            
            

            while (true)
            {

                var connection = connectionFactory.GetConnection();
                try
                {
                    if (machineWrappers.FirstOrDefault(o=>o.Machine.Id == machine.Id).ShouldStop)
                    {
                        machineWrappers.Remove(machineWrappers.FirstOrDefault(o => o.Machine.Id == machine.Id));
                        break;
                    }
                    var requestsAssignedToThisMachine = new List<RequestForMonitoring>();
                    lock (requestRepository)
                    {
                        var assignees = requestRepository.Where(o => o.MachinesThatCanProcessThisGoddamnThing.FirstOrDefault(p => p.Id == machine.Id) != null).ToList();
                        if (assignees != null && assignees.Any())
                        {
                            requestsAssignedToThisMachine = assignees.ToList();
                        }
                    }

                    if (requestsAssignedToThisMachine == null || requestsAssignedToThisMachine.Any() == false)
                    {
                        Console.WriteLine($"У машины {machine.Id} пустая очередь, ожидаем заявок");
                        await Task.Delay(10000);
                        continue;
                    }
                    
                    if (machine.MachineType == "lathe")
                    {
                        RequestPositionForMonitoring positionToProcess = null;
                        lock (requestRepository)
                        {
                            var orderToProcess = requestRepository.OrderBy(o => o.Priority).FirstOrDefault();
                             positionToProcess = orderToProcess.RequestPositions.FirstOrDefault(p => p.Quantity != p.QuantityLatheInProgress);
                                
                        }
                        if(positionToProcess != null)
                        {
                            var currentMachine = machineWrappers.FirstOrDefault(o => o.Machine.Id == machine.Id);
                            currentMachine.RequestID = positionToProcess.ProductId;

                            var client = new HttpClient();
                            if (machine.IdState != 2)
                            {
                                machine.IdState = 2;
                                var result = await client.PostAsync($"http://localhost:{machine.Port}/set/working", new StringContent(JsonConvert.SerializeObject(new ProductID
                                {
                                    productId = positionToProcess.ProductId
                                })));
                            }

                            


                            Console.WriteLine("Взята в работу позиция " + positionToProcess.Id + " машиной " + machine.Id);
                            positionToProcess.QuantityLatheInProgress++;
                            var product = await connection.QueryFirstOrDefaultAsync<Product>($"select * from {DBHelper.Schema}.{DBHelper.Products} where id = :id_product",
                            new { id_product = positionToProcess.ProductId });

                            var timeToWait = product.LatheTime * 1000;
#if DEBUG
                            timeToWait = timeToWait / 10;
#endif
                            await Task.Delay((int)(timeToWait));
                            lock (requestRepository)
                            {
                                positionToProcess.QuantityLathe++;

                                //var client = new HttpClient();
                                //_ = client.GetAsync($"http://localhost:1040/crm/requests/{positionToProcess.RequestId}/items/{positionToProcess.Id}/add-execution-qty/1}");

                            }
                        }
                        else
                        {
                            Console.WriteLine("Нечего производить, ожидаем на машине " + machine.Id); 
                            await Task.Delay((int)(10000));
                        }
                        
                    }
                    if (machine.MachineType == "milling")
                    {
                        RequestPositionForMonitoring positionToProcess = null;
                        lock (requestRepository)
                        {
                            var orderToProcess = requestRepository.OrderBy(o => o.Priority).FirstOrDefault();
                            positionToProcess = orderToProcess.RequestPositions.FirstOrDefault(p => p.QuantityLathe > p.QuantityMillingInProgress);

                            
                        }
                        if (positionToProcess != null)
                        {
                            Console.WriteLine("Взята в работу позиция " + positionToProcess.Id + " машиной " + machine.Id);
                            positionToProcess.QuantityMillingInProgress++;

                            var currentMachine = machineWrappers.FirstOrDefault(o => o.Machine.Id == machine.Id);
                            currentMachine.RequestID = positionToProcess.ProductId;

                            var client = new HttpClient();
                            if (machine.IdState !=2)
                            {
                                machine.IdState = 2;
                                var result = await client.PostAsync($"http://localhost:{machine.Port}/set/working", new StringContent(JsonConvert.SerializeObject(new ProductID
                                {
                                    productId = positionToProcess.ProductId
                                })));
                            }

                            var product = await connection.QueryFirstOrDefaultAsync<Product>($"select * from {DBHelper.Schema}.{DBHelper.Products} where id = :id_product",
                            new { id_product = positionToProcess.ProductId });

                            var timeToWait = product.MillingTime * 1000;
#if DEBUG
                            timeToWait = timeToWait / 10;
#endif
                            await Task.Delay((int)(timeToWait));
                            lock (requestRepository)
                            {
                                positionToProcess.QuantityMilling++;

                                Task.Run(async () =>
                                {
                                    try
                                    {
                                        var client = new HttpClient();
                                        var result = await client.PutAsync($"http://localhost:1040/crm/requests/{positionToProcess.RequestId}/items/{positionToProcess.Id}/add-execution-qty/1", null);
                                        Console.WriteLine("Произведена деталь по позиции " + positionToProcess.Id);

                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Ошибка при обновлении через API");
                                    }

                                });

                            }
                        }
                        else
                        {
                            Console.WriteLine("Нечего производить, ожидаем на машине " + machine.Id);
                            await Task.Delay((int)(10000));
                        }

                    }
                    connection.Dispose();
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка в опросе оборудования: " + e.ToString());
                }
                
            }
        }
    }
    public class ProductID
    {
        public long productId{ get; set; }

    }
    public class MachineWrapper
    {
        public Machine Machine { get; set; }

        public long RequestID { get; set; }
        public bool ShouldStop { get; set; }
        public Task MonitoringTask { get; set; }
    }
    public class RequestForMonitoring
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public long IdContractor { get; set; }
        public string StateCode { get; set; }
        public string StateCaption { get; set; }
        public string ContractorName { get; set; }
        public int Priority { get; set; }

        public List<RequestPositionForMonitoring> RequestPositions { get; set; }
        public List<Machine> MachinesThatCanProcessThisGoddamnThing { get; set; }

    }
    public class RequestPositionForMonitoring
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long RequestId { get; set; }
        public long Quantity{ get; set; }
        public long QuantityExec { get; set; }
        public long QuantityLathe { get; set; }
        public long QuantityLatheInProgress { get; set; }
        public long QuantityMilling{ get; set; }
        public long QuantityMillingInProgress { get; set; }

        

    }
}
