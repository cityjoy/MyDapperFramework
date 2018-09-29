using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using MyDapperFramework.Repository.Entity;
using MyDapperFramework.Repository.Repository;
namespace MyDapperFramework.Service
{

    public class UserService : BaseService<User>
    {

        /// <summary>
        /// 更新性别
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public bool UpdateUser(byte sex, int userId)
        {
            bool result = Update(new { Sex = 1 }, user => user.Id == userId);
            return result;
        }
        /// <summary>
        /// 获取分页列表 
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="ps">每页几条(默认15条,最多50条)</param>
        /// <returns></returns>
        public UserListViewModel GetPageListData(XPager pager)
        {
            UserListViewModel model = new UserListViewModel();
            List<User> list = GetPageData(pager, m => m.Sex == 1, null).ToList();
            model.UserList = list;
            model.Pager = pager;
            return model;
        }
    }
}
