﻿using Skeleton.Abstraction.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Abstraction.Orm
{
    public interface IAsyncEntityQuery<TEntity>
        where TEntity : class, IEntity<TEntity>, new()
    {
        Task<TEntity> FirstOrDefaultAsync();

        Task<TEntity> FirstOrDefaultAsync(object id);

        Task<IEnumerable<TEntity>> FindAsync();

        Task<IEnumerable<TEntity>> QueryAsync(IQuery query);
    }
}