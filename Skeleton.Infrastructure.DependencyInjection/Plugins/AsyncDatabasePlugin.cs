﻿using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Startup;
using Skeleton.Infrastructure.Data;

namespace Skeleton.Infrastructure.DependencyInjection.Plugins
{
    public sealed class AsyncDatabasePlugin : IPlugin
    {
        public void Configure(IBootstrapper bootstrapper)
        {
            bootstrapper.RegisterType<IAsyncDatabase, AsyncDatabase>(DependencyLifetime.Scoped);
        }
    }
}