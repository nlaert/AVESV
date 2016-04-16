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
            ConstructorInfo ctor = typeof(StringBuilder).GetConstructor(new Type[] { });
            MethodInfo callGetPrimitiveValue = typeof(Jsoninstr).GetMethod("GetPrimitiveValue");
            MethodInfo callToJson = typeof(Jsoninstr).GetMethod("ToJson");
            MethodInfo strBuilderAppend = typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) });
            MethodInfo strBuilderToString = typeof(StringBuilder).GetMethod("ToString", new Type[] { });

            LocalBuilder localStringBuilder = il.DeclareLocal(typeof(StringBuilder));//0
            LocalBuilder localString = il.DeclareLocal(typeof(string));//1
            LocalBuilder paramObject = il.DeclareLocal(typeof(object));//2
            LocalBuilder localObject = il.DeclareLocal(typeof(object));//3

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Stloc_2);
            il.Emit(OpCodes.Newobj, ctor);  //inicializa StringBuilder
            il.Emit(OpCodes.Stloc_0);

            if (!type.IsPrimitive && type != typeof(string))
            {
                int i = 0;
                var properties = type.GetProperties();
                AppendToStringBuilder(il, strBuilderAppend, "{");
                foreach (var p in properties)
                {
                    Type propType = p.PropertyType;

                    MethodInfo propertyGetMethod = p.GetGetMethod();
                    AppendToStringBuilder(il, strBuilderAppend, "\"" + p.Name + "\": ");
                    il.Emit(OpCodes.Ldloc_2);
                    il.Emit(OpCodes.Call, propertyGetMethod);

                    if (propType.IsPrimitive || propType == typeof(string))
                    {
                        il.Emit(OpCodes.Call, callGetPrimitiveValue);
                    }
                    else
                    {
                        il.Emit(OpCodes.Call, callToJson);
                    }
                    il.Emit(OpCodes.Stloc_1);
                    AppendToStringBuilder(il, strBuilderAppend);
                    if (i < properties.Length - 1)
                        AppendToStringBuilder(il, strBuilderAppend, ",");
                    i++;

                }
                AppendToStringBuilder(il, strBuilderAppend, "}");
            }
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, strBuilderToString);
            il.Emit(OpCodes.Stloc_1);
            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ret);

        }

        private static void AppendToStringBuilder(ILGenerator il, MethodInfo strBuilderAppend)
        {
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Call, strBuilderAppend);
            il.Emit(OpCodes.Pop);
        }

        private static void AppendToStringBuilder(ILGenerator il, MethodInfo strBuilderAppend, string str)
        {
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldstr, str);
            il.Emit(OpCodes.Call, strBuilderAppend);
            il.Emit(OpCodes.Pop);
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
