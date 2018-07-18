using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyDapperFramework.Repository.Interface
{
    /// <summary>
    /// 基础数据访问接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IDataAccess<TEntity> where TEntity : class
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// 批量添加实体 事物操作
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        bool Insert(IEnumerable<TEntity> entitys);


        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Update(TEntity entity);


        /// <summary>
        /// 只更新部分字段
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="prams">参数 new {name="xxx"}</param>
        /// <returns></returns>
        bool Update(object id, object prams);


        /// <summary>
        /// 批量更改实体 事物操作
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        bool Update(IEnumerable<TEntity> entitys);


        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        bool Delete(object key);


        /// <summary>
        /// 批量删除实体 事物操作
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Delete(IEnumerable<object> key);


        /// <summary>
        /// 逻辑删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool DeleteLogic(object key);


        /// <summary>
        /// 获取实体根据Id
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        TEntity GetById(object id);


        /// <summary>
        /// 实现此接口 必须使用Sql聚合函数
        ///</summary>
        /// <returns></returns>
        long GetCount(Expression<Func<TEntity, bool>> expression = null);


        /// <summary>
        /// 获取结果集第一条数据
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="sortList">如排序字段 new {Name=true,Age=flase} 就是Name升序然后再按照Age降序</param>
        /// <returns></returns>
        TEntity GetFist(Expression<Func<TEntity, bool>> expression = null, object sortList = null);


        /// <summary>
        /// 获取数据表数据
        /// </summary>
        /// <param name="expression">linq 表达式 谓词</param>
        /// <param name="sortList">如排序字段 new {Name=true,Age=flase} 就是Name升序然后再按照Age降序</param>
        /// <returns></returns>
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> expression, object sortList = null);



        /// <summary>
        /// 数据表 分页
        /// </summary>
        /// <param name="pageNum">指定页数</param>
        /// <param name="pageSize">指定每页多少项</param>
        ///  <param name="outTotal">输出当前表总数</param>
        /// <param name="expression">条件 linq表达式 谓词</param>
        /// <param name="sort">如排序字段 new {Name=true,Age=flase} 就是Name升序然后再按照Age降序</param>
        /// <returns></returns>
        IEnumerable<TEntity> GetPageData(int pageNum, int pageSize, out long outTotal, Expression<Func<TEntity, bool>> expression = null, object sort = null);


        /// <summary>
        /// 查看指定的数据是否存在
        /// </summary>
        /// <param name="expression">linq表达式 谓词</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> expression);

    }


}
