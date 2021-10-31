using Buyer.Data.Models;
using Domain.Entitities.Models;

namespace Buyer.Core.User.Support
{
    public static class UserMapper
    {
        public static UserModel MapToModel(BuyerUser entity)
        {
            var userModel = new UserModel
            {
                Id = entity.Id,
                Username = entity.UserName,

            };

            return userModel;
        }
    }
}
