namespace Skeleton.Common
{
    using System;
    using System.Collections.Generic;

    public interface IContainer : IHideObjectMethods
    {
        object Resolve(Type serviceType);

        object Resolve(Type serviceType, string key);

        TService Resolve<TService>() where TService : class;

        TService Resolve<TService>(string key) where TService : class;

        IEnumerable<object> ResolveAll(Type serviceType);

        IEnumerable<TService> ResolveAll<TService>() where TService : class;
    }
}