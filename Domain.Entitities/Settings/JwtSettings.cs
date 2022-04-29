namespace Domain.Entitities.Settings
{
    public class JwtSettings
    {
        public string PrivateKeyXml { get; set; }
        public string PublicKeyXml { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryMinutes { get; set; } = 30;
    }
}
