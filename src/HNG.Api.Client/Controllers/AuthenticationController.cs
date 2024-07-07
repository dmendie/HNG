using HNG.Abstractions.Contracts;
using HNG.Abstractions.Services.Business;
using HNG.Api.Client.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HNG.Api.Client.Controllers
{
    /// <summary>
    /// Organisation Controller
    /// </summary>
    [Tags("Stage 2 Task")]
    [Route("auth")]
    [AllowAnonymous]
    public class AuthenticationController : BaseApiController
    {
        readonly IUserService UserService;

        /// <summary>
        /// constructor
        /// </summary>
        public AuthenticationController(IUserService userService)
        {
            UserService = userService;
        }

        /// <summary>
        /// Registers user - registers a users and creates a default organisation 
        /// </summary>
        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserCreationDTO userInfo)
        {
            var (status, response) = await UserService.Register<ResponseDataDTO>(userInfo);
            if (status == 200)
            {
                response!.Message = "Registration successful";
                return Ok(response);
            }

            return StatusCode(400, new { Status = "Bad request", Message = "Registration unsuccessful", StatusCode = 400 });
        }

        /// <summary>
        /// Login user - logs in a user using valid creds
        /// </summary>
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDTO credentials)
        {
            var (status, response) = await UserService.LoginUser<ResponseDataDTO>(credentials);
            if (status == 200)
            {
                response!.Message = "Login successful";
                return Ok(response);
            }

            return StatusCode(401, new { Status = "Bad request", Message = "Authentication failed", StatusCode = 401 });
        }
    }
}
