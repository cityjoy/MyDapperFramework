using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDapperFramework.Repository.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public byte Sex { get; set; }
        public List<Blogs> Blogs { get; set; }
    }
    public class Blogs
    {

        public int Id { get; set; }

        public int UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
