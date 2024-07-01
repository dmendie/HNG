using HNG.Abstractions.Contracts;
using HNG.Abstractions.Services.Business;
using IPinfo.Models;
using Microsoft.AspNetCore.Mvc;
using OpenWeatherMap.Models;

namespace HNG.Api.Client.Controllers
{
    /// <summary>
    /// Hello Controller - greet user sending request to this endpoint
    /// </summary>
    [Tags("Stage 1 Task")]
    [Route("api/hello")]
    public class HelloController : BaseApiController
    {
        readonly IHelloService HelloService;
        /// <summary>
        /// HelloController constructor
        /// </summary>
        public HelloController(IHelloService helloService)
        {
            HelloService = helloService;
        }

        /// <summary>
        /// Greet user - returns a greeting with user's location and ip address
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<VisitorIPAddressDTO>> GreetUser([FromQuery] HelloUserParamDTO UserParam)
        {
            IPResponse? Ip = null;
            WeatherInfo? Weather = null;
            if (HttpContext.Items.TryGetValue("IpInfo", out var ipInfo)) { Ip = ipInfo as IPResponse; }
            if (HttpContext.Items.TryGetValue("WeatherInfo", out var weatherInfo)) { Weather = weatherInfo as WeatherInfo; }
            var data = await HelloService.GreetUser(Ip?.IP, Ip?.City, Weather?.Main?.Temperature.DegreesCelsius.ToString(), UserParam?.visitor_name);
            return Ok(data);
        }
    }
}
