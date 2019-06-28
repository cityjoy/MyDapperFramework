
using DapperExtensions;
using MyDapperFramework.Repository;
using MyDapperFramework.Repository.Entity;
using MyDapperFramework.Repository.Mappings;
using MyDapperFramework.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyDapperFramework.Service
{
    /// <summary>
    /// 服务基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseService<TEntity> where TEntity : class
    {
        BaseRepository<TEntity> repository;
        public BaseService()
        {
            Mappings.Initialize();
            repository = new BaseRepository<TEntity>();
        }
        #region 数据操作

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity Insert(TEntity entity)
        {
            return repository.Insert(entity);
        }

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool Insert(IEnumerable<TEntity> entitys)
        {
            return repository.Insert(entitys);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(TEntity entity)
        {
            return repository.Update(entity);
        }

        /// <summary>
        /// 根据sql语句更新符合条件的数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public bool Update(string sql, object parm)
        {
            return repository.Update(sql, parm);
        }

        /// <summary>
        /// 更新实体指定字段数据
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool Update(object parameters, Expression<Func<TEntity, bool>> expression)
        {
            return repository.Update(parameters, expression);

        }



        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool Update(IEnumerable<TEntity> entitys)
        {
            return repository.Update(entitys);

        }

        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(object key)
        {
            return repository.Delete(key);

        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool Delete(IEnumerable<object> keys)
        {
            return repository.Delete(keys);

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
            return repository.GetById(id);

        }

        /// <summary>
        /// 获取数据表总项数
        /// </summary>
        /// <param name="expression">linq表达式 谓词</param>
        /// <returns></returns>
        public long GetCount(Expression<Func<TEntity, bool>> expression = null)
        {
            return repository.GetCount(expression);

        }

        /// <summary>
        /// 获取结果集第一条数据
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="sortList"></param>
        /// <returns></returns>
        public TEntity GetFist(Expression<Func<TEntity, bool>> expression = null, object sortList = null)
        {
            return repository.GetFist(expression, sortList);

        }

        /// <summary>
        /// 查看指定的数据是否存在
        /// </summary>
        /// <param name="expression">linq表达式 谓词</param>
        /// <returns></returns>
        public bool Exists(Expression<Func<TEntity, bool>> expression)
        {
            return repository.Exists(expression);

        }

        /// <summary>
        /// 根据条件获取表数据
        /// </summary>
        /// <param name="expression">linq表达式</param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> expression, object sortList = null)
        {
            return repository.GetList(expression, sortList);

        }

        /// <summary>
        /// 数据表指定字段分页
        /// </summary>
        /// <param name="pager">页数信息</param>
        /// <param name="fields">要查询是字段对象</param>
        /// <param name="expression">条件 linq表达式 谓词</param>
        /// <param name="sortList">排序字段</param>
        /// <returns></returns>
        public List<TEntity> GetPageData(XPager pager, string[] fields, Expression<Func<TEntity, bool>> expression = null, object sortList = null)
        {
            return repository.GetPageData(pager, fields, expression, sortList);

        }


        /// <summary>
        /// 数据表所有字段分页
        /// </summary>
        /// <param name="pager">页数信息</param>
        /// <param name="expression">条件 linq表达式 谓词</param>
        /// <param name="sortList">排序字段</param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPageData(XPager pager, Expression<Func<TEntity, bool>> expression = null, object sortList = null)
        {
            return repository.GetPageData(pager, expression, sortList);

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
        public IEnumerable<TEntity> GetPageData(int pageNum, int pageSize, out long outTotal,
            Expression<Func<TEntity, bool>> expression = null, object sortList = null)
        {
            return repository.GetPageData(pageNum, pageSize, out   outTotal, expression = null, sortList = null);

        }
        /// <summary>
        /// 获取id范围内的列表
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<TEntity> GetListInIds(string fields, int[] ids)
        {
            string entityName = typeof(TEntity).Name;
            List<TEntity> list = repository.GetListInIds(entityName, fields, ids);
            return list;
        }

        #endregion


    }
}
