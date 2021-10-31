using Domain.Entitities.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Token.Jwt
{
    public class JwtGenerator : IJwtGenerator
	{
		private JwtSettings jwtSettings;
        private readonly ILogger<JwtGenerator> logger;

        public JwtGenerator(JwtSettings jwtSettings, ILogger<JwtGenerator> logger)
		{
			this.jwtSettings = jwtSettings;
            this.logger = logger;
        }

		public string Generate(string userId, IEnumerable<string> validRoles)
		{
			var token = string.Empty;

			try
            {

				var dateTimeNow = DateTime.Now;
				var expiresAt = dateTimeNow.AddMinutes(jwtSettings.ExpiryMinutes);
				var myIssuer = jwtSettings.Issuer;
				var myAudience = jwtSettings.Audience;

				using (RSA privateRsa = RSA.Create())
				{
					string strPrivateKey = jwtSettings.PrivateKey;
					privateRsa.FromXmlString(strPrivateKey);

					var privateKey = new RsaSecurityKey(privateRsa)
					{
						CryptoProviderFactory = new CryptoProviderFactory()
						{
							CacheSignatureProviders = false
						}
					};

					SigningCredentials signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);


					var claims = new List<Claim>();

					claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));

					foreach (string role in validRoles)
					{
						claims.Add(new Claim(ClaimTypes.Role, role));
					}

					var jwt = new JwtSecurityToken(
						signingCredentials: signingCredentials,
						claims: claims,
						notBefore: dateTimeNow,
						expires: expiresAt,
						audience: myAudience,
						issuer: myIssuer
						);

					token = new JwtSecurityTokenHandler().WriteToken(jwt);
				}

			}
            catch(Exception ex)
            {
				logger.LogError(ex, $"Failed to create token for UserId:{userId}");
            }
			return token;
		}
	}
}

