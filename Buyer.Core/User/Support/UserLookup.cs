using Buyer.Data.Context;
using Buyer.Data.Models;
using Domain.Entitities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Core.User.Support
{
    public class UserLookup : IUserLookup
    {
        private readonly BuyertContext context;
        private readonly ILogger<UserLookup> logger;

        public UserLookup(BuyertContext context, ILogger<UserLookup> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<IEnumerable<UserModel>> GetAll()
        {
            logger.LogInformation("Starting UserLookup GetAll");

            IEnumerable<UserModel> users = Enumerable.Empty<UserModel>();

            try
            {
                users = await context.BuyerUsers.Select(p => 
                    new UserModel 
                    { 
                        Id = p.Id,
                        Username = p.UserName
                    }).ToListAsync();
            }
            catch(Exception ex) 
            {
                logger.LogError(ex,"Error Getting Users");
            }

            logger.LogInformation("Finished UserLookup GetAll");

            return users;
        }

        public async Task<BuyerUser> GetByUserName(string userName)
        {
            try
            {
                var user = await context.BuyerUsers.FirstOrDefaultAsync(y => y.UserName.Equals(userName));
                return user;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Getting UserName: {userName}");
            }

            return null;
        }
    }
}
