using HNG.Abstractions.Contracts;
using HNG.Abstractions.Services.Business;
using SimpleValidator;

namespace HNG.Business
{
    public class HelloService : BaseBusinessLayerService, IHelloService
    {
        public HelloService()
        { }

        public Task<VisitorIPAddressDTO> GreetUser(string? Ip, string? City, string? Temperature, string? VisitorName)
        {
            var isDigit = long.TryParse(VisitorName, out var digitName);

            var validator = new Validator();
            validator
            .IsNotNullOrEmpty(nameof(Ip), Ip ?? string.Empty, "Your IP information could not found*")
            .IsNotNullOrEmpty(nameof(VisitorName), VisitorName ?? string.Empty, "Visitor name is required*")
            .IsNot(nameof(isDigit), () => isDigit == true, "A valid visitor name should start with an alphabet")
            .EnsureNoHtml(nameof(Ip), Ip ?? string.Empty, "IP must not contain html content*")
            .EnsureNoHtml(nameof(VisitorName), VisitorName ?? string.Empty, "Visitor name must not contain html content*");
            validator.ThrowValidationExceptionIfInvalid();

            var randomTemp = Random.Shared.Next(-20, 55);

            var ipdetail = new VisitorIPAddressDTO
            {
                Client_Ip = Ip!,
                Location = City!,
                Greeting = $"Hello, {VisitorName}!, the temperature is {Temperature} degrees celcius in {City}"
            };

            return Task.FromResult(ipdetail);
        }
    }
}
