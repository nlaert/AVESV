using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jsonzai.Reflect
{
    public class Jsonfier
    {
        public static string ToJson(object src)
        {
            return Route(src);
        }

        private static string Route(object src)
        {
            if (src == null)
                return "null";
            Type type = src.GetType();
            if (type.IsPrimitive || type == typeof(string))
            {
                return GetPrimitiveValue(src);
            }
            if (type.IsArray)
            {
                return GetArrayValues(src);
            }
            else
            {
                return GetObjectValues(src);
            }
        }

        private static string GetPrimitiveValue(object src)
        {
            Type type = src.GetType();
            if (type == typeof(string))
                return "\"" + src.ToString() + "\"";
            else
                return src.ToString();
        }

        private static string GetArrayValues(object src)
        {
            StringBuilder JSON = new StringBuilder("[");
            Type type = src.GetType();
            var methodInfo = type.GetMethod("GetValue", new Type[] { typeof(Int32) });

            MethodInfo lengthMethod = type.GetProperty("Length").GetGetMethod();
            int length = (int)lengthMethod.Invoke(src, null);

            for(int i = 0; i < length; i++)
            {
                object value = methodInfo.Invoke(src, new object[1] { i });
                string aux = Route(value);
                JSON.Append(aux);
                if (i < length - 1)
                    JSON.Append(",");

            }
            JSON.Append("]");
            return JSON.ToString();
        }

        private static string GetObjectValues(object src)
        {
            StringBuilder JSON = new StringBuilder("{");
            Type type = src.GetType();
            PropertyInfo[] properties = type.GetProperties();
            for(int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                JSON.Append("\"" + property.Name + "\":");
                object value = property.GetValue(src);
                string aux = Route(property.GetValue(src));
                JSON.Append(aux);
                if (i < properties.Length - 1)
                    JSON.Append(",");
            }
            JSON.Append("}");
            return JSON.ToString();
        }
    }

}
