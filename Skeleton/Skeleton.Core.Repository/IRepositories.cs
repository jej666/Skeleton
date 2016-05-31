using System;
using System.Collections.Generic;
using Skeleton.Common;

namespace Skeleton.Core.Repository
{
    public interface IRepositories :
        IDisposable,
        IHideObjectMethods
    {
        IEnumerable<IEntityRepository> ResolveAll();

        IEntityRepository Resolve<TType>() where TType : class, IEntityRepository;
        IEntityRepository Resolve(Type type);

        IRepositories Register<TType>(IEntityRepository repository) where TType : class, IEntityRepository;
        IRepositories Register(Type type, IEntityRepository repository);
    }
}