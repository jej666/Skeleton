using Microsoft.Practices.Unity;
using Skeleton.Infrastructure.DependencyInjection;
using System;

namespace Skeleton.Web.Server
{
    public class WebAppHost : AppHost
    {
        private static readonly Lazy<IUnityContainer> Container =
           new Lazy<IUnityContainer>(() => new UnityContainer());

        public IUnityContainer UnityContainer => Container.Value;

        public WebAppHost() : base(Container.Value)
        {
        }
    }
}
