using Data.Entitys;
using EnumsNET;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace WebService.Core.Authorization
{
    public static class DynamicOperation
    {

        public static void CreateDynamicOperation()
        {
            AssemblyName asmName = new AssemblyName("public.operation");
            var assembly = Assembly.GetAssembly(typeof(PublicOperation));
            AssemblyBuilder asmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
            ModuleBuilder mb = asmBuilder.DefineDynamicModule(asmName.Name + ".dll");

            string enumTypeName = string.Format("{0}.{1}", typeof(PublicOperation).Namespace, "PublicOperation");
            EnumBuilder eb = mb.DefineEnum(enumTypeName, TypeAttributes.Public, typeof(int));

            Type flagAttrType = typeof(FlagsAttribute);
            ConstructorInfo flagAttrConstructorInfo = flagAttrType.GetConstructor(new Type[] { });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(flagAttrConstructorInfo, new object[] { });
            eb.SetCustomAttribute(attributeBuilder);
            foreach (var type in typeof(EntityBase).Assembly.GetTypes())
            {
                if (type.BaseType == typeof(EntityBase))
                {
                    short flag = 9000;
                    foreach (var e in Enums.GetMembers<PublicOperation>())
                    {
                        eb.DefineLiteral($"{type.Name}_{e.AsString()}", e.ToInt16() | flag);
                        flag++;
                    }
                }
            }
            var typeinfo = eb.CreateTypeInfo();
        }
    }
}
