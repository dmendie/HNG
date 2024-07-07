using HNG.Abstractions.Contracts;
using HNG.Abstractions.Services.Infrastructure;

namespace HNG.Authentication.Mock
{
    public class MockUserAuthenticationService : IAuthenticationService
    {
        string? _sessionId;
        public MockUserAuthenticationService()
        { }

        public UserContextDTO UserContext => Get();

        private UserContextDTO Get()
        {
            return new UserContextDTO
            {
                ClientId = "web",
                UserId = "7acbba30-a989-4aa4-c702-08db3920bd4e",
                UserName = "johndoe",
                OrgId = "89E75A35-A8E0-4B17-B89A-E4E929B0929C",
                SessionId = _sessionId == null ? _sessionId = Guid.NewGuid().ToString() : _sessionId
            };

        }
    }
}
