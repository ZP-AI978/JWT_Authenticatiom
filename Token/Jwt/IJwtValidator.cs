namespace Token.Jwt
{
    public interface IJwtValidator
    {
        public bool IsTokenValid(string token);
    }
}
