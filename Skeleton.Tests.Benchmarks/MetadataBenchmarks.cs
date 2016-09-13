using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests.Benchmarks
{
    [TestClass]
    public class MetadataBenchmarks : MetadataTestsBase
    {
        private const int Iterations = 2000000;

        [TestMethod]
        public void RunBenchmark()
        {
            var benchmarks = new Benchmarks
            {
                {CreateInstance_Metadata, "CreateInstance (Metadata)"},
                {CreateInstance_Reflection, "CreateInstance (Static Reflection)"},
                {SetPropertyValue_Metadata, "SetPropertyValue (Metadata)"},
                {SetPropertyValue_Reflection, "SetPropertyValue (Static Reflection)"},
                {GetPropertyValue_Metadata, "GetPropertyValue (Metadata)"},
                {GetPropertyValue_Reflection, "GetPropertyValue (Static Reflection)"}
            };

            benchmarks.Run();
        }

        private static void SetPropertyValue_Metadata()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var properties = metadata.GetProperties().ToArray();

            for (int i = 0; i <= Iterations; ++i)
            {
                var property = properties[0];
                var instance = metadata.CreateInstance<MetadataType>();
                property.SetValue(instance, i);
            }
        }

        private static void GetPropertyValue_Metadata()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var properties = metadata.GetProperties().ToArray();

            for (int i = 0; i <= Iterations; ++i)
            {
                var property = properties[0];
                var instance = metadata.CreateInstance<MetadataType>();
                var value = property.GetValue(instance);
            }
        }

        private static void SetPropertyValue_Reflection()
        {
            for (int i = 0; i <= Iterations; ++i)
            {
                var property = typeof(MetadataType).GetProperty("Property");
                var instance = Activator.CreateInstance(typeof(MetadataType));
                property.SetValue(instance, i);
            }
        }

        private static void GetPropertyValue_Reflection()
        {
            for (int i = 0; i <= Iterations; ++i)
            {
                var property = typeof(MetadataType).GetProperty("Property");
                var instance = Activator.CreateInstance(typeof(MetadataType));
                var value = property.GetValue(instance);
            }
        }

        private static void CreateInstance_Metadata()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();

            for (int i = 0; i <= Iterations; ++i)
            {
                var instance = metadata.CreateInstance<MetadataType>();
            }
        }

        private static void CreateInstance_Reflection()
        {
            for (int i = 0; i <= Iterations; ++i)
            {
                var instance = Activator.CreateInstance(typeof(MetadataType));
            }
        }
    }
}