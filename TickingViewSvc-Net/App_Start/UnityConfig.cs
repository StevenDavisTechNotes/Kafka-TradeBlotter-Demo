using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using System.Threading;
using TickingViewSvc_Net.Services;

namespace TickingViewSvc_Net
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            var cts = new CancellationTokenSource();
            var viewEngine = new ViewEngine(cts.Token); // viewEngine;

            container.RegisterInstance(typeof(IViewEngine), viewEngine);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}