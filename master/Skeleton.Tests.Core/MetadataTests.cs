using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction.Reflection;
using Skeleton.Common;
using System;
using System.Linq;

namespace Skeleton.Tests.Core
{
    [TestClass]
    public class MetadataTests : MetadataTestsBase
    {
        [TestMethod]
        public void Should_Cache_Type()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var matchingInCache = MatchingInCache(metadata);

            Assert.IsNotNull(matchingInCache);
        }

        [TestMethod]
        public void Should_Cache_Type_And_Remove_It()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var matchingInCache = MatchingInCache(metadata);

            Assert.IsNotNull(matchingInCache);

            MetadataProvider.RemoveMetadata(typeof(MetadataType));

            matchingInCache = MatchingInCache(metadata);

            Assert.IsTrue(!matchingInCache.Any());
        }

        [TestMethod]
        public void Should_CreateInstance_DefaultCtor()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var instance = metadata.GetConstructor().InstanceCreator(null) as MetadataType;

            Assert.IsNotNull(instance);
            Assert.IsInstanceOfType(instance, typeof(MetadataType));
        }

        [TestMethod]
        public void Should_CreateInstance_CtorWithParameters()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var instance = metadata.GetConstructor(new Type[] { typeof(int) }).InstanceCreator(new object[] { 3 }) as MetadataType;
            var field = metadata.GetField("Field");

            Assert.IsNotNull(instance);
            Assert.IsInstanceOfType(instance, typeof(MetadataType));
            Assert.IsTrue((int)field.Getter(instance) == 3);
        }

        [TestMethod]
        public void Should_GetProperties()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var properties = metadata.GetProperties().ToList();

            Assert.IsTrue(properties.IsNotNullOrEmpty());
            Assert.IsInstanceOfType(properties.First(), typeof(IMemberAccessor));
        }

        [TestMethod]
        public void Should_GetFields()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var fields = metadata.GetFields().ToList();

            Assert.IsTrue(fields.IsNotNullOrEmpty());
            Assert.IsInstanceOfType(fields.First(), typeof(IMemberAccessor));
        }

        [TestMethod]
        public void Should_GetProperty()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty("Property");

            Assert.IsNotNull(property);
            Assert.IsInstanceOfType(property, typeof(IMemberAccessor));

            var instance = metadata.GetConstructor().InstanceCreator(null);
            property.Setter(instance, 1);
            var value = property.Getter(instance);

            Assert.IsTrue((int)value == 1);
            Assert.IsTrue(property.HasGetter);
            Assert.IsTrue(property.HasSetter);
            Assert.IsNotNull(property.MemberInfo);
            Assert.IsInstanceOfType(value, property.MemberType);
            Assert.IsTrue(property.Name == "Property");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_SetProperty_ThrowOnNullInstance()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty("Property");

            Assert.IsNotNull(property);
            Assert.IsInstanceOfType(property, typeof(IMemberAccessor));

            property.Setter(null, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_SetProperty_ThrowOnNullValue()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty("Property");
            var instance = metadata.GetConstructor().InstanceCreator(null);

            Assert.IsNotNull(property);
            Assert.IsInstanceOfType(property, typeof(IMemberAccessor));
            Assert.IsNotNull(instance);

            property.Setter(instance, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_GetProperty_ThrowOnNullInstance()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty("Property");

            Assert.IsNotNull(property);
            Assert.IsInstanceOfType(property, typeof(IMemberAccessor));

            property.Getter(null);
        }

        [TestMethod]
        public void Should_GetProperty_ByExpression()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty<MetadataType>(p => p.Property);

            Assert.IsNotNull(property);
            Assert.IsInstanceOfType(property, typeof(IMemberAccessor));
        }

        [TestMethod]
        public void Should_GetPrivateField()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var field = metadata.GetPrivateField("_field");

            Assert.IsNotNull(field);
            Assert.IsInstanceOfType(field, typeof(IMemberAccessor));

            var instance = metadata.GetConstructor().InstanceCreator(null);
            field.Setter(instance, 1);
            var value = field.Getter(instance);

            Assert.IsTrue((int)value == 1);
            Assert.IsTrue(field.HasGetter);
            Assert.IsTrue(field.HasSetter);
            Assert.IsNotNull(field.MemberInfo);
            Assert.IsInstanceOfType(value, field.MemberType);
            Assert.IsTrue(field.Name == "_field");
        }

        [TestMethod]
        public void Should_GetMethod()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var instance = metadata.GetConstructor().InstanceCreator(null);
            var field = metadata.GetPrivateField("_field");
            var property = metadata.GetProperty("Property");
            var method = metadata.GetMethod("Method");

            field.Setter(instance, 1);
            property.Setter(instance, 1);

            var result = method.Invoker(instance, null);

            Assert.IsTrue((int)result == 2);
            Assert.IsNotNull(method.MethodInfo);
            Assert.IsTrue(method.Name == "Method");
        }

        [TestMethod]
        public void Should_GetMethod_WithParameters()
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

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_GetMethod_ThrowOnNullInstance()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var method = metadata.GetMethod("Method");

            Assert.IsNotNull(method);
            Assert.IsTrue(method.Name == "Method");

            method.Invoker(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Should_CreateMethod_ThrowOnNotFoundMethodName()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            metadata.GetMethod("InexistantMethod");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_SetField_ThrowOnNullInstance()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var field = metadata.GetField("Field");

            Assert.IsNotNull(field);
            Assert.IsInstanceOfType(field, typeof(IMemberAccessor));

            field.Setter(null, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_SetField_ThrowOnNullValue()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var field = metadata.GetField("Field");
            var instance = metadata.GetConstructor().InstanceCreator(null);

            Assert.IsNotNull(field);
            Assert.IsInstanceOfType(field, typeof(IMemberAccessor));
            Assert.IsNotNull(instance);

            field.Setter(instance, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_GetField_ThrowOnNullInstance()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var field = metadata.GetField("Field");

            Assert.IsNotNull(field);
            Assert.IsInstanceOfType(field, typeof(IMemberAccessor));

            field.Getter(null);
        }
    }
}