using System.Security.Claims;

namespace TS.ServiceLogic.Common
{
    public static class Utility
    {
        public static int ValidateAdminAndGetId(ClaimsPrincipal? user)
        {
            int userId = ValidateUserAndGetId(user);

            var userRoleClaim = user?.FindFirst(ClaimTypes.Role)?.Value;
            if (userRoleClaim != "Admin")
                throw new UnauthorizedAccessException("Only admins can perform this action.");

            return userId;
        }

        public static int ValidateUserAndGetId(ClaimsPrincipal? user)
        {
            if (user == null || user.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException("User not authenticated.");

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid user identity.");
            }

            return userId;
        }
    }
}