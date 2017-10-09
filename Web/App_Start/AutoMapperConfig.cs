using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using Core;
using Core.Domain;
using NLog;
using Web.Infrastructure;
using Web.Infrastructure.Extensions;

namespace Web
{
	/// <summary>
	/// A marker interface, everything that uses this interface will automatically
	/// have an Automapper map created from the requested type on app start
	/// </summary>
	public interface IMapFrom<T> { }
	/// <summary>
	/// A marker interface, everything that uses this interface will automatically
	/// have an Automapper map created to the requested type on app start
	/// </summary>
	public interface IMapTo<T> { }
    /// <summary>
    /// When the IMapFrom<>, IMapTo<>, are too simplistic, tag a static method in the class
    /// with this attribute to provid eyour own
    /// Usage:
    ///     [GenerateMappingWith] public static void CreateMap(IConfiguration c) {
    ///         c.CreateMap(....)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class GenerateMappingWithAttribute : Attribute { }
	public static class AutoMapperConfig
	{
		public static void Initialize() {
            Mapper.Initialize(c =>
                Initialize(c, typeof(AutoMapperConfig).Assembly)
            );
		}

        public static void Initialize(IConfiguration config, Assembly assembly) {
			createMaps(assembly, typeof(IMapFrom<>),
				(t, i) => new {
					DestinationType = t,
					SourceType = i.GetGenericArguments()[0],
				},
				(x) => {
					log.Debug($"Creating map for model {x.SourceType} to {x.DestinationType}");
					createMap(config, x.SourceType, x.DestinationType);
				});
			createMaps(assembly, typeof(IMapTo<>),
				(t, i) => new {
					DestinationType = i.GetGenericArguments()[0],
					SourceType = t,
				},
				(x) => {
					log.Debug($"Creating map for model {x.SourceType} to {x.DestinationType}");
					createMap(config, x.SourceType, x.DestinationType);
				});
            createManualMaps(config, assembly);
			createEnumerationToDbSerializableCollectionMaps(config, assembly);
		}

		static void createMaps<T>(Assembly assembly, Type mapType, Func<Type, Type, T> interfaceMaps, Action<T> apply) {
			assembly.GetTypes()
				.SelectMany(t =>
					t.AllInterfaces().Where(i => i.IsGenericType && !i.IsGenericTypeDefinition)
						.Where(i => i.GetGenericTypeDefinition() == mapType)
						.Select(i => interfaceMaps(t, i))
				).ForEach(apply);
		}

        static void createManualMaps(IConfiguration config, Assembly assembly) {
            assembly.GetTypes()
                    .SelectMany(t => t.GetMethods(BindingFlags.Static| BindingFlags.Public))
                    .Where(mi => mi.HasAttribute<GenerateMappingWithAttribute>())
                    .ForEach(mi => mi.Invoke(null, new object[] { config }));
        }

		static void createMap(IConfiguration config, Type sourceType, Type destinationType) {
			if (destinationType.IsAbstract || destinationType.ContainsGenericParameters || !destinationType.IsClass)
				return;
            ignorePropertiesWithIdsMappingToEntities( config.CreateMap(sourceType, destinationType), sourceType, destinationType );
		}

		static void createEnumerationToDbSerializableCollectionMaps(IConfiguration config, Assembly assembly) {
			assembly.GetTypes().Where(t => !t.IsAbstract && t.BaseType != typeof(object))
				.Where(t => t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(DbSerializableCollection<>))
				.Select(t => new {
					CollectionType = t,
					ItemType = t.BaseType.GetGenericArguments()[0],
				}).Select(x => new {
					x.CollectionType,
					EnumerationType = typeof(IEnumerable<>).MakeGenericType(x.ItemType),
					TypeConverter = typeof(DbSerializableCollectionTypeConverter<,>).MakeGenericType(x.ItemType, x.CollectionType),
				})
				.ForEach(x => {
					log.Debug($"Creating map for model {x.EnumerationType} to {x.CollectionType}");
					config.CreateMap(x.EnumerationType, x.CollectionType)
						.ConvertUsing(x.TypeConverter);
                });
		}

        static void ignorePropertiesWithIdsMappingToEntities(IMappingExpression mapping, Type source, Type destination) {
            var idTypes = new[] { typeof(Guid), typeof(Guid?) };

            var destinationProps = propertiesOf(destination).ToDictionary(pi => pi.Name);

            var sourceProps             = propertiesOf(source);
            var whereThePropertyIsAnId  = sourceProps.Where(p => idTypes.Contains(p.PropertyType));
            var withDestination         = whereThePropertyIsAnId.Select(SourceProp => new { SourceProp, DestinationProp = destinationProps.TryGetValue(SourceProp.Name) });
            var andTheTargetIsAnEntity  = withDestination.Where(x => x.DestinationProp?.PropertyType.IsAssignableTo<IEntity>()??false );

            foreach (var sourceProp in andTheTargetIsAnEntity.Select(x => x.SourceProp.Name))
                mapping.ForMember(sourceProp, x => x.Ignore());
        }
        static PropertyInfo[] propertiesOf(Type type) {
            return type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }


        public class DbSerializableCollectionTypeConverter<ItemType, CollectionType> : ITypeConverter<List<ItemType>, CollectionType>
			where CollectionType : DbSerializableCollection<ItemType>, new()
		{
			public CollectionType Convert(ResolutionContext context) {
				var destination = (context.DestinationValue as CollectionType) ?? new CollectionType();
				destination.Set((context.SourceValue as IEnumerable<ItemType>).ToArray());
				return destination;
			}
		}

		static readonly Logger log = LogManager.GetLogger(typeof(AutoMapperConfig).FullName);
	}
}