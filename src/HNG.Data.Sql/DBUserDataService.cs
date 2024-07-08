using Dapper;
using HNG.Abstractions.Enums;
using HNG.Abstractions.Models;
using HNG.Abstractions.Services.Data;
using System.Data;
using System.Data.Common;

namespace HNG.Data.Sql
{
    public class DBUserDataService : DataLayerBase, IUserDataService
    {
        public DBUserDataService(AppSettings appSettings) : base(appSettings)
        {
        }

        public async Task UpdatePassword(IDbTransaction? transaction, string UserId, string Password)
        {
            using DbConnection connection = GetDefaultConnection();
            await connection.OpenAsync();

            // specify stored procedure parameters
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", UserId, DbType.String, ParameterDirection.Input);
            parameters.Add("@Password", Password, DbType.String, ParameterDirection.Input);

            //execute 
            await connection.ExecuteAsync("User_PasswordUpdate", parameters, transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task<User?> GetById(IDbTransaction? transaction, string UserId)
        {
            User? retVal;

            using DbConnection connection = GetDefaultConnection();
            await connection.OpenAsync();

            // specify stored procedure parameters
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", UserId, DbType.String, ParameterDirection.Input);

            //execute 
            var query = await connection.QueryAsync<User>("User_GetById", parameters, transaction, commandType: CommandType.StoredProcedure);

            retVal = query.SingleOrDefault();
            return retVal;
        }


        public async Task<User?> SearchUser(IDbTransaction? transaction, string SearchUserId, string UserId)
        {
            User? retVal;

            using DbConnection connection = GetDefaultConnection();
            await connection.OpenAsync();

            // specify stored procedure parameters
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", UserId, DbType.String, ParameterDirection.Input);
            parameters.Add("@SearchUserId", SearchUserId, DbType.String, ParameterDirection.Input);

            //execute 
            var query = await connection.QueryAsync<User>("User_Search", parameters, transaction, commandType: CommandType.StoredProcedure);

            retVal = query.SingleOrDefault();
            return retVal;
        }


        public async Task<User?> GetByEmailAddress(IDbTransaction? transaction, string EmailAddress)
        {
            User? retVal;

            using DbConnection connection = GetDefaultConnection();
            await connection.OpenAsync();

            // specify stored procedure parameters
            var parameters = new DynamicParameters();
            parameters.Add("@EmailAddress", EmailAddress, DbType.String, ParameterDirection.Input);

            //execute 
            var query = await connection.QueryAsync<User>("User_GetByEmailAddress", parameters, transaction, commandType: CommandType.StoredProcedure);

            retVal = query.SingleOrDefault();
            return retVal;
        }

        public async Task<string> InsertUser(IDbTransaction? transaction, string FirstName, string LastName, string Phone, string EmailAddress, string Password, string CreatedBy)
        {
            string NewUserId = Guid.NewGuid().ToString();
            string NewOrgId = Guid.NewGuid().ToString();

            using DbConnection connection = GetDefaultConnection();
            await connection.OpenAsync();

            // specify stored procedure parameters
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", NewUserId, DbType.String, ParameterDirection.Input);
            parameters.Add("@OrgId", NewOrgId, DbType.String, ParameterDirection.Input);
            parameters.Add("@Firstname", FirstName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Lastname", LastName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Phone", Phone, DbType.String, ParameterDirection.Input);
            parameters.Add("@Email", EmailAddress, DbType.String, ParameterDirection.Input);
            parameters.Add("@CreatedBy", CreatedBy, DbType.String, ParameterDirection.Input);
            parameters.Add("@Status", DefaultStatusType.Active, DbType.Int16, ParameterDirection.Input);
            parameters.Add("@Password", Password, DbType.String, ParameterDirection.Input);

            //execute 
            var query = await connection.ExecuteAsync("User_Insert", parameters, transaction, commandType: CommandType.StoredProcedure);

            return NewUserId;
        }

        public async Task<IEnumerable<Organisation>> GetOrganisations(IDbTransaction? transaction, string UserId)
        {
            IEnumerable<Organisation> retVal;

            using DbConnection connection = GetDefaultConnection();
            await connection.OpenAsync();

            // specify stored procedure parameters
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", UserId, DbType.String, ParameterDirection.Input);

            //execute 
            var query = await connection.QueryAsync<Organisation>("User_GetOrganisations", parameters, transaction, commandType: CommandType.StoredProcedure);

            retVal = query;
            return retVal;
        }
    }
}
