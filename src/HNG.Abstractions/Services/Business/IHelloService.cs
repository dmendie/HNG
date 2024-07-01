using HNG.Abstractions.Contracts;

namespace HNG.Abstractions.Services.Business
{
    public interface IHelloService : IBusinessService
    {
        Task<VisitorIPAddressDTO> GreetUser(string? Ip, string? City, string? Temperature, string? VisitorName);
    }
}
