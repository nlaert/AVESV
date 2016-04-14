using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Jsonzai.Instr
{
    public class JsonEmitter
    {
        public static string AssemblyNamePrefix = "JsonfyFor";
        public static string AssemblyFileExtension = ".dll";

        public static IJsonfier CreateAssembly(Type type)
        {
            string ASM_NAME = AssemblyNamePrefix + type.Name;
            string MOD_NAME = ASM_NAME;
            string TYP_NAME = ASM_NAME;
            string DLL_NAME = ASM_NAME + AssemblyFileExtension;

            AssemblyBuilder asmBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(ASM_NAME), AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule(MOD_NAME, DLL_NAME);
            TypeBuilder typeBuilder = modBuilder.DefineType(TYP_NAME);
            typeBuilder.AddInterfaceImplementation(typeof(IJsonfier));

            MethodBuilder JsonfyMethodBuilder = typeBuilder.DefineMethod(
                "Jsonfy",
                MethodAttributes.Public | 
                MethodAttributes.Virtual | 
                MethodAttributes.ReuseSlot,
                typeof(string),
                new Type[1] { typeof(object) });

            ImplementJsonfyMethod(JsonfyMethodBuilder, type);

            Type jsonfierType = typeBuilder.CreateType();
            asmBuilder.Save(DLL_NAME);


            return (IJsonfier)Activator.CreateInstance(jsonfierType);
        }

        private static void ImplementJsonfyMethod(MethodBuilder jsonfyMethodBuilder, Type type)
        {
            ILGenerator il = jsonfyMethodBuilder.GetILGenerator();

            MethodInfo callGetPrimitiveValue = typeof(Jsoninstr).GetMethod("GetPrimitiveValue");
            MethodInfo callToJson = typeof(Jsoninstr).GetMethod("ToJson");
            LocalBuilder tobj;


            if (!type.IsPrimitive && type != typeof(string))
            {
                foreach (var p in type.GetProperties())
                {
                    tobj = il.DeclareLocal(type);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Castclass, type);
                    if (type.IsPrimitive || type == typeof(string))
                        il.Emit(OpCodes.Call, callGetPrimitiveValue);
                    else
                    {
                        il.Emit(OpCodes.Ldloc_0, p.GetValue());
                        il.Emit(OpCodes.Call, callToJson);
                    }
                        
                }
                
            }
            else
            {
                tobj = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Castclass, type);
                il.Emit(OpCodes.Call, callGetPrimitiveValue);
            }
            
            il.Emit(OpCodes.Ret);

        }

       





        /*
        il.Emit(OpCodes.Ldstr, "Hello World");
        MethodInfo method = typeof(System.Console).GetMethod("WriteLine",
            BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string) }, null);
        il.Emit(OpCodes.Call, method);
        il.Emit(OpCodes.Ldstr, "Hello World");
        il.Emit(OpCodes.Ret);

         */



    }
    
}
