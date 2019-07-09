using StarCraft2League.Constants.Users;
using System.Security.Claims;

namespace StarCraft2League.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetId(this ClaimsPrincipal principal) =>
            int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));

        public static bool IsAdmin(this ClaimsPrincipal principal) =>
            principal.IsInRole(RoleConstants.Admin);

        public static bool IsModerator(this ClaimsPrincipal principal) =>
            principal.IsInRole(RoleConstants.Moderator);

        public static bool IsSimpleUser(this ClaimsPrincipal principal) =>
            principal.IsInRole(RoleConstants.User);

        public static bool HasProfile(this ClaimsPrincipal principal) =>
            bool.Parse(principal.FindFirstValue("HasProfile") ?? "False");
    }
}