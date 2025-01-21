using SV21T1020001.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020001.DataLayers
{
    public interface IUserAccountDAL
    {
        int Add(Register data);

        /// <summary>
        /// Xác thực tài khoản đăng nhập của người dùng
        /// Hàm trả về thông tin tài khoản nếu xác thực thành công
        /// ngược lại trả về hàm null
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserAccount? Authorize(string userName, string password);
        CustomerAccount? AuthorizeCustomer(string userName, string password);
        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ChangePassword(string userName, string password);
        int CheckOldePassword(String username, String password);
    }
}
