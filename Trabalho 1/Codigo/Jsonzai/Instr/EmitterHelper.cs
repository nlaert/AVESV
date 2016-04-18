using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsonzai.Instr
{
    public class EmitterHelper
    {

        public static string GetPrimitiveValue(object src)
        {
            Type type = src.GetType();
            if (type == typeof(string))
                return "\"" + src.ToString() + "\"";
            if (type == typeof(float))
            {
                float aux = (float)src;
                return aux.ToString(CultureInfo.InvariantCulture);
            }
            if (type == typeof(double))
            {
                double aux = (double)src;
                return aux.ToString(CultureInfo.InvariantCulture);
            }

            else
                return src.ToString();
        }

        public static string removeCaracteres(string typeName)
        {
            return typeName.Substring(0, typeName.Length - 2);
        }
    }
}
