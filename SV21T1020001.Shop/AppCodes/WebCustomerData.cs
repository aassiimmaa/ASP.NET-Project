using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace SV21T1020001.Shop
{
    /// <summary>
    /// Những thông tin cần thể hiện trong "danh tính" của người dùng
    /// </summary>
    public class WebCustomerData
    {
        public string UserId { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Province { get; set; } = "";
        public string Role { get; set; } = "Customer";


        /// <summary>
        /// Tạo ra chứng nhận để ghi nhận danh tính người dùng
        /// </summary>
        /// <returns></returns>
        public ClaimsPrincipal CreatePrincipal()
        {
            //Danh sáchs các Claim chứa các thông tin liên quan đến "danh tính" người dùng
            List<Claim> claims = new List<Claim>()
            {
                new Claim(nameof(UserId), UserId),
                new Claim(nameof(DisplayName), DisplayName),
                new Claim(nameof(Email), Email),
                new Claim(nameof(PhoneNumber), PhoneNumber),
                new Claim(nameof(Province), Province)
            };
            
            //Tạo identity dựa trên các thông tin có trong danh sách các Claim
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //Tạo Principal
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
