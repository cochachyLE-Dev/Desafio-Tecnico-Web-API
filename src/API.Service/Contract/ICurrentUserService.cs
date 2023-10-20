namespace API.Service.Contract
{
    public interface ICurrentUserService
    {
        public string Username { get; }
        public string Email { get; }       
    }
}
