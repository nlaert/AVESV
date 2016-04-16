using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;


namespace Jsonzai.Instr
{
    public class Jsoninstr
    {
        private static Dictionary<String, IJsonfier> dict = new Dictionary<string, IJsonfier>();

        public static string ToJson(object obj)
        {
            IJsonfier jsonfier = FindObject(obj);
            String aux = "";
            try {
                aux = jsonfier.Jsonfy(obj);
            } catch(Exception e)
            {
                Console.Write(e);
            }
            
            return aux;
           // return jsonfier.Jsonfy(obj);
        }

        private static IJsonfier FindObject(object obj)
        {
            Type type = obj.GetType();
            if (dict.ContainsKey(type.Name))
            {
                return dict[type.Name];
            }
            if (LoadAssemblies(obj))
            {
                return dict[type.Name];
            }
            IJsonfier jsonfier = JsonEmitter.CreateAssembly(type);
            dict.Add(type.Name, jsonfier);
            return jsonfier;
        }

        private static bool LoadAssemblies(object obj)
        {
            Assembly asm;
            IJsonfier newObj;
            string typeName = obj.GetType().Name;
            try
            {
                asm = Assembly.LoadFrom(JsonEmitter.AssemblyNamePrefix + typeName + JsonEmitter.AssemblyFileExtension);
                newObj = (IJsonfier)asm.CreateInstance(JsonEmitter.AssemblyNamePrefix + typeName);
            }
            catch (Exception)
            { 
                return false;
            }
            dict.Add(typeName, newObj);
            return true;
        }

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


    }
}
