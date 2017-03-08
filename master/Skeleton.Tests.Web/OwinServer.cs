﻿using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Tests.Common;
using Skeleton.Tests.Web.Mock;
using Skeleton.Web.Server;
using System;

namespace Skeleton.Tests.Web
{
    public sealed class OwinServer : IDisposable
    {
        private IDisposable _server;

        public void Dispose()
        {
            _server.Dispose();
        }

        public void Start()
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();
            SqlDbSeeder.SeedCustomers();

            Bootstrapper.UseDatabase(
                builder => builder.UsingConfigConnectionString("Default")
                .Build());

            _server = Startup.StartServer(new Uri(Constants.Url));
        }
    }
}