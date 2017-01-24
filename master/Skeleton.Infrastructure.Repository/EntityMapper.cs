using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Reflection;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class EntityMapper<TEntity, TDto> :
            HideObjectMethods,
            IEntityMapper<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        private readonly IInstanceAccessor _dtoInstanceAccessor;
        private readonly IMetadata _dtoMetadata;
        private readonly IEnumerable<IMemberAccessor> _dtoProperties;
        private readonly IInstanceAccessor _entityInstanceAccessor;
        private readonly IMetadata _entityMetadata;
        private readonly IEnumerable<IMemberAccessor> _entityProperties;
        private readonly ILogger _logger;

        public EntityMapper(
            ILogger logger,
            IMetadataProvider metadataProvider)
        {
            logger.ThrowIfNull(() => logger);
            metadataProvider.ThrowIfNull(() => metadataProvider);

            _logger = logger;
            _dtoMetadata = metadataProvider.GetMetadata<TDto>();
            _dtoInstanceAccessor = _dtoMetadata.GetConstructor();
            _dtoProperties = _dtoMetadata.GetDeclaredOnlyProperties();

            _entityMetadata = metadataProvider.GetMetadata<TEntity>();
            _entityInstanceAccessor = _entityMetadata.GetConstructor();
            _entityProperties = _entityMetadata.GetDeclaredOnlyProperties();
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
                var instanceDto = _dtoInstanceAccessor.InstanceCreator(null) as TDto;

                foreach (var entityProperty in _entityProperties)
                    foreach (var dtoProperty in _dtoProperties)
                        if (entityProperty.Name.EquivalentTo(dtoProperty.Name))
                            dtoProperty.Setter(instanceDto, entityProperty.Getter(entity));

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
                var instanceEntity = _entityInstanceAccessor.InstanceCreator(null) as TEntity;

                foreach (var dtoProperty in _dtoProperties)
                    foreach (var entityProperty in _entityProperties)
                        if (entityProperty.Name.EquivalentTo(dtoProperty.Name))
                            entityProperty.Setter(instanceEntity, dtoProperty.Getter(dto));

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