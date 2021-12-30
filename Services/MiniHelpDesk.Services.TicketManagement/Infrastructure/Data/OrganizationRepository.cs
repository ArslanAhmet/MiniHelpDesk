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
    public class OrganizationRepository : IOrganizationRepository
    {
        private string _connectionString;

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_connectionString);
            }
        }
        public OrganizationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<PagedList<Organization>> GetOrganizations(OrganizationsResourceParameters organizationsResourceParameters)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var collection = await dbConnection
                    .QueryAsync<Organization>("SELECT *, count(\"Id\") OVER() AS FullCount FROM public.\"Organizations\" " +
                    "where \"IsDeleted\" = false ORDER BY @OrderBy LIMIT  @PageSize offset @Offset",
                    new
                    {
                        OrderBy = organizationsResourceParameters.OrderBy,
                        PageSize = organizationsResourceParameters.PageSize,
                        Offset = (organizationsResourceParameters.PageNumber - 1) * organizationsResourceParameters.PageSize
                    });

                var fullCount = collection.Count() == 0 ? 0 : collection.FirstOrDefault().FullCount;

                PagedList<Organization> pagedList = new PagedList<Organization>(collection.ToList(),
                    fullCount,
                    organizationsResourceParameters.PageNumber,
                    organizationsResourceParameters.PageSize);

                return pagedList;
            }
        }

        public async Task<Organization> GetOrganizationAsync(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();

                var records = await dbConnection
                    .QueryAsync<Organization>("select * from public.\"Organizations\"  where \"Id\"= @id and \"IsDeleted\" = false",
                    new { id = id })
                    .ConfigureAwait(false);

                return records.SingleOrDefault();
            }
        }

        public void AddOrganization(Organization organization)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();

                organization.Created = DateTime.Now;
                organization.Id = dbConnection.ExecuteScalar<int>("insert into public.\"Organizations\" " +
                    "(\"Name\",\"IsDeleted\",\"Created\" ,\"Domain\" ) " +
                    "values(:Name, :IsDeleted, :Created, :Domain ) RETURNING \"Id\" ", organization);

            }
        }

        public void UpdateOrganization(Organization organization)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();

                dbConnection.Execute("UPDATE \"Organizations\" Set" +
                    " \"Name\" = @Name, \"IsDeleted\" = @IsDeleted, \"Domain\" = @Domain where \"Id\"= @id ",
                 new
                 {
                     Name = organization.Name,
                     IsDeleted = organization.IsDeleted,
                     Modified = DateTime.Now,
                     Domain = organization.Domain,
                     Id = organization.Id
                 });
            }
        }
    }
}
