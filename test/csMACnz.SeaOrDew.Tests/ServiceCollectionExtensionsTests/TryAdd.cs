using csMACnz.SeaOrDew.Tests;
using csMACnz.SeaOrDew.Tests.TestHandlers.SetA;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static csMACnz.SeaOrDew.Tests.TestHandlers.Stats;
using System.Linq;

namespace csMACnz.SeaOrDew.Tests
{
    public class TryAdd
    {
        [Fact]
        public void TryAdd_WithExistingInstanceRegistered_DoesNotReregister()
        {
            var fakeCollection = new TestServiceCollection();
            var testDescriptor = new ServiceDescriptor(
                typeof(ICustomCommandHandler<CustomTestCommand, CustomTestCommandResult>),
                new object());
            fakeCollection.Add(testDescriptor);

            fakeCollection.AddSeaOrDewHandlers(options =>
            {
                options.LoadCommandHandlersFromAssemblyUnderNamespace(TestHandlersAssembly, "csMACnz.SeaOrDew.Tests.TestHandlers.SetA");
                options.TryAdd = true;
            });

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
            Assert.Equal(SetACommandCount + 2, fakeCollection.Count);
            Assert.Contains(fakeCollection, s => s == testDescriptor);
        }

        [Fact]
        public void TryAddIsFalse_WithExistingInstanceRegistered_RegistersExtra()
        {
            var fakeCollection = new TestServiceCollection();
            var testDescriptor = new ServiceDescriptor(
                typeof(ICustomCommandHandler<CustomTestCommand, CustomTestCommandResult>),
                new object());
            fakeCollection.Add(testDescriptor);

            fakeCollection.AddSeaOrDewHandlers(options =>
            {
                options.LoadCommandHandlersFromAssemblyUnderNamespace(TestHandlersAssembly, "csMACnz.SeaOrDew.Tests.TestHandlers.SetA");
                options.TryAdd = false;
            });

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
            Assert.Equal(SetACommandCount + 2 + 1, fakeCollection.Count);
            Assert.Contains(fakeCollection, s => s == testDescriptor);
            Assert.Equal(2, fakeCollection.Count(s => s.ServiceType == typeof(ICustomCommandHandler<CustomTestCommand, CustomTestCommandResult>)));
        }

        [Fact]
        public void TryAdd_WithoutExistingInstanceRegistered_AllRegister()
        {
            var fakeCollection = new TestServiceCollection();

            fakeCollection.AddSeaOrDewHandlers(options =>
            {
                options.LoadCommandHandlersFromAssemblyUnderNamespace(TestHandlersAssembly, "csMACnz.SeaOrDew.Tests.TestHandlers.SetA");
                options.TryAdd = true;
            });

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
            Assert.Equal(SetACommandCount + 2, fakeCollection.Count);
        }

        [Fact]
        public void TryAdd_WithExistingHandlersRegistered_DoesNotReregister()
        {
            var fakeCollection = new TestServiceCollection();
            var commandInstanceDescriptor = new ServiceDescriptor(typeof(CommandHandler), new object());
            fakeCollection.Add(commandInstanceDescriptor);
            var queryInstanceDescriptor = new ServiceDescriptor(typeof(QueryHandler), new object());
            fakeCollection.Add(queryInstanceDescriptor);

            fakeCollection.AddSeaOrDewHandlers(options =>
            {
                options.TryAdd = true;
            });

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler) && s == commandInstanceDescriptor);
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler) && s == queryInstanceDescriptor);
            Assert.Equal(2, fakeCollection.Count);
        }

        [Fact]
        public void TryAddIsFalse_WithExistingHandlersRegistered_RegistersExtra()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.Add(new ServiceDescriptor(typeof(CommandHandler), new object()));
            fakeCollection.Add(new ServiceDescriptor(typeof(QueryHandler), new object()));

            fakeCollection.AddSeaOrDewHandlers(options =>
            {
                options.TryAdd = false;
            });

            Assert.Equal(2, fakeCollection.Count(s => s.ServiceType == typeof(CommandHandler)));
            Assert.Equal(2, fakeCollection.Count(s => s.ServiceType == typeof(QueryHandler)));
            Assert.Equal(4, fakeCollection.Count);
        }
    }
}