
using MyDapperFramework.Repository.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDapperFramework.Service
{
   public class BaseService
    {
       public BaseService()
       {
           Mappings.Initialize(); 
       }
    }
}
