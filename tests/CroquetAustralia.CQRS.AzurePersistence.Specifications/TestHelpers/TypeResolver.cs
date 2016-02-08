using System;
using System.Linq;
using System.Reflection;
using CroquetAustralia.Domain;

namespace CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers
{
    public class TypeResolver
    {
        private readonly Assembly[] _assemblies;

        public TypeResolver()
            : this(typeof(DomainCommandBus).Assembly, typeof(ArgumentOutOfRangeException).Assembly, typeof(CommandBus).Assembly)
        {
        }

        private TypeResolver(params Assembly[] assemblies)
        {
            _assemblies = assemblies.ToArray();
        }

        private Type TypeResolverResolver(Assembly assembly, string typeName, bool caseSensitive)
        {
            if (assembly != null)
            {
                throw new NotImplementedException("This is the first time I've seen assembly parameter with a value!");
            }

            var comparisonType = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            var query = _assemblies.Select(a => FindFirstTypeInAssemblyByName(a, typeName, comparisonType));
            var type = query.FirstOrDefault(t => t != null);

            return type;
        }

        private static Type FindFirstTypeInAssemblyByName(Assembly assembly, string typeName, StringComparison comparisonType)
        {
            return assembly.GetTypes().FirstOrDefault(t => t.Name.Equals(typeName, comparisonType));
        }

        private Assembly AssemblyResolver(AssemblyName assemblyName)
        {
            throw new NotImplementedException();
        }

        public Type GetType(string typeName)
        {
            return Type.GetType(typeName, AssemblyResolver, TypeResolverResolver, true);
        }
    }
}