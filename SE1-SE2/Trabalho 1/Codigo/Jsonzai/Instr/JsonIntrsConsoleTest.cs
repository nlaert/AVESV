
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
                new Student[4]{
                    new Student(27721, "Ze Manel"),
                    new Student(15642, "Maria Papoila"),
                    null,
                    null
                }
            );

            Course2 course2 = new Course2
           (
               "AVE",new Student(27721, "Ze Manel")

           );

            // Console.WriteLine(Jsoninstr.ToJson(course2));
            Console.WriteLine(Jsoninstr.ToJson("batatas"));

        }
    }
}
