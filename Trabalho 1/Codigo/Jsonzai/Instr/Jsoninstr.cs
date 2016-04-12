using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Jsonzai.Instr
{
    class Jsoninstr
    {
        Dictionary<String, IJsonfier> dict = new Dictionary<string, IJsonfier>();

        public static void ToJson(object obj)
        {
            LoadAssemblies();
            IJsonfier jsonfier = FindObject(obj);
            jsonfier.Jsonfy(obj);


        }

        private static IJsonfier FindObject(object obj)
        {
            throw new NotImplementedException();
        }

        private static void LoadAssemblies()
        {
            
        }
    }
}
