using SV21T1020001.DataLayers;
using SV21T1020001.DataLayers.SQLServer;
using SV21T1020001.DomainModels;
using SV21T1020001.BusinessLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020001.BusinessLayers
{
    public class UserAccountService
    {
        private static readonly IUserAccountDAL employeeAcountDB;
        private static readonly IUserAccountDAL customerAccountDB;
        private static readonly ICommonDAL<Register> registerAccountDB;


        static UserAccountService()
        {
            //string connectionString = "server=.;user id=sa; password=123;database=LiteCommerceDB;TrustServerCertificate=true";
            string connectionString = Configuration.ConnectionString;
            employeeAcountDB = new DataLayers.SQLServer.EmployeeAccountDAL(connectionString);
            customerAccountDB = new DataLayers.SQLServer.CustomerAccountDAL(connectionString);
            registerAccountDB = new DataLayers.SQLServer.RegisterDAL(connectionString);
        }

        public static int CheckOldPassword(string username, string password)
        {
            return employeeAcountDB.CheckOldePassword(username, password);
        }

        public static int checkCustomerPass(string username, string password)
        {
            return customerAccountDB.CheckOldePassword(username, password);
        }

        public static bool ChangePassword(string username, string password)
        {
            return employeeAcountDB.ChangePassword(username, password);
        }

        public static bool ChangePasswordCustomer(string username, string password)
        {
            return customerAccountDB.ChangePassword(username, password);
        }

        public static UserAccount? Authorize(UserTypes userTypes, string username, string password)
        {
            if (userTypes == UserTypes.Employee)
            {
                return employeeAcountDB.Authorize(username, password);
            }
            else
            {
                return null;
            }
        }

        public static CustomerAccount? AuthorizeCustomer(string username, string password)
        {
            return customerAccountDB.AuthorizeCustomer(username, password);
        }

        public static int Register(Register data)
        {
            return registerAccountDB.Add(data);
        }

        public enum UserTypes
        {
            Employee,
            Customer,
        }

    }
}
