namespace InstantaneousGram_Authentication.Contracts
{
    public interface IAuthService
    {
        Task<string> Authenticate(string email, string password);
    }
}
