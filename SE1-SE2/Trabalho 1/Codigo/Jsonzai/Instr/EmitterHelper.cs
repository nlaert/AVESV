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

        public static string RemoveCaracteres(string typeName)
        {
            //return typeName.Substring(0, typeName.Length - 2);
            return typeName;
        }

        private static string ArrayToJson(object[] src)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("[");
            for (int i = 0; i < src.Length; i++)
            { 
                if(src[i].GetType().IsPrimitive)
                {
                    strBuilder.Append(EmitterHelper.GetPrimitiveValue(src[i]));
                }
                else
                {
                    strBuilder.Append(Jsoninstr.ToJson(src[i]));
                }
              
                if (i < src.Length - 1)
                    strBuilder.Append(",");
            }
            strBuilder.Append("]");
            return strBuilder.ToString();

        }
    }
}
