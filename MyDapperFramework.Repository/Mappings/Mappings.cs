using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MyDapperFramework.Repository.Mappings
{
    public class Mappings
    {
        public static void Initialize()
        {
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(PluralizedAutoClassMapper<>);

            DapperExtensions.DapperExtensions.SetMappingAssemblies(new[] 
            { 
                typeof(Mappings).Assembly 
            });
        }
    }
}
