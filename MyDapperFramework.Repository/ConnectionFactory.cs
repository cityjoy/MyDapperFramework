using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDapperFramework.Repository
{
    public class ConnectionFactory
    {
        private static readonly string mainConnString = ConfigurationManager.AppSettings["MainDbConnectionString"];
        private static readonly string AdminConnString = ConfigurationManager.AppSettings["AuthorityDbConnectionString"];
        private static readonly string logConnString = ConfigurationManager.AppSettings["LogDbConnectionString"];
        /// <summary>
        /// 创建生涯主站数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection CreateMainSiteConnection()
        {
            IDbConnection conn = new SqlConnection(mainConnString + "Connect Timeout =18000;");
            conn.Open();
            return conn;
        }

        /// <summary>
        /// 创建生涯主站日志数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection CreateLogConnection()
        {
            IDbConnection conn = new SqlConnection(logConnString);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// 创建生涯管理后台数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection CreateAdminConnection()
        {
            IDbConnection conn = new SqlConnection(AdminConnString);
            conn.Open();
            return conn;
        }
    }
}
