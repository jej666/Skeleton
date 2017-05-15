﻿using Microsoft.Practices.Unity;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Reflection;
using Skeleton.Core.Caching;
using Skeleton.Core.Reflection;
using Skeleton.Infrastructure.DependencyInjection.LoggerExtension;
using Skeleton.Infrastructure.Logging;

namespace Skeleton.Infrastructure.DependencyInjection
{
    internal sealed class CommonModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            LoggerConfiguration.Configure();

            Container.AddExtension(new LoggerConstructorInjectionExtension())
                .RegisterInstance<ILoggerFactory>(new LoggerFactory())
                .RegisterType<ICacheProvider, MemoryCacheProvider>()
                .RegisterType<IAsyncCacheProvider, AsyncMemoryCacheProvider>()
                .RegisterType<IMetadataProvider, MetadataProvider>();
        }
    }
}