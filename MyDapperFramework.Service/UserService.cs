using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using MyDapperFramework.Repository.Entity;
using MyDapperFramework.Repository.Repository;
namespace MyDapperFramework.Service
{

    public class UserService : BaseService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool AddUser(User user)
        {
            UserRepository rep = new UserRepository();
            rep.Insert(user);
            return user.Id > 0;
        }
        /// <summary>
        /// 更新性别
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public bool UpdateUser(byte sex,int userId)
        {
            UserRepository rep = new UserRepository();
            bool result = rep.Update(new { Sex = 0 }, user => user.Id == userId);
            return result;
        }
        /// <summary>
        /// 获取所有活动分页列表 
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="ps">每页几条(默认15条,最多50条)</param>
        /// <returns></returns>
        public UserListViewModel GetPageListData( int page = 1, int ps = 15)
        {
            UserListViewModel model = new UserListViewModel();
            if (ps > 50) { ps = 50; }
            XPager pager = new XPager(page, ps, 0);
            UserRepository rep = new UserRepository();
            List<User> list = rep.GetPageData(pager, m => m.Sex == 1, null).ToList();
            model.UserList = list;
            model.Pager = pager;

            return model;
        }
    }
}
