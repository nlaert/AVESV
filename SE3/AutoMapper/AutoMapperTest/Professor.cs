using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapperTest
{
    public class Professor
    {
        [MapperIgnore]
        public string Name { get; set; }
        public int Nr { get; set; } 
    }
}
