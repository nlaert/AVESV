using System;
using System.Collections.Generic;
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
            string typeName = type.Name;
            if (type.IsArray)
                typeName = EmitterHelper.removeCaracteres(typeName);

            if (dict.ContainsKey(typeName))
            {
                return dict[typeName];
            }
            if (LoadAssemblies(obj))
            {
                return dict[typeName];
            }
            IJsonfier jsonfier = new JsonEmitter().CreateAssembly(type);
            dict.Add(typeName, jsonfier);
            return jsonfier;
        }

        private static bool LoadAssemblies(object obj)
        {
            Assembly asm;
            IJsonfier newObj;
            Type type = obj.GetType();
            string typeName = type.Name;
            if (type.IsArray)
                typeName = EmitterHelper.removeCaracteres(typeName);

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




    }
}
