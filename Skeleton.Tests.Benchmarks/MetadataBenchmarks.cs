using NUnit.Framework;
using Skeleton.Abstraction.Reflection;
using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Tests.Common;
using System;
using System.Reflection;

namespace Skeleton.Tests.Benchmarks
{
    [TestFixture]
    public class MetadataBenchmarks
    {
        private const int Iterations = 10000000;
        private readonly IMetadata _metadata;
        private readonly IMemberAccessor _propertyAccessor;
        private readonly PropertyInfo _propertyInfo;
        private readonly MetadataType _instance;
        private readonly IInstanceAccessor _constructorAccessor;

        public MetadataBenchmarks()
        {
            MetadataProvider = Bootstrapper.Resolver.Resolve<IMetadataProvider>();

            _instance = new MetadataType();
            _metadata = MetadataProvider.GetMetadata<MetadataType>();
            _propertyAccessor = _metadata.GetProperty("Property");
            _propertyInfo = typeof(MetadataType).GetProperty("Property");
            _constructorAccessor = _metadata.GetConstructor();
        }

        public static IMetadataProvider MetadataProvider { get; private set; }

        [Test]
        public void Metadata_RunBenchmarks()
        {
            var benchmarks = new BenchmarkCollection
            {
                {CreateInstance_Direct, "CreateInstance (Direct Call)"},
                {Property_Set_Direct, "Property SetValue (Direct Call)"},
                {Property_Get_Direct, "Property GetValue (Direct Call)"},
                {CreateInstance_Reflection, "CreateInstance (Static Reflection)"},
                {Property_SetValue_Reflection, "Property SetValue (Static Reflection)"},
                {Property_GetValue_Reflection, "Property GetValue (Static Reflection)"},
                {CreateInstance_Metadata, "CreateInstance (Metadata)"},
                {Property_Getter_Metadata, "Property Getter (Metadata)" },
                {Property_Setter_Metadata, "Property Setter (Metadata)"}
            };

            benchmarks.Run();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "instance")]
        private void CreateInstance_Direct()
        {
            Iterate(() =>
            {
                var instance = new MetadataType();
            });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "value")]
        private void Property_Get_Direct()
        {
            Iterate(() =>
            {
                var value = _instance.Property;
            });
        }

        private void Property_Set_Direct()
        {
            Iterate(i => _instance.Property = i);
        }

        private void Property_Getter_Metadata()
        {
            Iterate(() => _propertyAccessor.Getter(_instance));
        }

        private void Property_Setter_Metadata()
        {
            Iterate(i => _propertyAccessor.Setter(_instance, i));
        }

        private void Property_GetValue_Reflection()
        {
            Iterate(() => _propertyInfo.GetValue(_instance));
        }

        private void Property_SetValue_Reflection()
        {
            Iterate(i => _propertyInfo.SetValue(_instance, i));
        }

        private void CreateInstance_Metadata()
        {
            Iterate(() => _constructorAccessor.InstanceCreator(null));
        }

        private static void CreateInstance_Reflection()
        {
            Iterate(() => Activator.CreateInstance(typeof(MetadataType)));
        }

        private static void Iterate(Action action)
        {
            for (var i = 0; i <= Iterations; ++i)
            {
                action();
            }
        }

        private static void Iterate(Action<int> action)
        {
            for (var i = 0; i <= Iterations; ++i)
            {
                action(i);
            }
        }
    }
}