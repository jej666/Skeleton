using System;
using Skeleton.Core.Service;
using Skeleton.Shared.Abstraction;
using Skeleton.Shared.Abstraction.Reflection;

namespace Skeleton.Infrastructure.Service
{
    public sealed class EntityMapper<TEntity, TIdentity, TDto> :
        HideObjectMethods,
        IEntityMapper<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        private readonly ILogger _logger;
        private readonly IMetadata _entityMetadata;
        private readonly IMetadata _dtoMetadata;

        public EntityMapper(
            ILogger logger,
            IMetadataProvider metadataProvider)
        {
            _logger = logger;
            _dtoMetadata = metadataProvider.GetMetadata<TDto>();
            _entityMetadata = metadataProvider.GetMetadata<TEntity>();
        }

        public TDto Map(TEntity entity)
        {
            return HandleException(() =>
            {
                var instanceDto = _dtoMetadata.CreateInstance<TDto>();

                foreach (var entityProperty in _entityMetadata.GetDeclaredOnlyProperties())
                    foreach (var dtoProperty in _dtoMetadata.GetDeclaredOnlyProperties())
                    {
                        if (entityProperty.Name.EquivalentTo(dtoProperty.Name))
                            dtoProperty.SetValue(instanceDto, entityProperty.GetValue(entity));
                    }

                return instanceDto;
            });
        }

        public TEntity Map(TIdentity id, TDto dto)
        {
            return HandleException(() =>
            {
                var instanceEntity = _entityMetadata.CreateInstance<TEntity>();

                foreach (var dtoProperty in _dtoMetadata.GetDeclaredOnlyProperties())
                    foreach (var entityProperty in _entityMetadata.GetDeclaredOnlyProperties())
                    {
                        if (!entityProperty.Name.EquivalentTo(dtoProperty.Name))
                            continue;

                        if (!id.Equals(default(TIdentity)))
                            instanceEntity.IdAccessor.SetValue(instanceEntity, id);
                        else
                            entityProperty.SetValue(instanceEntity, dtoProperty.GetValue(dto));
                    }

                return instanceEntity;
            });
        }

        public TEntity Map(TDto dto)
        {
            return Map(default(TIdentity), dto);
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