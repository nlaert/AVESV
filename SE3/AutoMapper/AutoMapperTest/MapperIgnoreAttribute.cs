using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapperTest
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    class MapperIgnoreAttribute : Attribute
    {
    }
}
