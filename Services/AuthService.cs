using Car_Rental_System.Helpers;
using Car_Rental_System.Models;

namespace Car_Rental_System.Services
{
    internal sealed class AuthService
    {
        public bool TrySignIn(string userName, string password, out AppUser? user)
        {
            user = null;

            (string ownerUserName, string ownerPassword) = AppConfiguration.GetOwnerAccount();

            bool isOwnerUserName = string.Equals(
                userName.Trim(),
                ownerUserName,
                StringComparison.OrdinalIgnoreCase);
            bool isOwnerPassword = string.Equals(password, ownerPassword, StringComparison.Ordinal);

            if (!isOwnerUserName || !isOwnerPassword)
                return false;

            user = new AppUser
            {
                UserName = ownerUserName,
                Role = AppConstants.OwnerRole
            };

            return true;
        }
    }
}
