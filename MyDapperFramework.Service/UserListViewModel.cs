using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyDapperFramework.Service
{
    public class UserListViewModel
    {
        public Repository.Entity.XPager Pager { get; set; }
        public List<Repository.Entity.User> UserList { get; set; }
    }
}
