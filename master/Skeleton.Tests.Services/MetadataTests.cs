using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction;
using Skeleton.Common;
using Skeleton.Tests.Infrastructure;
using System;

namespace Skeleton.Tests
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
        public void Should_CreateInstance_Of_Type()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var customer = metadata.CreateInstance<MetadataType>();

            Assert.IsNotNull(customer);
            Assert.IsInstanceOfType(customer, typeof(MetadataType));
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
        public void Should_GetProperty()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty("Property");

            Assert.IsNotNull(property);
            Assert.IsInstanceOfType(property, typeof(IMemberAccessor));

            var instance = metadata.CreateInstance<MetadataType>();
            property.SetValue(instance, 1);
            var value = property.GetValue(instance);

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

            property.SetValue(null, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_SetProperty_ThrowOnNullValue()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty("Property");
            var instance = metadata.CreateInstance<MetadataType>();

            Assert.IsNotNull(property);
            Assert.IsInstanceOfType(property, typeof(IMemberAccessor));
            Assert.IsNotNull(instance);

            property.SetValue(instance, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_GetProperty_ThrowOnNullInstance()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty("Property");

            Assert.IsNotNull(property);
            Assert.IsInstanceOfType(property, typeof(IMemberAccessor));

            property.GetValue(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Should_WriteOnlyProperty_ThrowInvalidOperationOnGetValue()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty("WriteOnlyProperty");
            var instance = metadata.CreateInstance<MetadataType>();

            Assert.IsNotNull(property);
            Assert.IsInstanceOfType(property, typeof(IMemberAccessor));
            Assert.IsNotNull(instance);

            property.GetValue(instance);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Should_GetReadOnlyProperty_ThrowInvalidOperationOnSetValue()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var property = metadata.GetProperty("ReadOnlyProperty");
            var instance = metadata.CreateInstance<MetadataType>();

            Assert.IsNotNull(property);
            Assert.IsInstanceOfType(property, typeof(IMemberAccessor));
            Assert.IsNotNull(instance);

            property.SetValue(instance, 1);
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

            var instance = metadata.CreateInstance<MetadataType>();
            field.SetValue(instance, 1);
            var value = field.GetValue(instance);

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
            var instance = metadata.CreateInstance<MetadataType>();
            var field = metadata.GetPrivateField("_field");
            var property = metadata.GetProperty("Property");
            var method = metadata.GetMethod("Method");

            field.SetValue(instance, 1);
            property.SetValue(instance, 1);

            var result = method.Invoke(instance);

            Assert.IsTrue((int)result == 2);
            Assert.IsNotNull(method.MethodInfo);
            Assert.IsTrue(method.Name == "Method");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_GetMethod_ThrowOnNullInstance()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var method = metadata.GetMethod("Method");

            Assert.IsNotNull(method);
            Assert.IsTrue(method.Name == "Method");
            
            method.Invoke(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_SetField_ThrowOnNullInstance()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var field = metadata.GetField("Field");

            Assert.IsNotNull(field);
            Assert.IsInstanceOfType(field, typeof(IMemberAccessor));

            field.SetValue(null, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_SetField_ThrowOnNullValue()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var field = metadata.GetField("Field");
            var instance = metadata.CreateInstance<MetadataType>();

            Assert.IsNotNull(field);
            Assert.IsInstanceOfType(field, typeof(IMemberAccessor));
            Assert.IsNotNull(instance);

            field.SetValue(instance, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_GetField_ThrowOnNullInstance()
        {
            var metadata = MetadataProvider.GetMetadata<MetadataType>();
            var field = metadata.GetField("Field");

            Assert.IsNotNull(field);
            Assert.IsInstanceOfType(field, typeof(IMemberAccessor));

            field.GetValue(null);
        }
    }
}