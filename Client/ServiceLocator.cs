using System;
using System.Collections.Generic;

namespace Monopoly
{
    internal class ServiceLocator
    {
        private static ServiceLocator locator;

        private Dictionary<Type, object> registry;

        public static ServiceLocator Instance
        {
            get
            {
                if (locator == null)
                {
                    locator = new ServiceLocator();
                }

                return locator;
            }
        }

        private ServiceLocator()
        {
            registry = new Dictionary<Type, object>();
        }

        public void Register<T>(object serviceInstance)
        {
            registry[typeof(T)] = serviceInstance;
        }

        public T GetService<T>()
        {
            if (!registry.ContainsKey(typeof(T)))
            {
                return default(T);
            }

            return (T)registry[typeof(T)];
        }
    }
}
