using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;
using Skeleton.Abstraction.Reflection;
using Skeleton.Core;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton.Infrastructure.Orm
{
    public sealed class EntityMapper<TEntity, TDto> :
            HideObjectMethodsBase,
            IEntityMapper<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        private readonly IMetadata _dtoMetadata;
        private readonly IMetadata _entityMetadata;
        private readonly IInstanceAccessor _entityInstanceAccessor;
        private readonly IInstanceAccessor _dtoInstanceAccessor;
        private readonly IEnumerable<IMemberAccessor> _entityProperties;
        private readonly IEnumerable<IMemberAccessor> _dtoProperties;

        public EntityMapper(IMetadataProvider metadataProvider)
        {
            metadataProvider.ThrowIfNull(nameof(metadataProvider));

            _dtoMetadata = metadataProvider.GetMetadata<TDto>();
            _dtoInstanceAccessor = _dtoMetadata.GetConstructor();
            _dtoProperties = _dtoMetadata.GetDeclaredOnlyProperties();

            _entityMetadata = metadataProvider.GetMetadata<TEntity>();
            _entityInstanceAccessor = _entityMetadata.GetConstructor();
            _entityProperties = _entityMetadata.GetDeclaredOnlyProperties();
        }

        public IEnumerable<TDto> Map(IEnumerable<TEntity> entities)
        {
            entities.ThrowIfNullOrEmpty(nameof(entities));

            return entities.Select(Map).AsList();
        }

        public TDto Map(TEntity entity)
        {
            var instanceDto = _dtoInstanceAccessor.InstanceCreator(null) as TDto;

            foreach (var entityProperty in _entityProperties)
                foreach (var dtoProperty in _dtoProperties)
                    if (entityProperty.Name.EquivalentTo(dtoProperty.Name))
                        dtoProperty.Setter(instanceDto, entityProperty.Getter(entity));

            return instanceDto;
        }

        public IEnumerable<TEntity> Map(IEnumerable<TDto> dtos)
        {
            dtos.ThrowIfNullOrEmpty(nameof(dtos));

            return dtos.Select(Map).AsList();
        }

        public TEntity Map(TDto dto)
        {
            var instanceEntity = _entityInstanceAccessor.InstanceCreator(null) as TEntity;

            foreach (var dtoProperty in _dtoProperties)
                foreach (var entityProperty in _entityProperties)
                    if (entityProperty.Name.EquivalentTo(dtoProperty.Name))
                        entityProperty.Setter(instanceEntity, dtoProperty.Getter(dto));

            return instanceEntity;
        }
    }
}