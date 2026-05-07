using Car_Rental_System.Helpers;
using Car_Rental_System.Models;

namespace Car_Rental_System.Services
{
    internal sealed class AuthService
    {
        public bool TrySignIn(string userName, string password, out AppUser? user)
        {
            user = null;

            (string ownerUserName, string ownerPasswordHash) = AppConfiguration.GetOwnerAccount();

            bool isOwnerUserName = string.Equals(
                userName.Trim(),
                ownerUserName,
                StringComparison.OrdinalIgnoreCase);
            bool isOwnerPassword = VerifyPassword(password, ownerPasswordHash);

            if (!isOwnerUserName || !isOwnerPassword)
                return false;

            user = new AppUser
            {
                UserName = ownerUserName,
                Role = AppConstants.OwnerRole
            };

            return true;
        }

        private static bool VerifyPassword(string password, string passwordHash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, passwordHash);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Owner password hash is invalid. Generate a BCrypt hash and store it as OwnerAccount:PasswordHash.",
                    ex);
            }
        }
    }
}
