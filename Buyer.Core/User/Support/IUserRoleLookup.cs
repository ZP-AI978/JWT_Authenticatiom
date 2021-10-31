using Context.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Core.User.Support
{
    public interface IUserRoleLookup
    {
        public Task<IEnumerable<BuyerUserRole>> GetUserRoles(int Id);
    }
}
