using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using SV21T1020001.BusinessLayers;
using SV21T1020001.DomainModels;

namespace SV21T1020001.Shop.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            {
                ViewBag.Username = username;
                //Kiểm tra thông tin đầu vào
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("Error", "Nhập tên và mật khẩu");

                    return View();
                }
                var userAccount = UserAccountService.AuthorizeCustomer(username, password);
                if (userAccount == null)
                {
                    ModelState.AddModelError("Error", "Đăng nhập thất bại");
                    return View();
                }

                var userData = new WebCustomerData()
                {
                    UserId = userAccount.UserId,
                    DisplayName = userAccount.DisplayName,
                    Email = userAccount.Email,
                    PhoneNumber = userAccount.PhoneNumber,
                    Province = userAccount.Province,
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userData.CreatePrincipal());
                return RedirectToAction("Index", "Home");
            }
        }
        public async Task<IActionResult> LogoutAsync()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


        public IActionResult ChangePassword()
        {
            var data = new ChangePassword();
            return View(data);
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePassword data)
        {

            if (UserAccountService.checkCustomerPass(User.GetUserData().Email, data.OldPassword) == 0)
                ModelState.AddModelError(nameof(data.OldPassword), "Mật khẩu cũ không đúng");
            if (string.IsNullOrEmpty(data.NewPassword) || data.NewPassword.Length < 6)
                ModelState.AddModelError(nameof(data.NewPassword), "Mật khẩu tối thiểu phải 6 ký tự");
            else if (!data.NewPassword.Equals(data.ConfirmPassword))
                ModelState.AddModelError(nameof(data.ConfirmPassword), "Xác nhận mật khẩu không đúng");
            if (ModelState.IsValid)
            {
                if (UserAccountService.ChangePasswordCustomer(User.GetUserData().Email, data.NewPassword))
                    data.SuccessMessage = "Bạn đã thay đổi mật khẩu thành công!";
            }
            return View(data);
        }

        public IActionResult Profile()
        {
            var userData = User.GetUserData();
            Customer data = CommonDataService.GetCustomer(int.Parse(userData.UserId));
            if (data == null) {
                RedirectToAction("Index", "Home");
            }
            return View(data);
        }

        public IActionResult Update(int id)
        {
            Customer data = CommonDataService.GetCustomer(id);
            return View(data);
        }

        public IActionResult Save(Customer data)
        {
            ////Kiểm tra nếu thấy dữ liệu đầu vào không hợp lệ thì lưu trữ thông báo lỗi vào trong ModelState
            //if (string.IsNullOrWhiteSpace(data.CustomerName))
            //    ModelState.AddModelError("EmptyName", "Tên khách hàng không được rỗng");
            //if (string.IsNullOrWhiteSpace(data.Province))
            //    ModelState.AddModelError("EmptyProvince", "Vui lòng chọn tỉnh/thành");
            //if (string.IsNullOrWhiteSpace(data.Phone))
            //    ModelState.AddModelError("EmptyPhone", "Vui lòng nhập điện thoại");

            ////Dựa vào thuộc tính IsValid của ModelState để biết có thông báo lỗi hay không
            //if (ModelState.IsValid == false)
            //{
            //    return View("Update", data);
            //}

            bool result = CommonDataService.UpdateCustomer(data);
            ViewData["update"] = true;
            return RedirectToAction("Profile");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            var data = new Register();
            return View(data);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(Register data)
        {
            if (string.IsNullOrWhiteSpace(data.CustomerName))
                ModelState.AddModelError(nameof(data.CustomerName), "Tên khách hàng không được rỗng");
            if (string.IsNullOrWhiteSpace(data.ContactName))
                ModelState.AddModelError(nameof(data.ContactName), "Tên giao dịch không được rỗng");
            if (string.IsNullOrEmpty(data.Address))
                ModelState.AddModelError(nameof(data.Address), "Vui lòng nhập địa chỉ");
            if (string.IsNullOrEmpty(data.Province))
                ModelState.AddModelError(nameof(data.Province), "Vui lòng chọn tỉnh/thành");
            if (string.IsNullOrEmpty(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Vui lòng nhập điện thoại");
            if (string.IsNullOrEmpty(data.Email))
                ModelState.AddModelError(nameof(data.Email), "Vui lòng nhập email");
            if (string.IsNullOrEmpty(data.Password))
                ModelState.AddModelError(nameof(data.Password), "Vui lòng nhập mật khẩu");
            if (string.IsNullOrEmpty(data.PasswordConfirm))
                ModelState.AddModelError(nameof(data.PasswordConfirm), "Vui lòng nhập mật khẩu");
            if (data.Password != data.PasswordConfirm)
            {
                ModelState.AddModelError(nameof(data.Password), "Mật khẩu không trung khớp");
                ModelState.AddModelError(nameof(data.PasswordConfirm), "Mật khẩu không trung khớp");
            }

            if (ModelState.IsValid == false)
            {
                return View("Register", data);
            }

            int id = UserAccountService.Register(data);
            if (id <= 0)
            {
                ModelState.AddModelError("IsvalidEmail", "Email bị trùng");
                return View("Register", data);
            }
            return RedirectToAction("Login", "Account");
        }
    }
}
