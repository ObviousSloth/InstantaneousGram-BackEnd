namespace InstantaneousGram_Authentication.Contracts
{
    public interface ITokenService
    {
        string GenerateToken(string username);
    }
}
