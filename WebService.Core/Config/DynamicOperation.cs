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
            AssemblyName asmName = new AssemblyName("dynamic.operation");
            var assembly = Assembly.GetAssembly(typeof(Operation));
            AssemblyBuilder asmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
            ModuleBuilder mb = asmBuilder.DefineDynamicModule(asmName.Name + ".dll");

            string enumTypeName = string.Format("{0}.{1}", typeof(Operation).Namespace, "DynamicOperation");
            EnumBuilder eb = mb.DefineEnum(enumTypeName, TypeAttributes.Public, typeof(int));

            Type flagAttrType = typeof(FlagsAttribute);
            ConstructorInfo flagAttrConstructorInfo = flagAttrType.GetConstructor(new Type[] { });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(flagAttrConstructorInfo, new object[] { });
            eb.SetCustomAttribute(attributeBuilder);

            eb.DefineLiteral("User_Read", 9009);
            var typeinfo = eb.CreateTypeInfo();

        }
    }
}
