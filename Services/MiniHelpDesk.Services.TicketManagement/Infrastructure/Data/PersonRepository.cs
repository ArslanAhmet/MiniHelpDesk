using Dapper;
using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Helpers;
using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MiniHelpDesk.Services.TicketManagement.Infrastructure.Data
{
    public class PersonRepository : IPersonRepository
    {
        private string _connectionString;

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_connectionString);
            }
        }
        public PersonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<PagedList<Person>> GetPersons(PersonsResourceParameters personsResourceParameters)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var collection = await dbConnection
                    .QueryAsync<Person>("SELECT *, count(\"Id\") OVER() AS FullCount FROM public.\"Persons\" " +
                    "where \"IsDeleted\" = false ORDER BY @OrderBy LIMIT  @PageSize offset @Offset",
                    new
                    {
                        OrderBy = personsResourceParameters.OrderBy,
                        PageSize = personsResourceParameters.PageSize,
                        Offset = (personsResourceParameters.PageNumber - 1) * personsResourceParameters.PageSize
                    });

                var fullCount = collection.Count() == 0 ? 0 : collection.FirstOrDefault().FullCount;

                PagedList<Person> pagedList = new PagedList<Person>(collection.ToList(),
                    fullCount,
                    personsResourceParameters.PageNumber,
                    personsResourceParameters.PageSize);

                return pagedList;
            }
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                //var sql = String.Format($"select * from public.\"Persons\"  where \"Id\"= {id} and \"IsDeleted\" = false");
                //var records = await dbConnection
                //    .QueryAsync<Person>(sql)
                //    .ConfigureAwait(false);

                var records = await dbConnection
                    .QueryAsync<Person>("select * from public.\"Persons\"  where \"Id\"= @id and \"IsDeleted\" = false",
                    new { id = id })
                    .ConfigureAwait(false);

                return records.SingleOrDefault();
            }
        }

        public void AddPerson(Person person)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();

                person.Created = DateTime.Now;
                person.Id = dbConnection.ExecuteScalar<int>("insert into public.\"Persons\" " +
                    "(\"Name\",\"IsDeleted\",\"Created\" ,\"Language\"  ,\"Type\", \"Email\", \"LatestUpdate\") " +
                    "values(:Name, :IsDeleted, :Created, :Language, :Type, :Email, :LatestUpdate ) RETURNING \"Id\" ", person);

            }
        }

        public void UpdatePerson(Person person)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();

                dbConnection.Execute("UPDATE \"Persons\" Set" +
                    " \"Name\" = @Name, \"IsDeleted\" = @IsDeleted, \"Language\" = @Language" +
                    ", \"Type\" = @Type, \"Email\" = @Email, \"LatestUpdate\" = @LatestUpdate where \"Id\"= @id ",
                 new
                 {
                     Name = person.Name,
                     IsDeleted = person.IsDeleted,
                     Modified = DateTime.Now,
                     Language = person.Language,
                     Type = person.Type,
                     Email = person.Email,
                     LatestUpdate = person.LatestUpdate,
                     Id = person.Id
                 });
            }
        }
    }
}
