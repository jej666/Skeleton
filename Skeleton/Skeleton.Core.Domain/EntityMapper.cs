using Skeleton.Abstraction;
using System.Text;
using System;

namespace Skeleton.Core.Domain
{
    public class EntityMapper<TEntity, TIdentity> :
        IEntityMapper<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly ITypeAccessorCache _typeAccessorCache;
        private readonly ITypeAccessor _typeAccessor;

        public EntityMapper(ITypeAccessorCache typeAccessorCache)
        {
            _typeAccessorCache = typeAccessorCache;
            _typeAccessor = typeAccessorCache.Get<TEntity>();
        }

        public ITypeAccessor TypeAccessor
        {
            get { return _typeAccessor; }
        }

        public Dto Map<Dto>(TEntity entity) where Dto : class
        {
            var accessorDto = _typeAccessorCache.Get<Dto>();
            var instanceDto = accessorDto.CreateInstance<Dto>();

            foreach (var entityProp in _typeAccessor.GetDeclaredOnlyProperties())
                foreach (var dtoProp in accessorDto.GetDeclaredOnlyProperties())
                {
                    if (entityProp.Name.EquivalentTo(dtoProp.Name))
                        dtoProp.SetValue(instanceDto, entityProp.GetValue(entity));
                }

            return instanceDto;
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
