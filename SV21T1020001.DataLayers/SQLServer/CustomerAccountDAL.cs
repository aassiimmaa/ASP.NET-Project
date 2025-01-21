using Dapper;
using SV21T1020001.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020001.DataLayers.SQLServer
{
    public class CustomerAccountDAL : BaseDAL, IUserAccountDAL
    {
        public CustomerAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Register data)
        {
            throw new NotImplementedException();
        }

        public CustomerAccount? AuthorizeCustomer(string userName, string password)
        {
            CustomerAccount? data = null;
            using (var connection = OpenConnection())
            {
                var sql = "select CustomerID as UserId, Email ,CustomerName as DisplayName, Phone as PhoneNumber, Province from Customers where Email = @Email and Password= @Password";
                var parameters = new
                {
                    Email = userName,
                    Password = password
                };
                data = connection.QueryFirstOrDefault<CustomerAccount>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool ChangePassword(string userName, string password)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"update Customers
                    set Password = @password
                    where Email = @username";
                var parameters = new
                {
                    username = userName,
                    password = password
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;

            }
            return result;
        }

        public int CheckOldePassword(string username, string password)
        {
            using (var connection = OpenConnection())
            {
                var sql = @"IF EXISTS(SELECT * FROM Customers 
                           WHERE Email = @username
                           AND Password = @password)
                             SELECT 1
                           ELSE
                             SELECT 0";
                var parameters = new
                {
                    username = username,
                    password = password
                };
                return connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
            }
        }

        UserAccount? IUserAccountDAL.Authorize(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
