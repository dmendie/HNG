using Dapper;
using HNG.Abstractions.Models;
using HNG.Abstractions.Services.Data;
using System.Data;
using System.Data.Common;

namespace HNG.Data.Sql
{
    public class DBOrganisationDataService : DataLayerBase, IOrganisationDataService
    {
        public DBOrganisationDataService(AppSettings appSettings) : base(appSettings)
        {
        }

        public async Task<Organisation?> GetById(IDbTransaction? transaction, string OrgId)
        {
            Organisation? retVal;

            using DbConnection connection = GetDefaultConnection();
            await connection.OpenAsync();

            // specify stored procedure parameters
            var parameters = new DynamicParameters();
            parameters.Add("@OrgId", OrgId, DbType.String, ParameterDirection.Input);

            //execute 
            var query = await connection.QueryAsync<Organisation>("Organisation_GetByIdOnly", parameters, transaction, commandType: CommandType.StoredProcedure);

            retVal = query.SingleOrDefault();
            return retVal;
        }

        public async Task<Organisation?> GetById(IDbTransaction? transaction, string UserId, string OrgId)
        {
            Organisation? retVal;

            using DbConnection connection = GetDefaultConnection();
            await connection.OpenAsync();

            // specify stored procedure parameters
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", UserId, DbType.String, ParameterDirection.Input);
            parameters.Add("@OrgId", OrgId, DbType.String, ParameterDirection.Input);

            //execute 
            var query = await connection.QueryAsync<Organisation>("Organisation_GetById", parameters, transaction, commandType: CommandType.StoredProcedure);

            retVal = query.SingleOrDefault();
            return retVal;
        }

        public async Task<string> Insert(IDbTransaction? transaction, string UserId, string Name, string Description, string CreatedBy)
        {
            string NewOrgId = Guid.NewGuid().ToString();

            using DbConnection connection = GetDefaultConnection();
            await connection.OpenAsync();

            // specify stored procedure parameters
            var parameters = new DynamicParameters();
            parameters.Add("@OrgId", NewOrgId, DbType.String, ParameterDirection.Input);
            parameters.Add("@Name", Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@Description", Description, DbType.String, ParameterDirection.Input);
            parameters.Add("@UserId", UserId, DbType.String, ParameterDirection.Input);
            parameters.Add("@CreatedBy", CreatedBy, DbType.String, ParameterDirection.Input);

            //execute 
            var query = await connection.ExecuteAsync("Organisation_Insert", parameters, transaction, commandType: CommandType.StoredProcedure);

            return NewOrgId;
        }

        public async Task AddUser(IDbTransaction? transaction, string UserId, string OrgId, string CreatedBy)
        {
            using DbConnection connection = GetDefaultConnection();
            await connection.OpenAsync();

            // specify stored procedure parameters
            var parameters = new DynamicParameters();
            parameters.Add("@OrgId", OrgId, DbType.String, ParameterDirection.Input);
            parameters.Add("@UserId", UserId, DbType.String, ParameterDirection.Input);
            parameters.Add("@CreatedBy", CreatedBy, DbType.String, ParameterDirection.Input);

            //execute 
            var query = await connection.ExecuteAsync("Organisation_AddUser", parameters, transaction, commandType: CommandType.StoredProcedure);
        }
    }
}
