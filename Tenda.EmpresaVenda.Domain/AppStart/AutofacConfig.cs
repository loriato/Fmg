using Autofac;
using Autofac.Core.NonPublicProperty;
using System;
using System.Collections.Generic;
using System.Reflection;
using Tenda.Domain.Shared.Commons;

namespace Tenda.EmpresaVenda.Domain.AppStart
{
    public static class AutofacConfig
    {
        public static ContainerBuilder Register(ContainerBuilder builder)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();

            List<Type> repositories = ReflectionHelper.ClassInNamespace(Layers.Repository, currentAssembly);
            builder.RegisterTypes(repositories.ToArray()).PropertiesAutowired().InstancePerRequest().AutoWireNonPublicProperties();

            List<Type> services = ReflectionHelper.ClassInNamespace(Layers.Services, currentAssembly);
            builder.RegisterTypes(services.ToArray()).PropertiesAutowired().InstancePerRequest().AutoWireNonPublicProperties();

            var validators = ReflectionHelper.ClassInNamespace(Layers.Validators, currentAssembly);
            builder.RegisterTypes(validators.ToArray()).PropertiesAutowired().InstancePerRequest()
                .AutoWireNonPublicProperties();

            return builder;
        }
    }
}
