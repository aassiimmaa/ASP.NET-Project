using Dapper;
using SV21T1020001.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020001.DataLayers.SQLServer
{
    public class RegisterDAL : BaseDAL, ICommonDAL<Register>
    {

        public RegisterDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Register data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Customers where Email = @Email)
                                select -1
                            else
                                begin
                                    insert into Customers(CustomerName, ContactName, Province, Address, Phone, Email,Password,IsLocked)
                                    values(@CustomerName, @ContactName, @Province, @Address, @Phone, @Email,@Password,0);
                                    select SCOPE_IDENTITY();
                                end";
                var parameters = new
                {
                    CustomerName = data.CustomerName ?? "",
                    ContactName = data.ContactName ?? "",
                    Province = data.Province ?? "",
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    Password = data.Password ?? ""
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "")
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Register? Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool InUsed(int id)
        {
            throw new NotImplementedException();
        }

        public List<Register> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            throw new NotImplementedException();
        }

        public bool Update(Register data)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCustomer(Customer data)
        {
            throw new NotImplementedException();
        }
    }
}
