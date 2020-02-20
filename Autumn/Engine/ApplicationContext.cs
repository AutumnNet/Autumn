using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autumn.Net.Annotation;
using Autumn.Net.Exceptions;
using Autumn.Net.Interfaces;
using Autumn.Net.Object;
using Autumn.Net.Tools;

namespace Autumn.Net.Engine
{
    public class ApplicationContext
    {
        /// <summary>
        /// Application Paremeters
        /// </summary>
        public ApplicationParameter ApplicationParameter { get; private set; }
        
        /// <summary>
        /// Components Types
        /// </summary>
        private Dictionary<Type, HashSet<ComponentType>> ComponentTypes { get; }

        private Dictionary<ComponentType, object> ComponentInstance { get; }

        private HashSet<object> WaitAutowiredInstances { get; } 

        /// <summary>
        /// IAutumnComponentInitializationProcessor's 
        /// </summary>
        private List<ComponentType> ComponentProcessor { get; }
        
        /// <summary>
        /// Autowired Instance
        /// </summary>
        public HashSet<object> AutowiredInstance { get; }

        private IEnumerable<string> _profiles;
        
        /// <summary>
        /// Current Profiles
        /// </summary>
        public IEnumerable<string> Profiles
        {
            get
            {
                return _profiles ?? (_profiles = ((string) ApplicationParameter.GetOrDefault("profile", "base"))
                           .Split(',').Select(item => item.Trim()));
            }
        }

        private void Invoke(MethodInfo mi, object target)
        {
            var autowired = mi.GetCustomAttribute<AutowiredAttribute>(true);
            mi.Invoke(target, mi.GetAutumnMethodArguments( new AutowiredContext(target,this, mi.GetCustomAttribute<QualifierAttribute>(), mi, autowired)));
        }

        /// <summary>
        /// DI Component
        /// </summary>
        /// <param name="o">Object</param>
        public void Autowire(object o)
        {
            
            //Console.WriteLine($"Autowire {o.GetType().FullName}");
//          o
//                .GetType()
//                .GetFields(BindingFlags.SetField | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
//                .Where(field => field.GetCustomAttributes(typeof(AutowiredAttribute), true).Length > 0)
//                .ToList().ForEach(item => Console.WriteLine($" `--> {item.Name}"));
            
            //new AutowiredContext()

            o
                .GetType()
                .GetFields(BindingFlags.SetField | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(field => field.GetCustomAttributes(typeof(AutowiredAttribute), true).Length > 0)
                .ToList()
                .ForEach(item =>
                {
                    var autowired = item.GetCustomAttribute<AutowiredAttribute>(true);
                    item.SetValue(o,
                            GetInstance(item.FieldType,
                                new AutowiredContext(o, this, item.GetCustomAttribute<QualifierAttribute>(), item, autowired)));
                });
            
            o
                .GetType()
                .GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(field => field.GetCustomAttributes(typeof(AutowiredAttribute), true).Length > 0)
                .ToList()
                .ForEach(item =>
                {
                    var autowired = item.GetCustomAttribute<AutowiredAttribute>(true);
                    item.SetValue(o,
                            GetInstance(item.PropertyType,
                                new AutowiredContext(o, this, item.GetCustomAttribute<QualifierAttribute>(), item, autowired)));
                });

            ComponentProcessor.ForEach(itemType =>
            {
                var instance = GetInstance(itemType, GetAutowiredContext()) as IAutumnComponentInitializationProcessor;
                instance?.Process(o, this);
            });
            
            AutowiredInstance.Add(o);
        }

        /// <summary>
        /// Post Construct Object
        /// </summary>
        /// <param name="o">Object</param>
        public void PostConstruction(object o)
        {
            o
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(method => method.GetCustomAttributes(typeof(PostConstructAttribute), true).Length > 0)
                .ToList()
                .ForEach(method => Invoke(method, o));
        }

        /// <summary>
        /// PreDestroy Autowired component
        /// </summary>
        public void PreDestroy()
        {
            var aw = AutowiredContext.Base(this);
            AutowiredInstance.ToList().ForEach(item =>
            {
                item.GetType()
                    .GetMethods()
                    .Where(mi => mi.GetCustomAttributes(typeof(PreDestroyAttribute), true).Length > 0)
                    .ToList()
                    .ForEach(mi => mi.Invoke(item, mi.GetAutumnMethodArguments(aw)));
            });
        }
        
        /// <summary>
        /// Get one Instance of Type
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="ctx">Autowired Contexte</param>
        /// <returns>Single Instance</returns>
        /// <exception cref="AutumnComponentNotFoundException">Component not found</exception>
        /// <exception cref="AutumnComponentMultiplyException">Multiplies component found</exception>
        //public object GetInstance(Type type, IAutowiredName qualifier = null, IEnumerable<IOption> options = null)
        public object GetInstance(Type type, AutowiredContext ctx = null)
        {
            if (ctx == null)
                ctx = AutowiredContext.Base(this);
            
            if (type.IsMultiplierType()) return GetInstances(type, ctx);
            
            if (!ComponentTypes.ContainsKey(type))
                if (ctx == null || ctx.AutowiredRequired)
                    throw new AutumnComponentNotFoundException(type);
                else
                    return null;
            var componentTypes = ComponentTypes[type];
            
            if (ctx.Qualifier != null)
                componentTypes = new HashSet<ComponentType>(componentTypes.Where(item => ctx.Qualifier.IsName(item.Name)));
            
            if (componentTypes.Count == 1) return GetInstance(componentTypes.First(), ctx);
            if (componentTypes.Count(item => item.IsPrimary) == 1)
                return GetInstance(componentTypes.First(item => item.IsPrimary), ctx);
            if (componentTypes.Count(item => item.IsPrimary) > 1)
                throw new AutumnComponentMultiplyPrimaryException(type, componentTypes);
            throw new AutumnComponentMultiplyException(type, componentTypes);
        }

        
        private object GetInstance(ComponentType componentType, AutowiredContext ctx) => GetInstance(componentType, ctx, true); 
        private object GetInstance(ComponentType componentType, AutowiredContext ctx, bool autowired)
        {
            if (!componentType.Singleton || !ComponentInstance.ContainsKey(componentType))
            {
                if (componentType.IsBean)
                {
                    //Console.WriteLine("GetBean:{0}", componentType);
                    var arguments = componentType.BeanMethodInfo.GetAutumnMethodArguments(ctx);
                    //May create in recursive for Arguments
                    //Console.WriteLine("Arguments:{0}", arguments.Length);
                    if (componentType.Singleton && ComponentInstance.ContainsKey(componentType))
                        return ComponentInstance[componentType];
                    ComponentInstance.Add(componentType, 
                        componentType.BeanMethodInfo.Invoke(GetInstance(componentType.BeanTargetType, ctx), arguments));
                }
                else
                {
                    var arguments = componentType.Constructor.GetAutumnConstructorArguments(ctx);
                    //May create in recursive for Arguments
                    if (componentType.Singleton && ComponentInstance.ContainsKey(componentType))
                        return ComponentInstance[componentType];

                    ComponentInstance.Add(componentType, componentType.Constructor.Invoke(arguments));
                    if (autowired)
                    {
                        Autowire(ComponentInstance[componentType]);
                        PostConstruction(ComponentInstance[componentType]);
                    }
                    else
                        WaitAutowiredInstances.Add(ComponentInstance[componentType]);
                }
            }
            return ComponentInstance[componentType];            
        }

        /// <summary>
        /// Get List of Instances
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="qualifier"></param>
        /// <returns>IEnumerable of Instances</returns>
        /// <exception cref="AutumnComponentNotFoundException">Component not found</exception>
        public object GetInstances(Type type, AutowiredContext ctx)
        {
            var elementType = type.GetMultiplierElementType();
            if (!ComponentTypes.ContainsKey(elementType))
                if (ctx.AutowiredRequired)
                    throw new AutumnComponentNotFoundException(elementType);
                else
                    return AssemblyHelper.GetMultiplierObject(type, new object[0]);
            
            var array = ctx.Qualifier != null ? 
                ComponentTypes[elementType].Where(item => ctx.Qualifier.IsName(item.Name)).Select(item => GetInstance(item, ctx)) : 
                ComponentTypes[elementType].Select(item => GetInstance(item, ctx));
            return AssemblyHelper.GetMultiplierObject(type, array);
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
        
        public AutowiredContext GetAutowiredContext()
        {
            return AutowiredContext.Base(this);
        }

        
        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationContext(ApplicationParameter parameter) : this(parameter, new ApplicationConfiguration(Assembly.GetCallingAssembly()).ConfiguredAssemblies) {}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="mainAssembly"></param>
        public ApplicationContext(ApplicationParameter parameter, Assembly mainAssembly) : this(parameter, new ApplicationConfiguration(mainAssembly).ConfiguredAssemblies) {}
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assembly">Assembly</param>
        public ApplicationContext(ApplicationParameter parameter, IEnumerable<Assembly> assemblies)
        {
            // Create Component Types
            AutowiredInstance = new HashSet<object>();
            ComponentTypes = new Dictionary<Type, HashSet<ComponentType>>();
            ComponentInstance = new Dictionary<ComponentType, object>();
            WaitAutowiredInstances = new HashSet<object>();
            ApplicationParameter = parameter ?? new EmptyApplicationParameter();
            ComponentProcessor = new List<ComponentType>();
            var configurations = new HashSet<ComponentType>();
            var components = new HashSet<ComponentType>();

            foreach (var assembly in assemblies)
            {
                //Console.WriteLine("Get Configuration from Assembly:{0}", assembly.GetName().Name);
                foreach (var componentType in assembly.GetAutumnComponents(true).Profiled(Profiles))
                {
                    var item = new ComponentType(componentType);
                    AddComponentType(item);
                    if (item.Type.GetInterfaces().Contains(typeof(IAutumnComponentInitializationProcessor)))
                        ComponentProcessor.Add(item);
                    if (componentType.IsAutumnConfiguration())
                        configurations.Add(item);
                    else
                        components.Add(item);
                }
            }

            var aw = AutowiredContext.Base(this);
            // Instantiate Configurations
            foreach (var configurationType in configurations)
            {
                
                if (configurationType.Singleton && !configurationType.Lazy)
                    GetInstance(configurationType, aw, false); // Create Items
                
                //Console.WriteLine("CFG {0} BEANS COUNT: {1}", configurationType.Type, configurationType.Type.GetAutumnBeans().Count());
                foreach (var bean in configurationType.Type.GetAutumnBeans().Profiled(Profiles))
                {
                    //Console.WriteLine("BEAN: {0}", bean.Name);
                    var item = new ComponentType(bean, configurationType.Type);
                    AddComponentType(item);
                    if (item.Singleton && !item.Lazy)
                        GetInstance(item, aw, false); // Create Beans
                }
            }

            foreach (var componentType in components)
                if (componentType.Singleton && !componentType.Lazy)
                    GetInstance(componentType, aw,false); // Create other Components

            //Console.WriteLine($"Autowired queue size:{WaitAutowiredInstances.Count}");
            
            foreach (var instance in WaitAutowiredInstances)
                Autowire(instance);
            //Console.WriteLine($"Autowired Done");
            
            //Console.WriteLine($"PostConstruction queue size:{WaitAutowiredInstances.Count}");
            foreach(var instance in WaitAutowiredInstances)
                PostConstruction(instance);
            //Console.WriteLine($"PostConstruction Done");
            WaitAutowiredInstances.Clear();
            
        }
    }
}