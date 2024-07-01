using HNG.Abstractions.Models;
using HNG.Abstractions.Services.Infrastructure;

namespace HNG.Web.Common.Extensions.ServiceLogs
{
    /// <summary>
    /// Middleware responsible for logging Admin API requests to the database
    /// </summary>
    public class ClientServiceLogAttribute : BaseServiceLogAttribute
    {
        readonly IServiceLogService Logger;

        /// <summary>
        /// AdminServiceLogAttribute - constructor
        /// </summary>
        public ClientServiceLogAttribute(AppSettings appSettings, IServiceLogService logger) : base(appSettings, logger)
        {
            Logger = logger;
        }

        protected override void Enhance()
        {
            Logger.Log.ClientId = "web";
            Logger.Log.RequestUserName = "TestUser";
            Logger.Log.SessionId = null;
            Logger.Log.UserId = null;
            Logger.Log.CompanyId = null;
            Logger.Log.ProfileId = null;
        }
    }
}
