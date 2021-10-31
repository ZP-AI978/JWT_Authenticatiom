using Domain.Entitities.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Token.Jwt
{
    public class JwtValidator : IJwtValidator
    {
        private JwtSettings jwtSettings;
        private readonly ILogger<JwtValidator> logger;

        public JwtValidator(JwtSettings jwtSettings, ILogger<JwtValidator> logger)
        {
            this.jwtSettings = jwtSettings;
            this.logger = logger;
        }

        public bool IsTokenValid(string token)
        {

            string strPublicKey = jwtSettings.PublicKey;

            using (RSA publicRsa = RSA.Create())
            {
                publicRsa.FromXmlString(strPublicKey);

                var publicKey = new RsaSecurityKey(publicRsa);
                RsaSecurityKey signingKey = new RsaSecurityKey(publicRsa);

                var myIssuer = jwtSettings.Issuer;
                var myAudience = jwtSettings.Audience;
                var tokenHandler = new JwtSecurityTokenHandler();

                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = myIssuer,
                        ValidAudience = myAudience,
                        IssuerSigningKey = signingKey
                    }, out SecurityToken validatedToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Failed to Validate Token token: {token}");
                    return false;
                }
            }
            return true;
        }
    }
}
