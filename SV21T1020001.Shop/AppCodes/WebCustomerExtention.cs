using System.Security.Claims;

namespace SV21T1020001.Shop
{
    public static class WebCustomerExtention
    {
        /// <summary>
        /// Đọc thông tin người dùng từ principal
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static WebCustomerData? GetUserData(this ClaimsPrincipal principal)
        {
            try
            {
                if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
                    return null;

                var userData = new WebCustomerData();
                userData.UserId = principal.FindFirstValue(nameof(userData.UserId)) ?? "";
                userData.DisplayName = principal.FindFirstValue(nameof(userData.DisplayName)) ?? "";
                userData.Email = principal.FindFirstValue(nameof(userData.Email)) ?? "";
                userData.PhoneNumber = principal.FindFirstValue(nameof(userData.PhoneNumber)) ?? "";
                userData.Province = principal.FindFirstValue(nameof(userData.Province)) ?? "";
                return userData;

            }
            catch
            {
                return null;
            }
        }
    }
}
