using Data.Entitys;
using EnumsNET;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace WebService.Core.Authorization
{
    public class DynamicOperationTypeInfo
    {
        public TypeInfo TypeInfo { get; set; }
    }

    public static class DynamicOperation
    {
        public static string DynamicNameSpace = $"{typeof(Operation).Namespace}.Dynamic";
        public static DynamicOperationTypeInfo CreateDynamicOperation()
        {
            AssemblyName asmName = new AssemblyName("dynamic.operation");
            AssemblyBuilder asmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
            ModuleBuilder mb = asmBuilder.DefineDynamicModule(asmName.Name + ".dll");

            string enumTypeName = string.Format("{0}.{1}", DynamicNameSpace, "DynamicOperation");
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
                    foreach (var e in Enums.GetMembers<Operation>())
                    {
                        if (e.Attributes.Get<PublicAttribute>() is PublicAttribute)
                        {
                            eb.DefineLiteral($"{type.Name}_{e.AsString()}", e.ToInt16() | flag);
                            flag++;
                        }
                    }
                }
            }
            return new DynamicOperationTypeInfo
            {
                TypeInfo = eb.CreateTypeInfo()
            };
        }
    }
}
