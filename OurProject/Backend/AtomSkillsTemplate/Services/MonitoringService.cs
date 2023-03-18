using AtomSkillsTemplate.Connection;
using AtomSkillsTemplate.Connection.Interface;
using AtomSkillsTemplate.NewModels;
using AtomSkillsTemplate.Services.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomSkillsTemplate.Services
{
    public class MonitoringService : IMonitoringService
    {
        IConnectionFactory connectionFactory;
        List<RequestForMonitoring> requestRepository;
        public MonitoringService(IConnectionFactory connectionFactory)  
        {
            this.connectionFactory = connectionFactory;
        }
        public async void SetupEnvironment()
        {
            using var connection = connectionFactory.GetConnection();
            var requests = await connection.QueryAsync<RequestForMonitoring>($"select * from {DBHelper.Schema}.{DBHelper.Requests}");
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
        }
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

        public List<RequestPositionForMonitoring> RequestPositions { get; set; }

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
