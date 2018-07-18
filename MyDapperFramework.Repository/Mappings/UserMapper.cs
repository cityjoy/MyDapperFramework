using DapperExtensions.Mapper;
using MyDapperFramework.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDapperFramework.Repository.Mappings
{
    public class UserMapper : ClassMapper<User>
    {
        public UserMapper()
        {
            base.Table("User");
            Map(f => f.Id).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
            Map(f => f.Blogs).Ignore();//设置忽略
          
            AutoMap();
        }
    }
}
