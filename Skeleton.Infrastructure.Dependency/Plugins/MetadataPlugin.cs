﻿using Skeleton.Abstraction.Dependency;
using Skeleton.Abstraction.Reflection;
using Skeleton.Common;
using Skeleton.Core.Reflection;

namespace Skeleton.Infrastructure.Dependency.Plugins
{
    public sealed class MetadataPlugin : IPlugin
    {
        public void Configure(IDependencyContainer container)
        {
            container.ThrowIfNull(nameof(container));

            container.Register.Type<IMetadataProvider, MetadataProvider>();
        }
    }
}