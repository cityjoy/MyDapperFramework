using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using System.Reflection;
using System.Text.RegularExpressions;
using Dapper;
using System.Configuration;
using MyDapperFramework.Repository.Interface;
using MyDapperFramework.Repository.Dapper;
using MyDapperFramework.Repository.Mappings;
using MyDapperFramework.Repository.Entity;
using DapperExtensions.Sql;
namespace MyDapperFramework.Repository.Repository
{
    /// <summary>
    /// 数据访问仓储基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class BaseRepository<TEntity> : IDataAccess<TEntity>, IDisposable where TEntity : class
    {
        

        #region 事务操作
        /// <summary>
        /// 事务操作
        /// </summary>
        /// <param name="trans">要进行事务操作的SQL语句和参数列表</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public Tuple<bool, string> ExecuteTransaction(List<Tuple<string, object>> trans, int? commandTimeout = null)
        {
            if (!trans.Any()) return new Tuple<bool, string>(false, "要进行事务操作的SQL语句不能为空！");
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var tran in trans)
                        {
                            conn.Execute(tran.Item1, tran.Item2, transaction, commandTimeout);
                        }
                        transaction.Commit();
                        return new Tuple<bool, string>(true, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();//回滚
                        return new Tuple<bool, string>(false, ex.ToString());
                    }

                }
            }
        }
        #endregion

        #region 数据操作

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity Insert(TEntity entity)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                conn.Insert<TEntity>(entity);
                return entity;
            }

        }

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool Insert(IEnumerable<TEntity> entitys)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                IDbTransaction tran = conn.BeginTransaction();
                try
                {
                    conn.Insert(entitys, transaction: tran);
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();//事物回滚
                    tran.Dispose();
                }
                return false;

            }
        }
        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool InsertNotTran(IEnumerable<TEntity> entitys)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                try
                {
                    conn.Insert(entitys);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(TEntity entity)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                bool result = conn.Update<TEntity>(entity);
                return result;
            }
        }

        /// <summary>
        /// 更新实体指定字段数据
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool Update(object parameters, Expression<Func<TEntity, bool>> expression)
        {
            bool result = false;
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                var predicate = DapperLinqBuilder<TEntity>.FromExpression(expression);
                result = conn.Update<TEntity>(parameters, predicate);
            }
            return result;
        }


        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool Update(IEnumerable<TEntity> entitys)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {

                IDbTransaction tran = conn.BeginTransaction();
                try
                {
                    foreach (var item in entitys)
                    {
                        conn.Update<TEntity>(item, transaction: tran);
                    }
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();//事物回滚
                    tran.Dispose();
                }
                return false;

            }
        }

        /// <summary>
        /// 根据sql语句更新符合条件的数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public bool Update(string sql, object parm)
        {

            bool result = false;
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                result = conn.Execute(sql, parm) > 0;
            }
            return result;
        }


        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(object key)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                TEntity item = conn.Get<TEntity>(key);
                return conn.Delete(item);

            }
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool Delete(IEnumerable<object> keys)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                IDbTransaction tran = conn.BeginTransaction();
                try
                {
                    var tblName = typeof(TEntity).Name;
                    keys = keys.Select(k => string.Format("'{0}'", k));
                    var sql = string.Format("Delete From {0} where Id in ({1})", tblName, string.Join(",", keys));
                    conn.Execute(sql, transaction: tran);
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();//事物回滚
                    tran.Dispose();
                }
                return false;

            }
        }

        /// <summary>
        /// 根据sql语句删除符合条件的数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public bool Delete(string sql, object parm)
        {

            bool result = false;
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                result = conn.Execute(sql, parm) > 0;
            }
            return result;
        }

        /// <summary>
        /// 删除符合条件的数据
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool Delete(Expression<Func<TEntity, bool>> expression)
        {
            bool result = false;
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                var predicate = DapperLinqBuilder<TEntity>.FromExpression(expression);
                result = conn.Delete<TEntity>(predicate);
            }
            return result;
        }


        #endregion

        #region 数据查询
        /// <summary>
        /// 根据Id获取实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetById(object id)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                var item = conn.Get<TEntity>(id);
                return item;
            }

        }

        /// <summary>
        /// 获取数据表总项数
        /// </summary>
        /// <param name="expression">linq表达式 谓词</param>
        /// <returns></returns>
        public long GetCount(Expression<Func<TEntity, bool>> expression = null)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                var predicate = DapperLinqBuilder<TEntity>.FromExpression(expression);
                return conn.Count<TEntity>(predicate);
            }
        }

        /// <summary>
        /// 获取结果集第一条数据
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="sortList"></param>
        /// <returns></returns>
        public TEntity GetFist(Expression<Func<TEntity, bool>> expression = null, object sortList = null)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                var predicate = DapperLinqBuilder<TEntity>.FromExpression(expression);
                IList<ISort> sort = null;
                if (sortList != null)
                {
                    sort = SortConvert(sortList);//转换排序接口
                }
                var data = conn.GetList<TEntity>(predicate, sort);
                return data.FirstOrDefault();
            }
        }

        /// <summary>
        /// 查看指定的数据是否存在
        /// </summary>
        /// <param name="expression">linq表达式 谓词</param>
        /// <returns></returns>
        public bool Exists(Expression<Func<TEntity, bool>> expression)
        {
            var ct = this.GetCount(expression);
            return ct > 0;
        }

        /// <summary>
        /// 根据条件获取表数据
        /// </summary>
        /// <param name="expression">linq表达式</param>
        /// <returns></returns>
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> expression, object sortList = null)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                IList<ISort> sort = null;
                if (sortList != null)
                {
                    sort = SortConvert(sortList);//转换排序接口
                }
                if (expression == null)
                {
                    //允许脏读
                    return conn.GetList<TEntity>(null, sort, transaction: conn.BeginTransaction(IsolationLevel.ReadUncommitted)).ToList();//如果条件为Null 就查询所有数据
                }
                else
                {
                    var predicate = DapperLinqBuilder<TEntity>.FromExpression(expression);
                    return conn.GetList<TEntity>(predicate, sort, transaction: conn.BeginTransaction(IsolationLevel.ReadUncommitted)).ToList();
                }
            }
        }
        /// <summary>
        /// 根据条件获取表数据
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <returns></returns>
        public List<TEntity> GetList(IPredicateGroup predicate, object sortList = null)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                IList<ISort> sort = null;
                if (sortList != null)
                {
                    sort = SortConvert(sortList);//转换排序接口
                }
                return conn.GetList<TEntity>(predicate, sort, transaction: conn.BeginTransaction(IsolationLevel.ReadUncommitted)).ToList();
            }
        }

        /// <summary>
        /// 根据sql语句获取数据列表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<TEntity> GetList(string sql, object parm)
        {
            List<TEntity> result = null;
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                result = conn.Query<TEntity>(sql, parm).ToList();
                return result;
            }
        }


        /// <summary>
        /// 数据表 分页
        /// </summary>
        /// <param name="pager">页数信息</param>
        /// <param name="expression">条件 linq表达式 谓词</param>
        /// <param name="sortList">排序字段</param>
        /// <returns></returns>
        public List<TEntity> GetPageData(XPager pager, Expression<Func<TEntity, bool>> expression = null, object sortList = null)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                int commandTimeout = 1800;
                IPredicateGroup predicate = DapperLinqBuilder<TEntity>.FromExpression(expression); //转换Linq表达式
                IList<ISort> sort = null;
                if (sortList != null)
                {
                    sort = SortConvert(sortList);//转换排序接口
                }
                var entities = conn.GetPage<TEntity>(predicate, sort, pager.PageIndex, pager.PageSize, transaction).ToList();
                pager.RecordCount = conn.Count<TEntity>(predicate, transaction, commandTimeout);
                return entities;
            }
        }


        /// <summary>
        /// 数据表 分页
        /// </summary>
        /// <param name="pager">页数信息</param>
        /// <param name="fields">要查询是字段对象</param>
        /// <param name="expression">条件 linq表达式 谓词</param>
        /// <param name="sortList">排序字段</param>
        /// <returns></returns>
        public List<TEntity> GetPageData(XPager pager, string[] fields, Expression<Func<TEntity, bool>> expression = null, object sortList = null)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                int commandTimeout = 1800;
                IPredicateGroup predicate = DapperLinqBuilder<TEntity>.FromExpression(expression); //转换Linq表达式
                IList<ISort> sort = null;
                if (sortList != null)
                {
                    sort = SortConvert(sortList);//转换排序接口
                }
                var entities = conn.GetPage<TEntity>(fields, predicate, sort, pager.PageIndex, pager.PageSize, transaction).ToList();
                pager.RecordCount = conn.Count<TEntity>(predicate, transaction, commandTimeout);
                return entities;
            }
        }

        /// <summary>
        /// 数据表 分页
        /// </summary>
        /// <param name="pager">页数信息</param>
        /// <param name="predicate">条件</param>
        /// <param name="sortList">排序字段</param>
        /// <returns></returns>
        public List<TEntity> GetPageData(XPager pager, IPredicateGroup predicate = null, object sortList = null)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                int commandTimeout = 1800;
                IList<ISort> sort = null;
                if (sortList != null)
                {
                    sort = SortConvert(sortList);//转换排序接口
                }
                var entities = conn.GetPage<TEntity>(predicate, sort, pager.PageIndex, pager.PageSize, transaction).ToList();
                pager.RecordCount = conn.Count<TEntity>(predicate, transaction, commandTimeout);
                return entities;
            }
        }


        /// <summary>
        /// 数据表 分页
        /// </summary>
        /// <param name="pageNum">指定页数 索引从0开始</param>
        /// <param name="pageSize">指定每页多少项</param>
        ///<param name="outTotal">输出当前表的总项数</param>
        /// <param name="expression">条件 linq表达式 谓词</param>
        /// <param name="sortList">排序字段</param>
        /// <returns></returns>
        public List<TEntity> GetPageData(int pageNum, int pageSize, out long outTotal,
            Expression<Func<TEntity, bool>> expression = null, object sortList = null)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                int commandTimeout = 1800;
                IPredicateGroup predicate = DapperLinqBuilder<TEntity>.FromExpression(expression); //转换Linq表达式
                IList<ISort> sort = SortConvert(sortList);//转换排序接口
                var entities = conn.GetPage<TEntity>(predicate, sort, pageNum, pageSize, transaction: conn.BeginTransaction(IsolationLevel.ReadUncommitted)).ToList();
                outTotal = conn.Count<TEntity>(predicate, transaction, commandTimeout);
                return entities;
            }

        }
        /// <summary>
        /// 获取id范围内的列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fields">字段串（如:"Id,Name,Age"）</param>
        /// <param name="ids">id列表</param>
        /// <returns></returns>
        public List<TEntity> GetListInIds(string tableName, string fields, int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return new List<TEntity>();
            }
            List<TEntity> list = GetList(string.Format("SELECT * FROM [{0}]  WHERE [{1}] IN @ids", tableName, fields), new { ids = ids });
            return list;
        }


        #endregion

        #region 辅助方法
        /// <summary>
        /// 转换成Dapper排序方式
        /// </summary>
        /// <param name="sortList"></param>
        /// <returns></returns>
        private static IList<ISort> SortConvert(object sortList)
        {
            IList<ISort> sorts = new List<ISort>();
            if (sortList == null)
            {
                sorts.Add(Predicates.Sort<TEntity>(null));//默认以开始时间 最早创建的时间 true=asc= 升序
                return sorts;
            }

            Type obj = sortList.GetType();
            var fields = obj.GetRuntimeFields();
            Sort s = null;
            foreach (FieldInfo f in fields)
            {
                s = new Sort();
                var mt = Regex.Match(f.Name, @"^\<(.*)\>.*$");
                s.PropertyName = mt.Groups[1].Value;
                s.Ascending = f.GetValue(sortList) == null ? true : (bool)f.GetValue(sortList);
                sorts.Add(s);
            }

            return sorts;
        }


        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose()
        {
            this.Dispose();//交给GC释放
        }
        #endregion

        #region 执行复杂sql语句返回int类型数据(不是返回影响行数)
        /// <summary>
        /// 执行复杂sql语句返回int类型数据
        /// </summary>
        /// <param name="sql">要执行的sql</param>
        /// <param name="parm">参数</param>
        /// <returns></returns>
        public int ExcuteSqlForInt(string sql, object parm)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                var result = conn.ExecuteScalar<int>(sql, parm);
                return result;
            }
        }
        #endregion

        #region 执行复杂sql语句返回当前类型数据
        /// <summary>
        /// 执行复杂sql语句返回当前类型数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public TEntity ExcuteSqlForEntity(string sql, object parm)
        {
            TEntity result = null;
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                result = conn.Query<TEntity>(sql, parm).FirstOrDefault();
                return result;
            }
        }
        #endregion

        #region 执行复杂sql语句返回当前类型数据
        /// <summary>
        /// 执行sql返回datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public DataTable ExcuteSqlForDataTable(string sql, object parm)
        {
            DataTable table = new DataTable("MyTable");
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                var reader = conn.ExecuteReader(sql, parm);
                table.Load(reader);
            }
            return table;
        }
        #endregion

        /// <summary>
        /// 执行sql语句返回影响的行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parm">条件</param>
        /// <returns></returns>
        public int ExcuteSqlForAffectedRowNumber(string sql, object parm)
        {
            using (var conn = ConnectionFactory.CreateMainSiteConnection())
            {
                //开户事务
                var trans = conn.BeginTransaction();
                var result = conn.Execute(sql, parm, trans);
                if (result > 0)
                {
                    trans.Commit();
                }
                else
                    trans.Rollback();
                return result;
            }
        }
    }

}
