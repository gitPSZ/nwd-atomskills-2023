using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtomSkillsTemplate.Connection.Interface;
using AtomSkillsTemplate.Helpers;
using AtomSkillsTemplate.Models;
using AtomSkillsTemplate.Models.ClaimsForTable;
using AtomSkillsTemplate.Models.DTOs;
using AtomSkillsTemplate.NewModels;
using AtomSkillsTemplate.Repositories.Contracts;
using AtomSkillsTemplate.Connection;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace AtomSkillsTemplate.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private IConnectionFactory connectionFactory { get; set; }

        private string schemaName = "\"atom\"";
        private string tableName = "\"request\"";

        public RequestRepository(IConnectionFactory factory)
        {
            connectionFactory = factory;
        }

        public async Task<IEnumerable<Request>> GetRequest()
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var request = await connection.QueryAsync<Request>($"select r.*, c.caption as ContractorName   from {DBHelper.Schema}.{DBHelper.Requests} r left join {DBHelper.Schema}.{DBHelper.Contractors} c on c.id= r.id_contractor");
                return request;

            }
        }
        public async Task<IEnumerable<Priority>> GetPriorities()
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var priorities = await connection.QueryAsync<Priority>($"select * from {schemaName}.priority");
                return priorities;

            }
        }

        public async Task<long> GetCountRequest()
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var count = await connection.QueryFirstAsync<long>($"select count(*) from {schemaName}.request");
                return count;

            }
        }
        

        public async Task<IEnumerable<State>> GetStates()
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var states = await connection.QueryAsync<State>($"select * from {schemaName}.state");
                return states;

            }
        }

        public async Task<IEnumerable<ClaimsForTableModel>> GetRequestForTable(PersonDTO roleUser)
        {
            
            using (var connection = connectionFactory.GetConnection())
            {
                //выводит все заявки
                var sql = $@"select claim.id, claim.comment, claim.id_executor, claim.create_date,claim.place_of_service as PlaceOfService,  tc.caption_state as TypeClaim, 
claim.text ,p.caption_priority as Priorities, p.caption_priority, s.caption_state as State,
claim.date_time_edit_state, claim.date_time_close_claim, personAuthor.nameclient as Author, personExecutor.nameclient as Executor from {schemaName}.claim claim
left join {schemaName}.type_claims tc  on tc.id = claim.id_type
left join {schemaName}.priority p   on p.id = claim.id_priority
left join {schemaName}.person personAuthor   on personAuthor.id = claim.id_author 
left join {schemaName}.state s   on s.id = claim.id_state 
left join {schemaName}.person personExecutor   on personExecutor.id = claim.id_executor";
                //выводит заявки по инициатору
                if (roleUser.RoleId == 1 || roleUser.RoleId == 0)
                { 
                    sql = sql+ " where claim.id_author = :id";
                }
                else if (roleUser.RoleId == 2)
                {
                    sql = sql + " where claim.id_executor = :id";
                }
                //выводит заявки по исполнителю

                var request = await connection.QueryAsync<ClaimsForTableModel>(sql, new {id = roleUser.ID});
                return request;

            }
        }

        public async Task<RequestDTO> SaveRequest(RequestDTO request)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var requestId = await connection.QueryFirstOrDefaultAsync<long?>(
                    $@"insert into {schemaName}.{tableName}(id_author, create_date, id_type, text, place_of_service, id_state, id_priority) 
                    values (:id_author, :create_date, :id_type, :text, :place_of_service, :id_state, :id_priority) returning id", new
                    {
                id_author = request.IdAuthor,        
                create_date = DateTime.Now,
                id_type = request.IdType,
                text=request.Text,
                place_of_service = request.placeOfService,
                id_state = 1,
                id_priority = request.IdPriority,

                    });
                if (requestId == null)
                {
                    return default(RequestDTO);
                }

                request.ID = Convert.ToInt32(requestId);
                return request;
            }

        }

        public async Task<ClaimsForTableModel> SaveExecutor(ClaimsForTableModel request)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var requestId = await connection.QueryFirstOrDefaultAsync<long?>(
                    $@"update {schemaName}.{tableName} set id_state=:id_state, id_executor=:id_executor where id=:id returning id", new
                    {
                        id_state = 2,
                        id_executor = request.IdExecutor,
                        id = request.ID

                    });
                if (requestId == null)
                {
                    return default(ClaimsForTableModel);
                }

                request.ID = Convert.ToInt32(requestId);
                return request;
            }

        }


        public async Task<RequestDTO> ToWork(RequestDTO request)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var requestId = await connection.QueryFirstOrDefaultAsync<long?>(
                    $@"update {schemaName}.{tableName} set id_state=:id_state, id_executor=:id_executor where id=:id returning id", new
                    {
                        id_state = 3,
                        id_executor = request.IdExecutor,
                        id = request.ID

                    });
                if (requestId == null)
                {
                    return default(RequestDTO);
                }

                request.ID = Convert.ToInt32(requestId);
                return request;
            }

        }

        public async Task<ClaimsForTableModel> CancelClaim(ClaimsForTableModel request)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var requestId = await connection.QueryFirstOrDefaultAsync<long?>(
                    $@"update {schemaName}.{tableName} set id_state=:id_state, comment=:comment where id=:id returning id", new
                    {
                        id_state = 6,
                        comment = request.Comment,
                        id = request.ID

                    });
                if (requestId == null)
                {
                    return default(ClaimsForTableModel);
                }

                request.ID = Convert.ToInt32(requestId);
                return request;
            }

        }

        public async Task<RequestDTO> AcceptClaim(RequestDTO request)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var requestId = await connection.QueryFirstOrDefaultAsync<long?>(
                    $@"update {schemaName}.{tableName} set id_state=:id_state, id_executor=:id_executor where id=:id returning id", new
                    {
                        id_state = 5,
                        id_executor = request.IdExecutor,
                        id = request.ID

                    });
                if (requestId == null)
                {
                    return default(RequestDTO);
                }

                request.ID = Convert.ToInt32(requestId);
                return request;
            }

        }

        public async Task<RequestDTO> CancelClaim(RequestDTO request)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var requestId = await connection.QueryFirstOrDefaultAsync<long?>(
                    $@"update {schemaName}.{tableName} set id_state=:id_state, id_executor=:id_executor where id=:id returning id", new
                    {
                        id_state = 5,
                        id_executor = request.IdExecutor,
                        id = request.ID

                    });
                if (requestId == null)
                {
                    return default(RequestDTO);
                }

                request.ID = Convert.ToInt32(requestId);
                return request;
            }

        }
    }
}
