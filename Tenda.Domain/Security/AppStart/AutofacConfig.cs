using Autofac;
using Autofac.Core.NonPublicProperty;
using System;
using System.Collections.Generic;
using System.Reflection;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Shared.Commons;

namespace Tenda.Domain.Security.AppStart
{
    public static class AutofacConfig
    {
        public static ContainerBuilder Register(ContainerBuilder builder)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();

            List<Type> repositories = ReflectionHelper.ClassInNamespace(Layers.SecurityRepository, currentAssembly);
            builder.RegisterTypes(repositories.ToArray()).PropertiesAutowired().AutoWireNonPublicProperties().InstancePerRequest();

            List<Type> services = ReflectionHelper.ClassInNamespace(Layers.SecurityServices, currentAssembly);
            builder.RegisterTypes(services.ToArray()).PropertiesAutowired().AutoWireNonPublicProperties().InstancePerRequest();

            builder.RegisterType<UsuarioPortalService>().PropertiesAutowired().AutoWireNonPublicProperties().InstancePerRequest();

            return builder;
        }
    }
}
