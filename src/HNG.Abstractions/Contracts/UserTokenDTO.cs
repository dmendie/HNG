namespace HNG.Abstractions.Contracts
{
    public class UserTokenDTO
    {
        public string AccessToken { get; set; } = null!;
        public UserDTO User { get; set; } = new UserDTO();
    }
}
