using Skeleton.Abstraction;
using Skeleton.Abstraction.Reflection;
using Skeleton.Common;
using Skeleton.Common.Reflection;
using System;

namespace Skeleton.Core.Domain
{
    public sealed class EntityMapper<TEntity, TIdentity> :
        HideObjectMethods,
        IEntityMapper<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IMetadata _typeAccessor;

        public EntityMapper()
        {
            _typeAccessor = typeof(TEntity).GetMetadata();
        }

        public IMetadata TypeAccessor
        {
            get { return _typeAccessor; }
        }

        public TDto Map<TDto>(TEntity entity) where TDto : class
        {
            var accessorDto = typeof(TDto).GetMetadata();
            var instanceDto = accessorDto.CreateInstance<TDto>();

            foreach (var entityProperty in _typeAccessor.GetDeclaredOnlyProperties())
                foreach (var dtoProperty in accessorDto.GetDeclaredOnlyProperties())
                {
                    if (entityProperty.Name.EquivalentTo(dtoProperty.Name))
                        dtoProperty.SetValue(instanceDto, entityProperty.GetValue(entity));
                }

            return instanceDto;
        }

        public TEntity Reverse<TDto>(int id, TDto dto) where TDto : class
        {
            var accessorDto = typeof(TDto).GetMetadata();
            var instanceEntity = _typeAccessor.CreateInstance<TEntity>();

            foreach (var dtoProperty in accessorDto.GetDeclaredOnlyProperties())
                foreach (var entityProperty in _typeAccessor.GetDeclaredOnlyProperties())
                {
                    if (entityProperty.Name.EquivalentTo(dtoProperty.Name))
                        if (id > 0)
                            instanceEntity.IdAccessor.SetValue(instanceEntity, id);
                        else
                            entityProperty.SetValue(instanceEntity, dtoProperty.GetValue(dto));
                }

            return instanceEntity;
        }

        public TEntity Reverse<TDto>(TDto dto) where TDto : class
        {
            return Reverse<TDto>(0, dto);
        }

        //[DebuggerTypeProxy]
        //public string Stringify(TEntity entity)
        //{
        //    var builder = new StringBuilder();

        //    foreach (var property in _typeAccessor.GetDeclaredOnlyProperties())
        //        builder.AppendLine("{0} : {1}".FormatWith(
        //            property.Name, property.GetValue(entity)));

        //    return builder.ToString();
        //}
    }
}
