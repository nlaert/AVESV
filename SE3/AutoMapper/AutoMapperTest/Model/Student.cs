using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapperTest
{
    class Student { 
        public string Name { get; set; } 
        public int Nr { get; set; }
        public Professor prof { get; set; }
        public Professor2 prof2 { get; set; }
    }
}
