using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Server=pgcserv.local.imf.ru; Port=5432; User Id=as2023; Password=as2023; Database=otg02";
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            var transaction = conn.BeginTransaction();
            try
            {
                
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lX3VzZXIiOiJVc2VyMSIsIm5hbWVfdGVhbSI6IlRlYW0xIiwicGVybWlzc2lvbl9sZXZlbHMiOjEsImlhdCI6MTY3ODQyOTE2N30.L_2A4uz0IBOLfIatQSb3Qj6Ihnhv14bWHUAoRVa9DCU");
                client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                var result = client.GetAsync("http://172.23.48.61:3000/refs/history_executors").GetAwaiter().GetResult();
                var jsonString = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var histories = JsonConvert.DeserializeObject<List<HistoryExecutor>>(jsonString);

                foreach(var history in histories)
                {
                    conn.Query($"insert into \"as2023\".history_executors(id, system_number, date_start, date_end, id_claim, comment, id_state) " +
                        $" values ({history.id},'{history.system_number}',:date_start, :date_end, {history.id_claim},'{history.comment}',{history.id_state})", 
                        new { date_start = history.date_start, date_end = history.date_end });
                }
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
            }
            
        }
    }

    public class HistoryExecutor
    {
        public long id { get; set; }
        public string system_number { get; set; }
        public DateTime date_start { get; set; }
        public DateTime? date_end { get; set; }
        public long id_claim { get; set; }
        public string comment { get; set; }
        public long id_state { get; set; }
    }
}
