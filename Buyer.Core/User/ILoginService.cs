using Domain.Entitities.Models;
using Domain.Entitities.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Core.User
{
    public interface ILoginService
    {
        public Task<string> Login(LoginRequest request);
        public Task<IEnumerable<UserModel>> GetAll();
    }
}
