using System;
using System.Collections.Generic;

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

        public void Add<TType>(TType instance)
        {
            _data[typeof(TType)] = instance;
        }
    }
}