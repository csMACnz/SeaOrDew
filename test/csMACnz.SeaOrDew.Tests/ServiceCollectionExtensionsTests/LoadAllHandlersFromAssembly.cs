using System;
using System.Reflection;
using System.Reflection.Emit;
using Xunit;
using static csMACnz.SeaOrDew.Tests.TestHandlers.Stats;

namespace csMACnz.SeaOrDew.Tests.ServiceCollectionExtensionsTests
{
    public class LoadAllHandlersFromAssembly
    {
        [Fact]
        public void CorrectlyAddsHandlers()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(options =>
            {
                options.LoadAllHandlersFromAssembly(TestHandlersAssembly);
            });

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
            Assert.Equal(OtherTestsHandlersCount + AssemblyHandlerCount + 2, fakeCollection.Count);
        }

        [Fact]
        public void CorrectlyHandlesTypeloadFailures()
        {
            var assemblyName = new AssemblyName("TestAssembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var module = assemblyBuilder.DefineDynamicModule("MainModule");
            var typeBuilder = module.DefineType(name: "TestType", attr: TypeAttributes.Public | TypeAttributes.Class);
            
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(options =>
            {
                options.LoadAllHandlersFromAssembly(module.Assembly);
            });

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
            Assert.Equal(2, fakeCollection.Count);
        }

    }
}