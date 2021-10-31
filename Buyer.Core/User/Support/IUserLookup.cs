using Context.Models;
using Domain.Entitities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Core.User.Support
{
    public interface IUserLookup
    {
        public Task<IEnumerable<UserModel>> GetAll();
        public Task<BuyerUser> GetByUserName(string userName);
    }
}
