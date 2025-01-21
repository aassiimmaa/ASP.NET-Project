using Microsoft.Data.SqlClient;
namespace SV21T1020001.DataLayers.SQLServer
{
    public abstract class BaseDAL
    {
        protected string _connectionString = "";
        /// <summary>
        /// Constructor/Ctor/Hàm tạo/Hàm dựng
        /// </summary>
        /// <param name="connectionString"></param>
        public BaseDAL(string connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// Tạo và mở kết nối đến CSDL (SQL Server)
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();
            return connection;
        }
    }
}
