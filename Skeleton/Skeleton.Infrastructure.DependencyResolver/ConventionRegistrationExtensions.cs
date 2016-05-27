using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace Skeleton.Infrastructure.DependencyResolver
{
    public static class ConventionRegistrationExtensions
    {
        private static readonly Dictionary<Type, HashSet<Type>> InternalTypeMapping =
            new Dictionary<Type, HashSet<Type>>();

        public static void RegisterByConvention(this IUnityContainer container,
            IEnumerable<Assembly> assembliesToLoad)
        {
            foreach (var type in GetClassesFromAssemblies(assembliesToLoad))
            {
                var interfacesToRegister = GetInterfacesToRegister(type);
                AddToInternalTypeMapping(type, interfacesToRegister);
            }

            container.RegisterConventions();
        }

        private static void RegisterConventions(this IUnityContainer container)
        {
            foreach (var typeMapping in InternalTypeMapping)
            {
                if (typeMapping.Value.Count == 1)
                {
                    var type = typeMapping.Value.First();
                    container.RegisterType(typeMapping.Key, type);
                }
                else
                {
                    foreach (var type in typeMapping.Value)
                    {
                        container.RegisterType(typeMapping.Key, type, type.Name);
                    }
                }
            }
        }

        private static void AddToInternalTypeMapping(Type type, IEnumerable<Type> interfacesOnType)
        {
            foreach (var interfaceOnType in interfacesOnType)
            {
                if (!InternalTypeMapping.ContainsKey(interfaceOnType))
                {
                    InternalTypeMapping[interfaceOnType] = new HashSet<Type>();
                }

                InternalTypeMapping[interfaceOnType].Add(type);
            }
        }

        private static IEnumerable<Type> GetInterfacesToRegister(Type type)
        {
            var allInterfacesOnType = type.GetInterfaces()
                .Select(i => i.IsGenericType ? i.GetGenericTypeDefinition() : i)
                .ToList();

            return allInterfacesOnType.Except(allInterfacesOnType.SelectMany(i => i.GetInterfaces())).ToList();
        }

        private static IEnumerable<Type> GetClassesFromAssemblies(IEnumerable<Assembly> assemblies)
        {  
            return AllClasses
                .FromAssemblies(assemblies)
                .Where(
                    n =>
                        n.Namespace != null
                        && n.Namespace.StartsWith(n.Namespace, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}