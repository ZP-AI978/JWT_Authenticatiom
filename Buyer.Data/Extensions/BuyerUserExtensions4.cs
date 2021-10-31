using Buyer.Data.Models;

namespace Buyer.Data.Extensions
{
    public static class BuyerUserExtensions4
    {
        public static bool IsValidPassword(this BuyerUser buyerUser, string password)
        {
            return buyerUser.UserPassword.Equals(password);
        }
    }
}
