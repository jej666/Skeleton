using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction.Reflection;
using Skeleton.Tests.Core;
using System;
using System.Reflection;

namespace Skeleton.Tests.Benchmarks
{
    [TestClass]
    public class MetadataBenchmarks : MetadataTestsBase
    {
        private const int Iterations = 10000000;
        private readonly IMetadata _metadata ;
        private readonly IMemberAccessor _propertyAccessor;
        private readonly PropertyInfo _propertyInfo;
        private readonly MetadataType _instance;
        private readonly IInstanceAccessor _constructorAccessor;

        public MetadataBenchmarks()
        {
            _instance = new MetadataType();
            _metadata = MetadataProvider.GetMetadata<MetadataType>();
            _propertyAccessor = _metadata.GetProperty("Property");
            _propertyInfo = typeof(MetadataType).GetProperty("Property");
            _constructorAccessor = _metadata.GetConstructor();
        }

        [TestMethod]
        public void RunBenchmark()
        {
            var benchmarks = new Benchmarks
            {
                {CreateInstance_Reflection, "CreateInstance (Static Reflection)"},
                {Property_SetValue_Reflection, "Property SetValue (Static Reflection)"},
                {Property_GetValue_Reflection, "Property GetValue (Static Reflection)"},
                {CreateInstance_Metadata, "CreateInstance (Metadata)"},
                {Property_Getter_Metadata, "Property Getter (Metadata)" },
                {Property_Setter_Metadata, "Property Setter (Metadata)"}
            };

            benchmarks.Run();
        }

        private  void Property_Getter_Metadata()
        {
            for (var i = 0; i <= Iterations; ++i)
            {
                _propertyAccessor.Getter(_instance);
            }
        }

        private void Property_Setter_Metadata()
        {
            for (var i = 0; i <= Iterations; ++i)
            {
                _propertyAccessor.Setter(_instance, i);
            }
        }

        private void Property_GetValue_Reflection()
        {
            for (var i = 0; i <= Iterations; ++i)
            {
                _propertyInfo.GetValue(_instance);
            }
        }

        private void Property_SetValue_Reflection()
        {
            for (var i = 0; i <= Iterations; ++i)
            {
                _propertyInfo.SetValue(_instance, i);
            }
        }

        private void CreateInstance_Metadata()
        {
            for (var i = 0; i <= Iterations; ++i)
            {
                _constructorAccessor.InstanceCreator(null);
            }
        }

        private static void CreateInstance_Reflection()
        {
            for (var i = 0; i <= Iterations; ++i)
            {
                Activator.CreateInstance(typeof(MetadataType));
            }
        }
    }
}