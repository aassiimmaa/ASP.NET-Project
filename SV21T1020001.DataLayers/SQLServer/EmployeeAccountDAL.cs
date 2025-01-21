using Dapper;
using SV21T1020001.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020001.DataLayers.SQLServer
{
    public class EmployeeAccountDAL : BaseDAL, IUserAccountDAL
    {
        public EmployeeAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Register data)
        {
            throw new NotImplementedException();
        }

        public UserAccount? Authorize(string userName, string password)
        {
            UserAccount? data = null;
            using (var connection = OpenConnection())
            {
                var sql = "select EmployeeID as UserId, Email as UserName,FullName as DisplayName,Photo,RoleNames from Employees where Email = @Email and Password = @Password";
                var parameters = new
                {
                    Email = userName,
                    Password = password
                };
                data = connection.QueryFirstOrDefault<UserAccount>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public CustomerAccount? AuthorizeCustomer(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(string username, string password)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"update Employees
                            set Password = @password
                            where Email = @username";
                var parameters = new
                {
                    username = username,
                    Password = password
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;

            }
            return result;
        }

        public int CheckOldePassword(string username, string password)
        {
            int result;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Employees 
		                              where Email = @username
		                              and Password = @password)
                               select 1
                            else select 0 ";
                var parameters = new
                {
                    username = username,
                    password = password
                };
                result = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);

            }
            return result;
        }
    }
}
