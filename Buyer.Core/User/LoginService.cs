using Buyer.Core.User.Support;
using Buyer.Data.Extensions;
using Buyer.Data.Models;
using Domain.Entitities.Models;
using Domain.Entitities.Requests;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Token.Jwt;

namespace Buyer.Core.User
{
    public class LoginService : ILoginService
    {
        private readonly IJwtGenerator jwtGenerator;
        private readonly IUserLookup userLookup;
        private readonly IUserRoleLookup userRoleLookup;
        private readonly ILogger<LoginService> logger;

        public LoginService(IJwtGenerator jwtGenerator, 
            IUserLookup userLookup,
            IUserRoleLookup userRoleLookup,
            ILogger<LoginService> logger)
        {
            this.jwtGenerator = jwtGenerator;
            this.userLookup = userLookup;
            this.userRoleLookup = userRoleLookup;
            this.logger = logger;
        }

        public async Task<IEnumerable<UserModel>> GetAll()
        {
            return await userLookup.GetAll();
        }

        public async Task<string> Login(LoginRequest request)
        {
            logger.LogInformation($"Login Intitaed for Username{request.Username}");

            var user = await userLookup.GetByUserName(request.Username);

            if (user == null || !user.IsValidPassword(request.UserPassword))
                return string.Empty;

            var userRoles = await userRoleLookup.GetUserRoles(user.Id);

            if (!userRoles.Any())
                return string.Empty;

            var userRoleNames = userRoles.Select(y => y.Role.RoleName).ToList();

            var token = jwtGenerator.Generate(user.UserName, userRoleNames);
            return token;
        }

       
    }
}
