using MyDapperFramework.Repository.Dapper;
using MyDapperFramework.Repository.Interface;
using MyDapperFramework.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MyDapperFramework.Repository
{
    /// <summary>
    /// 数据上下文工厂
    /// 通过工厂模式创建
    /// </summary>
    public class ContextFactory
    {

        //禁止实例化 
        static ContextFactory()
        {

        }

        /// <summary>
        /// 跨库 跨表 事物提交
        /// </summary>
        /// <param name="operate"></param>
        /// <returns></returns>
        public static bool Submit(Action operate)
        {
            using (var tranScope = new TransactionScope())
            {
                try
                {
                    operate();
                    tranScope.Complete();
                    return true;
                }
                catch(Exception ex)
                {
                    //任务出差 就不会提交 自动回滚
                    return false;
                }          
            }  
        }

        public static IDataAccess<TEntity> CreateDbSet<TEntity>() where TEntity : class
        {
            return new BaseRepository<TEntity>("LocalDb");
        }
    }
}
