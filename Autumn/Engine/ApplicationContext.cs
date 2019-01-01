using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autumn.Annotation;
using Autumn.Exceptions;
using Autumn.Object;
using Autumn.Tools;

namespace Autumn.Engine
{
    public class ApplicationContext
    {
        public ApplicationParameter ApplicationParameter { get; private set; }
        
        /// <summary>
        /// Components Types
        /// </summary>
        private Dictionary<Type, HashSet<ComponentType>> ComponentTypes { get; }

        private Dictionary<ComponentType, object> ComponentInstance { get; }

        private HashSet<object> WaitAutowiredInstances { get; } 

        /// <summary>
        /// Get one Instance of Type
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Single Instance</returns>
        /// <exception cref="AutumnComponentNotFoundException">Component not found</exception>
        /// <exception cref="AutumnComponentMultiplyException">Multiplies component found</exception>
        public object GetInstance(Type type)
        {
            if (!ComponentTypes.ContainsKey(type))
                throw new AutumnComponentNotFoundException(type);
            var componentTypes = ComponentTypes[type];
            if (componentTypes.Count > 1)
                throw new AutumnComponentMultiplyException(type, componentTypes);

            return GetInstance(componentTypes.First());
        }

        private void Invoke(MethodInfo mi, object target)
        {
            mi.Invoke(target, mi.GetAutumnMethodArguments(this));
        }

        private void Autowireding(object o)
        {
            o
                .GetType()
                .GetFields(BindingFlags.SetField | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(field => field.GetCustomAttributes(typeof(AutowiredAttribute), true).Length > 0)
                .ToList()
                .ForEach(item => GetInstance(item.FieldType));
            
            o
                .GetType()
                .GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(field => field.GetCustomAttributes(typeof(AutowiredAttribute), true).Length > 0)
                .ToList()
                .ForEach(item => GetInstance(item.PropertyType));
        }

        private void PostConstruction(object o)
        {
            o
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(method => method.GetCustomAttributes(typeof(PostConstruct), true).Length > 0)
                .ToList()
                .ForEach(method => Invoke(method, o));
        }

        private object GetInstance(ComponentType componentType) => GetInstance(componentType, true); 
        private object GetInstance(ComponentType componentType, bool autowired)
        {
            if (!componentType.Singleton || !ComponentInstance.ContainsKey(componentType))   
                ComponentInstance.Add(componentType, componentType.Constructor
                    .Invoke(componentType.Constructor.GetAutumnConstructorArguments(this)));
            if (autowired)
            {
                Autowireding(ComponentInstance[componentType]);
                PostConstruction(ComponentInstance[componentType]);
            }else
                WaitAutowiredInstances.Add(ComponentInstance[componentType]);
            return ComponentInstance[componentType];            
        }
        
        /// <summary>
        /// Get List of Instances
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>IEnumerable of Instances</returns>
        /// <exception cref="AutumnComponentNotFoundException">Component not found</exception>
        public IEnumerable<object> GetInstances(Type type)
        {
            if (!ComponentTypes.ContainsKey(type))
                throw new AutumnComponentNotFoundException(type);
            return ComponentTypes[type].Select(GetInstance);
        }
        
        /// <summary>
        /// Add Component Inheritance types into Dictionary
        /// </summary>
        /// <param name="item">ComponentType</param>
        private void AddComponentType(ComponentType item)
        {
            foreach (var iType in item.InheritanceTypes)
            {
                if (!ComponentTypes.ContainsKey(iType))
                    ComponentTypes.Add(iType, new HashSet<ComponentType>());
                ComponentTypes[iType].Add(item);
            }
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationContext() : this(AssemblyHelper.GetDefaultAssembly()){}
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assembly">Assembly</param>
        public ApplicationContext(Assembly assembly)
        {
            // Create Component Types
            ComponentTypes = new Dictionary<Type, HashSet<ComponentType>>();
            ComponentInstance = new Dictionary<ComponentType, object>();
            WaitAutowiredInstances = new HashSet<object>();
            var configurations = new HashSet<ComponentType>();
            var components = new HashSet<ComponentType>();
            foreach (var componentType in assembly.GetAutumnComponents(true))
            {
                var item = new ComponentType(componentType);
                AddComponentType(item);
                if (componentType.IsAutumnConfiguration())
                    configurations.Add(item);
                else
                    components.Add(item);
            }

            // Instantiate Configurations
            foreach (var configurationType in configurations)
            {
                if (configurationType.Singleton && !configurationType.Lazy)
                    GetInstance(configurationType, false); // Create Items
                foreach (var bean in configurationType.GetType().GetAutumnBeans())
                {
                    var item = new ComponentType(bean, configurationType.Type);
                    AddComponentType(item);
                    if (item.Singleton && !item.Lazy)
                        GetInstance(item, false); // Create Beans
                }
            }

            foreach (var componentType in components)
                if (componentType.Singleton && !componentType.Lazy)
                    GetInstance(componentType, false); // Create other Components

            foreach(var instance in WaitAutowiredInstances)
                Autowireding(instance);
            
            foreach(var instance in WaitAutowiredInstances)
                PostConstruction(instance);
            
            WaitAutowiredInstances.Clear();
        }
    }
}