using Dapper;
using HNG.Abstractions.Models;
using HNG.Abstractions.Services.Data;
using System.Data;
using System.Data.Common;

namespace HNG.Data.Sql
{
    public class DBServiceLogDataService : DataLayerBase, IServiceLogDataService
    {
        public DBServiceLogDataService(AppSettings appSettings) : base(appSettings) { }

        public async Task<ServiceLogSearchResult?> GetById(IDbTransaction? transaction, long serviceLogId, string? component)
        {
            ServiceLogSearchResult? retVal;

            using DbConnection connection = GetDefaultConnection();
            await connection.OpenAsync();

            // specify stored procedure parameters
            var parameters = new DynamicParameters();
            parameters.Add("@ServiceLogId", serviceLogId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@Component", component, DbType.String, ParameterDirection.Input);

            //execute 
            var query = await connection.QueryAsync<ServiceLogSearchResult>("ServiceLogs_GetById", parameters, transaction, commandType: CommandType.StoredProcedure);

            retVal = query.SingleOrDefault();
            return retVal;
        }

        public async Task<long> Insert(IDbTransaction? transaction, int ApplicationId, string? TraceId, string Component, string? EntityId, string RequestUri, string RequestIpAddress, string RequestUserName, string? ClientId, string? SessionId, string? CompanyId, string? ProfileId, int? UserId, DateTime RequestDate, DateTime? ResponseDate, string? RequestHeaders, string? ResponseHeaders, string? RequestData, string? ResponseData, int? ResponseCode, int? ResponseStatusCode, string? ExtraInfo, string? ErrorMessage, string ServerHostName, string ServerIpAddress, string ServerUserName)
        {
            long retVal;

            using DbConnection connection = GetDefaultConnection();
            await connection.OpenAsync();

            // specify stored procedure parameters
            var parameters = new DynamicParameters();
            parameters.Add("@ServiceLogId", null, DbType.Int64, ParameterDirection.Output);
            parameters.Add("@ApplicationId", ApplicationId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@TraceId", TraceId, DbType.String, ParameterDirection.Input);
            parameters.Add("@Component", Component, DbType.String, ParameterDirection.Input);
            parameters.Add("@EntityId", EntityId, DbType.String, ParameterDirection.Input);
            parameters.Add("@RequestUri", RequestUri, DbType.String, ParameterDirection.Input);
            parameters.Add("@RequestIpAddress", RequestIpAddress, DbType.String, ParameterDirection.Input);
            parameters.Add("@RequestUserName", RequestUserName, DbType.String, ParameterDirection.Input);
            parameters.Add("@ClientId", ClientId, DbType.String, ParameterDirection.Input);
            parameters.Add("@SessionId", SessionId, DbType.String, ParameterDirection.Input);
            parameters.Add("@CompanyId", CompanyId, DbType.String, ParameterDirection.Input);
            parameters.Add("@ProfileId", ProfileId, DbType.String, ParameterDirection.Input);
            parameters.Add("@UserId", UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@RequestDate", RequestDate, DbType.DateTime2, ParameterDirection.Input);
            parameters.Add("@ResponseDate", ResponseDate, DbType.DateTime2, ParameterDirection.Input);
            parameters.Add("@RequestHeaders", RequestHeaders, DbType.String, ParameterDirection.Input);
            parameters.Add("@ResponseHeaders", ResponseHeaders, DbType.String, ParameterDirection.Input);
            parameters.Add("@RequestData", RequestData, DbType.String, ParameterDirection.Input);
            parameters.Add("@ResponseData", ResponseData, DbType.String, ParameterDirection.Input);
            parameters.Add("@ResponseCode", ResponseCode, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ResponseStatusCode", ResponseStatusCode, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ExtraInfo", ExtraInfo, DbType.String, ParameterDirection.Input);
            parameters.Add("@ErrorMessage", ErrorMessage, DbType.String, ParameterDirection.Input);
            parameters.Add("@ServerHostName", ServerHostName, DbType.String, ParameterDirection.Input);
            parameters.Add("@ServerIpAddress", ServerIpAddress, DbType.String, ParameterDirection.Input);
            parameters.Add("@ServerUserName", ServerUserName, DbType.String, ParameterDirection.Input);

            //execute 
            await connection.ExecuteAsync("ServiceLog_Insert", parameters, transaction, commandType: CommandType.StoredProcedure);

            retVal = parameters.Get<long>("@ServiceLogId"); return retVal;
        }

        public async Task<IEnumerable<ServiceLogSearchResult>> Search(IDbTransaction? transaction, long? serviceLogId, int? applicationId, string? traceId, string? component, string? entityId, string? requestUri, string? requestIpAddress, string? requestUserName, string? clientId, string? sessionId, string? companyId, string? profileId, int? userId, DateTime? requestDateFrom, DateTime? requestDateTo, int? responseCode)
        {
            IEnumerable<ServiceLogSearchResult> retVal;

            using DbConnection connection = GetDefaultConnection();
            await connection.OpenAsync();

            // Specify stored procedure parameters
            var parameters = new DynamicParameters();
            parameters.Add("@ServiceLogId", serviceLogId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@ApplicationId", applicationId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@TraceId", traceId, DbType.String, ParameterDirection.Input);
            parameters.Add("@Component", component, DbType.String, ParameterDirection.Input);
            parameters.Add("@EntityId", entityId, DbType.String, ParameterDirection.Input);
            parameters.Add("@RequestUri", requestUri, DbType.String, ParameterDirection.Input);
            parameters.Add("@RequestIpAddress", requestIpAddress, DbType.String, ParameterDirection.Input);
            parameters.Add("@RequestUserName", requestUserName, DbType.String, ParameterDirection.Input);
            parameters.Add("@ClientId", clientId, DbType.String, ParameterDirection.Input);
            parameters.Add("@SessionId", sessionId, DbType.String, ParameterDirection.Input);
            parameters.Add("@CompanyId", companyId, DbType.String, ParameterDirection.Input);
            parameters.Add("@ProfileId", profileId, DbType.String, ParameterDirection.Input);
            parameters.Add("@UserId", userId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@RequestDateFrom", requestDateFrom, DbType.DateTime2, ParameterDirection.Input);
            parameters.Add("@RequestDateTo", requestDateTo, DbType.DateTime2, ParameterDirection.Input);
            parameters.Add("@ResponseCode", responseCode, DbType.Int32, ParameterDirection.Input);

            //execute 
            var query = await connection.QueryAsync<ServiceLogSearchResult>("ServiceLogs_Search", parameters, transaction, commandType: CommandType.StoredProcedure);

            retVal = query;
            return retVal;
        }
    }
}
