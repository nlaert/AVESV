
using Jsonzai.Test.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsonzai.Instr
{
    class JsonIntrsConsoleTest
    {
        public static void Main()
        {
            var s = new Student(35466, "Nick");

            Course expected = new Course
           (
               "AVE",
              new Student(35466, "Nick")
               
           );
            Jsoninstr.ToJson(expected);
        }
    }
}
