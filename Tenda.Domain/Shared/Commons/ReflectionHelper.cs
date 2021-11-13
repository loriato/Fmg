using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tenda.Domain.Shared.Commons
{
    public static class ReflectionHelper
    {
        [Obsolete("Método vazio")]
        private static List<Assembly> SolutionAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();
            //FIXME: How to solve this?
            return assemblies;
        }

        public static List<Type> ClassInNamespace(string targetNamespace, params Assembly[] assemblies)
        {
            List<Type> entityTypes = new List<Type>();

            foreach (Assembly assembly in assemblies)
            {
                entityTypes.AddRange(assembly.GetTypes()
                    .Where(reg => reg.IsClass)
                    .Where(reg => reg.Namespace == targetNamespace));
            }

            return entityTypes;
        }

        public static List<Type> ClassInNamespace(string targetNamespace)
        {
            List<Type> entityTypes = new List<Type>();

            var assemblies = SolutionAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                entityTypes.AddRange(assembly.GetTypes()
                    .Where(reg => reg.IsClass)
                    .Where(reg => reg.Namespace == targetNamespace));
            }

            return entityTypes;
        }

        public static List<Type> EnumInNamespace(string targetNamespace)
        {
            List<Type> entityTypes = new List<Type>();

            var assemblies = SolutionAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                entityTypes.AddRange(assembly.GetTypes()
                    .Where(reg => reg.IsEnum)
                    .Where(reg => reg.Namespace == targetNamespace));
            }

            return entityTypes;
        }

        public static List<Type> EnumInNamespace(string targetNamespace, params Assembly[] assemblies)
        {
            List<Type> entityTypes = new List<Type>();

            foreach (Assembly assembly in assemblies)
            {
                entityTypes.AddRange(assembly.GetTypes()
                    .Where(reg => reg.IsEnum)
                    .Where(reg => reg.Namespace == targetNamespace));
            }

            return entityTypes;
        }

        public static List<Type> SubclassOf(Type type)
        {
            List<Type> entityTypes = new List<Type>();

            var assemblies = SolutionAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                entityTypes.AddRange(assembly.GetTypes().Where(reg => reg.IsSubclassOf(type)));
            }

            entityTypes.Sort((x, y) => x.Name.CompareTo(y.Name));

            return entityTypes;
        }

        public static List<Type> SubclassOfBaseEntity()
        {
            return SubclassOf(typeof(BaseEntity));
        }

        public static List<Type> SubclassOfBaseEntityExceptView()
        {
            var entityTypes = SubclassOfBaseEntity();
            entityTypes.RemoveAll(reg => reg.Name.StartsWith("View"));
            return entityTypes;
        }


        public static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}
