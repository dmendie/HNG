using HNG.Abstractions.Contracts;
using HNG.Abstractions.Services.Business;
using Microsoft.AspNetCore.Mvc;

namespace HNG.Api.Client.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
    [Tags("Stage 2 Task")]
    [Route("api/users/{id}")]
    public class UserController : BaseSecuredApiController
    {
        readonly IUserService UserService;

        /// <summary>
        /// constructor
        /// </summary>
        public UserController(IUserService userService)
        {
            UserService = userService;
        }

        /// <summary>
        /// Search user - gets a user by their own record or user record in organisations they belong to or created
        /// </summary>
        [HttpGet]
        public async Task<ResponseDataDTO> SearchUser(string id)
        {
            var response = await UserService.Search<ResponseDataDTO>(id, UserContext);
            return response;
        }

    }
}
