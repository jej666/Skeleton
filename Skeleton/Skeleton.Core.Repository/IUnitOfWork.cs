using System;
using System.Collections.Generic;
using Skeleton.Common;

namespace Skeleton.Core.Repository
{
    public interface IUnitOfWork :
        IDisposable,
        IHideObjectMethods
    {
        IEnumerable<IEntityRepository> ResolveAll();

        IEntityRepository Resolve<TType>() where TType : class,IEntityRepository;
        IEntityRepository Resolve(Type type);

        IUnitOfWork Register<TType>(IEntityRepository repository) where TType : class, IEntityRepository;
        IUnitOfWork Register(Type type, IEntityRepository repository);
    }
}