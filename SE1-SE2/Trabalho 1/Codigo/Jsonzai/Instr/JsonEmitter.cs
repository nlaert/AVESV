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

        ILGenerator il;
        ConstructorInfo ctor;
        MethodInfo callGetPrimitiveValue;
        MethodInfo callToJson;
        MethodInfo strBuilderAppend;
        MethodInfo strBuilderToString;
        LocalBuilder localStringBuilder;
        LocalBuilder localString;
        LocalBuilder paramObject;
        LocalBuilder lengthLocal;






        public  IJsonfier CreateAssembly(Type type)
        {
            string typeName = type.Name;
            if (type.IsArray)
                typeName = EmitterHelper.RemoveCaracteres(typeName);
            string ASM_NAME = AssemblyNamePrefix + typeName;
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

        private  void ImplementJsonfyMethod(MethodBuilder jsonfyMethodBuilder, Type type)
        {
            il = jsonfyMethodBuilder.GetILGenerator();
            ctor = typeof(StringBuilder).GetConstructor(new Type[] { });
            callGetPrimitiveValue = typeof(EmitterHelper).GetMethod("GetPrimitiveValue");
            callToJson = typeof(Jsoninstr).GetMethod("ToJson");
            strBuilderAppend = typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) });
            strBuilderToString = typeof(StringBuilder).GetMethod("ToString", new Type[] { });

            localStringBuilder = il.DeclareLocal(typeof(StringBuilder));//0
            localString = il.DeclareLocal(typeof(string));//1
            paramObject = il.DeclareLocal(typeof(object));//2
            lengthLocal = il.DeclareLocal(typeof(int));//3
    

            il.Emit(OpCodes.Ldarg_1); //carrega o arg 1 (obj) para a stack
            il.Emit(OpCodes.Stloc_2);
            il.Emit(OpCodes.Newobj, ctor);  //inicializa StringBuilder
            il.Emit(OpCodes.Stloc_0);

            il.Emit(OpCodes.Ldarg_1);
            MethodInfo method = typeof(System.Console).GetMethod("WriteLine",
                BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(object) }, null);
            il.Emit(OpCodes.Call, method);

            if (type.IsArray)
            {
                GetArrayValues();
            }
            else if (!type.IsPrimitive && type != typeof(string))
            {
                int i = 0;
                var properties = type.GetProperties();
                AppendToStringBuilder("{");
                foreach (var p in properties)
                {
                    Type propType = p.PropertyType;

                    MethodInfo propertyGetMethod = p.GetGetMethod();

                    AppendToStringBuilder("\"" + p.Name + "\": ");
                    il.Emit(OpCodes.Ldloc_2);
                    il.Emit(OpCodes.Call, propertyGetMethod);//retorna o valor



                    if (propType.IsPrimitive || propType == typeof(string))
                    {
                        if (propType.IsValueType)
                        {
                            il.Emit(OpCodes.Box, propType);

                        }
                        else
                        {
                            lengthLocal = il.DeclareLocal(propType);
                            il.Emit(OpCodes.Castclass, typeof(object));
                           // il.Emit(OpCodes.Stloc_3);
                           // il.Emit(OpCodes.Ldloc_3);
                        }

                        il.Emit(OpCodes.Call, callGetPrimitiveValue);
                    }
                    else
                    {
                        il.Emit(OpCodes.Call, callToJson);
                    }
                    il.Emit(OpCodes.Stloc_1);
                    AppendToStringBuilder();
                    if (i < properties.Length - 1)
                        AppendToStringBuilder(",");
                    i++;

                }
                AppendToStringBuilder("}");
            }
            else
            {
                EmitPrimitiveValue(type);
            }
                
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, strBuilderToString);
            il.Emit(OpCodes.Stloc_1);
            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ret);

        }

        private  void AppendToStringBuilder()
        {
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Call, strBuilderAppend);
            il.Emit(OpCodes.Pop);
        }

        private  void AppendToStringBuilder(string str)
        {
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldstr, str);
            il.Emit(OpCodes.Call, strBuilderAppend);
            il.Emit(OpCodes.Pop);
        }

        private void EmitPrimitiveValue(Type type)
        {
            il.Emit(OpCodes.Ldloc_2);

            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, typeof(object));

            }
            else
            {
                lengthLocal = il.DeclareLocal(type);
                il.Emit(OpCodes.Castclass, typeof(object));
               // il.Emit(OpCodes.Stloc_3);
              //  il.Emit(OpCodes.Ldloc_3);
            }
            il.Emit(OpCodes.Call, callGetPrimitiveValue);
            il.Emit(OpCodes.Stloc_1);
            AppendToStringBuilder();
        }



       

        
        private void GetArrayValues()
        {
            // StringBuilder JSON = new StringBuilder("[");
           // AppendToStringBuilder("[");

            il.Emit(OpCodes.Ldloc_2);
            MethodInfo callArrayToJson = typeof(EmitterHelper).GetMethod("ArrayToJson");
            il.Emit(OpCodes.Call, callArrayToJson);
            il.Emit(OpCodes.Stloc_1);
            AppendToStringBuilder();
          //  AppendToStringBuilder("]");

        }

       



        //    object value = methodGetValue.Invoke(src, new object[1] { i });
        //    string aux = Route(value);
        //    JSON.Append(aux);
        //    if (i < length - 1)
        //        JSON.Append(",");

        //}
        //    AppendToStringBuilder("]");




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
