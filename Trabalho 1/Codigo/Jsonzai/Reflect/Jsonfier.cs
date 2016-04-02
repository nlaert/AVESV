using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jsonzai.Reflect
{
    public class Jsonfier
    {
        public enum Selection { Fields, Properties, Methods };
        public static Selection Selected;
        public static string ToJson(object src, Selection option)
        {
            Selected = option;
            return Route(src);
        }

        public static string ToJson(object src)
        {
            Selected = Selection.Fields;
            return Route(src);
        }

        public static string ToJson(Type type)
        {
            return Route(type);
        }

        public static string Route(Type type)
        {
            if (type.IsEnum)
            {
                return GetEnumNames(type);
            }
            return Route((object)type);
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
                switch (Selected)
                {
                    case Selection.Fields:
                        return GetFieldValues(src);
                    case Selection.Methods:
                        return GetNonVoidMethods(src);
                    default:
                        return GetPropertyValues(src);
                }
            }
        }

        private static string GetPrimitiveValue(object src)
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

       
        private static string GetEnumNames(Type type)
        {
            StringBuilder JSON = new StringBuilder("[");

            
            Array valueNames = Enum.GetNames(type);
            for (int i = 0; i < valueNames.Length; i++)
            {
                object aux = valueNames.GetValue(i);
                string jsonAux = Route(aux);
                JSON.Append(jsonAux);
                if (i < valueNames.Length - 1)
                    JSON.Append(",");
            }
            JSON.Append("]");
            return JSON.ToString();
        }

        private static string GetArrayValues(object src)
        {
            StringBuilder JSON = new StringBuilder("[");
            Type type = src.GetType();
            var methodInfo = type.GetMethod("GetValue", new Type[] { typeof(Int32) });

            MethodInfo lengthMethod = type.GetProperty("Length").GetGetMethod();
            int length = (int)lengthMethod.Invoke(src, null);

            for (int i = 0; i < length; i++)
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

        private static string GetPropertyValues(object src)
        {
            StringBuilder JSON = new StringBuilder("{");
            Type type = src.GetType();
            PropertyInfo[] properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                JSON.Append("\"" + property.Name + "\":");
                string aux = Route(property.GetValue(src));
                JSON.Append(aux);
                if (i < properties.Length - 1)
                    JSON.Append(",");
            }
            JSON.Append("}");
            return JSON.ToString();
        }

        private static string GetFieldValues(object src)
        {
            StringBuilder JSON = new StringBuilder("{");
            Type type = src.GetType();
            var fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                JSON.Append("\"" + field.Name + "\":");
                string aux = Route(field.GetValue(src));
                JSON.Append(aux);
                if (i < fields.Length - 1)
                    JSON.Append(",");
            }
            JSON.Append("}");
            return JSON.ToString();
        }

        private static string GetNonVoidMethods(object src)
        {
            throw new NotImplementedException();
        }
    }

}
