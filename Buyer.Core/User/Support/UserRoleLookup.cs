using Context.Context;
using Context.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Core.User.Support
{
    public class UserRoleLookup : IUserRoleLookup
    {
        private readonly BuyertContext context;
        private readonly ILogger<UserLookup> logger;

        public UserRoleLookup(BuyertContext context, ILogger<UserLookup> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<IEnumerable<BuyerUserRole>> GetUserRoles(int id)
        {
            IEnumerable<BuyerUserRole> userRoles = Enumerable.Empty<BuyerUserRole>();

            try
            {
                userRoles = await context.BuyerUserRoles.Where(y => y.Id == id).Include(i => i.Role).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Getting UserRoles for UserId:{id}");
            }

            return userRoles;
        }
    }
}
