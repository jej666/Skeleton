using System;
using System.Collections.Generic;
using System.Linq;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class EntityMapper<TEntity, TDto> :
            HideObjectMethods,
            IEntityMapper<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        private readonly IMetadata _dtoMetadata;
        private readonly IMetadata _entityMetadata;
        private readonly ILogger _logger;

        public EntityMapper(
            ILogger logger,
            IMetadataProvider metadataProvider)
        {
            _logger = logger;
            _dtoMetadata = metadataProvider.GetMetadata<TDto>();
            _entityMetadata = metadataProvider.GetMetadata<TEntity>();
        }

        public IEnumerable<TDto> Map(IEnumerable<TEntity> entities)
        {
            entities.ThrowIfNullOrEmpty(() => entities);

            return entities.Select(Map).AsList();
        }

        public TDto Map(TEntity entity)
        {
            return HandleException(() =>
            {
                var instanceDto = _dtoMetadata.CreateInstance<TDto>();

                foreach (var entityProperty in _entityMetadata.GetDeclaredOnlyProperties())
                    foreach (var dtoProperty in _dtoMetadata.GetDeclaredOnlyProperties())
                        if (entityProperty.Name.EquivalentTo(dtoProperty.Name))
                            dtoProperty.SetValue(instanceDto, entityProperty.GetValue(entity));

                return instanceDto;
            });
        }

        public IEnumerable<TEntity> Map(IEnumerable<TDto> dtos)
        {
            dtos.ThrowIfNullOrEmpty(() => dtos);

            return dtos.Select(Map).AsList();
        }

        public TEntity Map(TDto dto)
        {
            return HandleException(() =>
            {
                var instanceEntity = _entityMetadata.CreateInstance<TEntity>();

                foreach (var dtoProperty in _dtoMetadata.GetDeclaredOnlyProperties())
                    foreach (var entityProperty in _entityMetadata.GetDeclaredOnlyProperties())
                    {
                        if (!entityProperty.Name.EquivalentTo(dtoProperty.Name))
                            continue;
                        
                            entityProperty.SetValue(instanceEntity, dtoProperty.GetValue(dto));
                      
                    }

                return instanceEntity;
            });
        }

        private T HandleException<T>(Func<T> handler)
        {
            try
            {
                return handler();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw;
            }
        }
    }
}