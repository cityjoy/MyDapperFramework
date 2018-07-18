using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MyDapperFramework.Repository;
using MyDapperFramework.Repository.Entity;
namespace MyDapperFramework.Repository.Repository
{
    public class UserRepository : BaseRepository<User> 
    {
        public UserRepository()
            : base(ConnectionFactory.CreateMainSiteConnection())
        {

        }
    }
}
