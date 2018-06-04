using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using Ninject;
using Sports_Store.Domain.Abstract;
using Sports_Store.Domain.Concrete;
using Sports_Store.Domain.Entities;

namespace ShoppingSiteASP.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
           return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            //Mock<IProductRepository> mockProducts = new Mock<IProductRepository>();
            //mockProducts.Setup(mp => mp.Products).Returns(
            //    new List<Product> { new Product() { Name="Football", Price = 25 },
            //    new Product() { Name="Surf Board", Price = 179 },
            //    new Product() { Name="Running Shoes", Price = 95 }}
            //    );

            //kernel.Bind<IProductRepository>().ToConstant(mockProducts.Object);

            kernel.Bind<IProductRepository>().To<EFProductRepository>();

            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);
        }
    }
}