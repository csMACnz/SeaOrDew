using System;
using System.Collections.Generic;
using csMACnz.SeaOrDew.Tests.TestHandlers.SetA;

namespace csMACnz.SeaOrDew.Tests.Fakes
{
    internal class FakeServiceProvider:IServiceProvider
    {
        private Dictionary<Type, object> _data = new Dictionary<Type, object>();

        public object GetService(Type serviceType)
        {
            if(_data.ContainsKey(serviceType))
            {
                return _data[serviceType];
            }
            throw new Exception($"{serviceType.FullName} Not found.");
        }

        public  void AddHandler<T1,TError>(ICustomCommandHandler<T1, CommandResult<TError>> instance)
        {
            Add<ICustomCommandHandler<T1, CommandResult<TError>>>(instance);
        }

        public  void AddHandler<T1, T2>(ICustomCommandHandler<T1, T2> instance)
        {
            Add<ICustomCommandHandler<T1, T2>>(instance);
        }

        public void AddHandler<T1, T2>(IQueryHandler<T1, T2> instance)
        {
            Add<IQueryHandler<T1, T2>>(instance);
        }

        public void Add<TType>(TType instance)
        {
            _data[typeof(TType)] = instance;
        }
    }
}