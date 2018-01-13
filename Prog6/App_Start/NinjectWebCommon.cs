using Ninject.Web.Common.WebHost;
using Prog6.Controllers;
using Prog6.Respositories;
using Prog6.Respositories.Interfaces;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Prog6.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Prog6.App_Start.NinjectWebCommon), "Stop")]

namespace Prog6.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                kernel.Bind<ITamagotchiRepository>().To<EntityTamagotchiRepository>();
                kernel.Bind<IHotelkamerRepository>().To<EntityHotelkamerRepository>();
                kernel.Bind<IHotelkamerEffectRepository>().To<EntityHotelkamerEffectRepository>();
                kernel.Bind<IHotelkamerTypeRepository>().To<EntityHotelkamerTypeRepository>();
                kernel.Bind<IBoekingRepository>().To<EntityBoekingRepository>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }        
    }
}