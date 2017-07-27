using NUnit.Framework;
using Skeleton.Abstraction.Reflection;
using Skeleton.Core;
using Skeleton.Core.Reflection;
using Skeleton.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton.Tests.Core
{
    [TestFixture]
    public class MetadataTests : CoreTestsBase
    {
        private IEnumerable<Type> MatchingInCache(IMetadata metadata)
        {
            var matchingInCache = MetadataProvider
                .Types
                .Where(t => t == metadata.Type);

            return matchingInCache.ToList();
        }

        [Test]
        public void Metadata_Can_Cache_Type()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var matchingInCache = MatchingInCache(metadata);

            Assert.IsNotNull(matchingInCache);
        }

        [Test]
        public void Metadata_Can_Cache_Type_And_Remove_It()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var matchingInCache = MatchingInCache(metadata);

            Assert.IsNotNull(matchingInCache);

            MetadataProvider.RemoveMetadata(typeof(MetadataType));

            matchingInCache = MatchingInCache(metadata);

            Assert.IsTrue(!matchingInCache.Any());
        }

        [Test]
        public void Metadata_CreateInstance_DefaultCtor()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var instance = metadata.GetConstructor().InstanceCreator(null) as MetadataType;

            Assert.IsNotNull(instance);
            Assert.IsInstanceOf(typeof(MetadataType), instance);
        }

        [Test]
        public void Metadata_CreateInstance_CtorWithParameters()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var instance = metadata.GetConstructor(new Type[] { typeof(int) })
                .InstanceCreator(new object[] { 3 }) as MetadataType;
            var field = metadata.GetField("Field");

            Assert.IsNotNull(instance);
            Assert.IsInstanceOf(typeof(MetadataType), instance);
            Assert.IsTrue((int)field.Getter(instance) == 3);
        }

        [Test]
        public void Metadata_GetProperties()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var properties = metadata.GetProperties().ToList();

            Assert.IsTrue(properties.IsNotNullOrEmpty());
            Assert.IsInstanceOf(typeof(IMemberAccessor), properties.First());
        }

        [Test]
        public void Metadata_GetFields()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var fields = metadata.GetFields().ToList();

            Assert.IsTrue(fields.IsNotNullOrEmpty());
            Assert.IsInstanceOf(typeof(IMemberAccessor), fields.First());
        }

        [Test]
        public void Metadata_GetDeclaredOnlyFields()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var fields = metadata.GetDeclaredOnlyFields().ToList();

            Assert.IsTrue(fields.IsNotNullOrEmpty());
            Assert.IsInstanceOf(typeof(IMemberAccessor), fields.First());
        }

        [Test]
        public void Metadata_GetProperty()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty("Property");

            Assert.IsNotNull(property);
            Assert.IsInstanceOf(typeof(IMemberAccessor), property);

            var instance = metadata.GetConstructor().InstanceCreator(null);
            property.Setter(instance, 1);
            var value = property.Getter(instance);

            Assert.IsTrue((int)value == 1);
            Assert.IsTrue(property.HasGetter);
            Assert.IsTrue(property.HasSetter);
            Assert.IsNotNull(property.MemberInfo);
            Assert.IsInstanceOf(property.MemberType, value);
            Assert.IsTrue(property.Name == "Property");
        }

        [Test]
        public void Metadata_SetProperty_ThrowOnNullInstance()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty("Property");

            Assert.IsNotNull(property);
            Assert.IsInstanceOf(typeof(IMemberAccessor), property);
            Assert.Catch(typeof(NullReferenceException), () => property.Setter(null, 1));
        }

        [Test]
        public void Metadata_SetProperty_ThrowOnNullValue()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty("Property");
            var instance = metadata.GetConstructor().InstanceCreator(null);

            Assert.IsNotNull(property);
            Assert.IsInstanceOf(typeof(IMemberAccessor), property);
            Assert.IsNotNull(instance);
            Assert.Catch(typeof(NullReferenceException), () => property.Setter(instance, null));
        }

        [Test]
        public void Metadata_GetProperty_ThrowOnNullInstance()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty("Property");

            Assert.IsNotNull(property);
            Assert.IsInstanceOf(typeof(IMemberAccessor), property);
            Assert.Catch(typeof(NullReferenceException), () => property.Getter(null));
        }

        [Test]
        public void Metadata_GetProperty_ByExpression()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty<MetadataType>(p => p.Property);

            Assert.IsNotNull(property);
            Assert.IsInstanceOf(typeof(IMemberAccessor), property);
        }

        [Test]
        public void Metadata_GetProperty_ByExpression_ThrowOnNullPropertyInfo()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();

            Assert.Catch(typeof(ArgumentException), () => metadata.GetProperty<MetadataType>(p => p.Field));
        }

        [Test]
        public void Metadata_GetMethod_WithParameters()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var instance = metadata.GetConstructor().InstanceCreator(null);
            var property = metadata.GetProperty("Property");
            var method = metadata.GetMethod("Method", new[] { typeof(int) });

            method.Invoker(instance, new object[] { 3 });

            Assert.IsNotNull(method.MethodInfo);
            Assert.IsTrue(method.Name == "Method");
            Assert.IsTrue((int)property.Getter(instance) == 3);
        }

        [Test]
        public void Metadata_GetMethod_ThrowOnNullInstance()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var method = metadata.GetMethod("Method");

            Assert.IsNotNull(method);
            Assert.IsTrue(method.Name == "Method");
            Assert.Catch(typeof(NullReferenceException), () => method.Invoker(null, null));
        }

        [Test]
        public void Metadata_CreateMethod_ThrowOnNotFoundMethodName()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();

            Assert.Catch(typeof(InvalidOperationException), () => metadata.GetMethod("InexistantMethod"));
        }

        [Test]
        public void Metadata_SetField_ThrowOnNullInstance()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var field = metadata.GetField("Field");

            Assert.IsNotNull(field);
            Assert.IsInstanceOf(typeof(IMemberAccessor), field);
            Assert.Catch(typeof(NullReferenceException), () => field.Setter(null, 1));
        }

        [Test]
        public void Metadata_SetField_ThrowOnNullValue()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var field = metadata.GetField("Field");
            var instance = metadata.GetConstructor().InstanceCreator(null);

            Assert.IsNotNull(field);
            Assert.IsInstanceOf(typeof(IMemberAccessor), field);
            Assert.IsNotNull(instance);
            Assert.Catch(typeof(NullReferenceException), () => field.Setter(instance, null));
        }

        [Test]
        public void Metadata_GetField_ThrowOnNullInstance()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var field = metadata.GetField("Field");

            Assert.IsNotNull(field);
            Assert.IsInstanceOf(typeof(IMemberAccessor), field);
            Assert.Catch(typeof(NullReferenceException), () => field.Getter(null));
        }

        [Test]
        public void Metadata_MemberAccessorBase_NotEquals()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var field = metadata.GetField("Field");
            var property = metadata.GetProperty("Property");

            Assert.IsNotNull(field);
            Assert.IsNotNull(property);
            Assert.IsInstanceOf(typeof(IMemberAccessor), field);
            Assert.IsInstanceOf(typeof(IMemberAccessor), property);
            EqualityTests.TestUnequalObjects(field as MemberAccessorBase, property as MemberAccessorBase);
        }

        [Test]
        public void Metadata_MemberAccessorBase_Equals()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property1 = metadata.GetProperty("Property");
            var property2 = metadata.GetProperty("Property");

            Assert.IsNotNull(property1);
            Assert.IsNotNull(property2);
            Assert.IsInstanceOf(typeof(IMemberAccessor), property1);
            Assert.IsInstanceOf(typeof(IMemberAccessor), property2);
            EqualityTests.TestEqualObjects(property1 as MemberAccessorBase, property2 as MemberAccessorBase);
        }

        [Test]
        public void Metadata_MemberAccessorBase_NotNull()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty("Property");

            Assert.IsNotNull(property);
            Assert.IsInstanceOf(typeof(IMemberAccessor), property);
            EqualityTests.TestAgainstNull(property as MemberAccessorBase);
        }
    }
}