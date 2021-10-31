namespace Domain.Entitities.Settings
{
    public class JwtSettings
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryMinutes { get; set; } = 30;
    }
}
