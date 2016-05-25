using System;
using System.Collections.Generic;
using Skeleton.Common;
using Skeleton.Core.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public abstract class UnitOfWork :
        DisposableBase,
        IUnitOfWork
    {
        private readonly Dictionary<Type, IEntityRepository> _repositories =
            new Dictionary<Type, IEntityRepository>();

        public IEnumerable<IEntityRepository> ResolveAll()
        {
            return _repositories.Values;
        }

        public IEntityRepository Resolve<TType>() where TType : class, IEntityRepository
        {
            return Resolve(typeof(TType));
        }

        public IEntityRepository Resolve(Type type)
        {
            return _repositories.ContainsKey(type)
                ? _repositories[type]
                : null;
        }

        public IUnitOfWork Register<TType>(IEntityRepository repository) where TType : class, IEntityRepository
        {
           return Register(typeof(TType), repository);
        }

        public IUnitOfWork Register(Type type, IEntityRepository repository)
        {
             if (!_repositories.ContainsKey(type))
                 _repositories.Add(type,repository);

            return this;
        }
    }
}