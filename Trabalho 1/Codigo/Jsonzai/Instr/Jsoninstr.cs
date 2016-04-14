using System;
using System.Collections.Generic;
using System.Reflection;


namespace Jsonzai.Instr
{
    class Jsoninstr
    {
        private static Dictionary<String, IJsonfier> dict = new Dictionary<string, IJsonfier>();

        public static string ToJson(object obj)
        {
            IJsonfier jsonfier = FindObject(obj);
            return jsonfier.Jsonfy(obj);
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
            var jsonfier = JsonEmitter.CreateAssembly(type);
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
                newObj = (IJsonfier)asm.CreateInstance(typeName);
            }
            catch (Exception)
            { 
                return false;
            }
            dict.Add(typeName, newObj);
            return true;
        }
    }
}
