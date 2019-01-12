namespace RsaSecureToken.Interface
{
    public interface IRsaToken
    {
        string GetRandom(string account);
    }
}