using System;
using System.Linq;
using System.Reflection;

namespace csMACnz.SeaOrDew
{
    public static class GenericTypeExtensions
    {
        public static bool IsConcreteInstanceOfGenericTypeDefinition(this TypeInfo type, Type genericTypeDefinition)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genericTypeDefinition;
        }

        public static Type GetConcreteInterfaceImplementationForGenericInterface(this TypeInfo type, Type genericInterfaceTypeDefinition)
        {
            var genericInterfaceTypeDefinitionInfo = genericInterfaceTypeDefinition.GetTypeInfo();
            if (!genericInterfaceTypeDefinitionInfo.IsInterface ||
                !genericInterfaceTypeDefinitionInfo.IsGenericTypeDefinition)
                throw new ArgumentException("Only generic interface definition types can be checked.", nameof(genericInterfaceTypeDefinition));

            return type.ImplementedInterfaces.FirstOrDefault(i => i.GetTypeInfo().IsConcreteInstanceOfGenericTypeDefinition(genericInterfaceTypeDefinition));
        }
    }
}