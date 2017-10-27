using System;
using System.Collections.Generic;
using System.Reflection;
using csMACnz.SeaOrDew.Tests.TestHandlers.SetA;
using Xunit;

namespace csMACnz.SeaOrDew.Tests
{
    public class GenericTypeExtensionsTests
    {
        [Fact]
        public void GetConcreteInterfaceImplementationForGenericInterface_NotAGenericOrInterface_Throws()
        {
            var aType = typeof(CommandError).GetTypeInfo();
            var generic = typeof(CommandError);
            var exception = Assert.Throws<ArgumentException>(() => GenericTypeExtensions.GetConcreteInterfaceImplementationForGenericInterface(aType, generic));

            AssertCorrectArgumentException(exception);
        }

        [Fact]
        public void GetConcreteInterfaceImplementationForGenericInterface_GenericNotAnInterface_Throws()
        {
            var aType = typeof(CommandError).GetTypeInfo();
            var generic = typeof(List<>);
            var exception = Assert.Throws<ArgumentException>(() => GenericTypeExtensions.GetConcreteInterfaceImplementationForGenericInterface(aType, generic));

            AssertCorrectArgumentException(exception);
        }
        
        [Fact]
        public void GetConcreteInterfaceImplementationForGenericInterface_InterfaceNotGeneric_Throws()
        {
            var aType = typeof(CommandError).GetTypeInfo();
            var generic = typeof(IFormattable);
            var exception = Assert.Throws<ArgumentException>(() => GenericTypeExtensions.GetConcreteInterfaceImplementationForGenericInterface(aType, generic));

            AssertCorrectArgumentException(exception);
        }

        [Fact]
        public void GetConcreteInterfaceImplementationForGenericInterface_TypesMatch_True()
        {
            var aType = typeof(TestCommandHandler).GetTypeInfo();
            var generic = typeof(ICustomCommandHandler<,>);
            var result = GenericTypeExtensions.GetConcreteInterfaceImplementationForGenericInterface(aType, generic);

            Assert.Equal(typeof(ICustomCommandHandler<TestCommand, CommandResult<CommandError>>), result);
        }

        [Fact]
        public void GetConcreteInterfaceImplementationForGenericInterface_TypesDoNotMatch_Null()
        {
            var aType = typeof(CommandError).GetTypeInfo();
            var generic = typeof(ICustomCommandHandler<,>);
            var result = GenericTypeExtensions.GetConcreteInterfaceImplementationForGenericInterface(aType, generic);

            Assert.Null(result);
        }
        
        [Fact]
        public void IsConcreteInstanceOfGenericTypeDefinition_TypesMatch_True()
        {
            var aType = typeof(CommandResult<CommandError>).GetTypeInfo();
            var generic = typeof(CommandResult<>);
            var result = GenericTypeExtensions.IsConcreteInstanceOfGenericTypeDefinition(aType, generic);

            Assert.True(result);
        }

        [Fact]
        public void IsConcreteInstanceOfGenericTypeDefinition_TypesDoNotMatch_False()
        {
            var aType = typeof(CommandError).GetTypeInfo();
            var generic = typeof(CommandResult<>);
            var result = GenericTypeExtensions.IsConcreteInstanceOfGenericTypeDefinition(aType, generic);

            Assert.False(result);
        }
        
        private void AssertCorrectArgumentException(ArgumentException exception)
        {
            Assert.Equal("Only generic interface definition types can be checked.\r\nParameter name: genericInterfaceTypeDefinition", exception.Message);
            Assert.Equal("genericInterfaceTypeDefinition", exception.ParamName);
        }
    }
}