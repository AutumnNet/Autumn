using System;
using System.Reflection;
using Autumn.Annotation;

namespace Autumn.Scanner
{
    public class ComponentContainer
    {
        public Type ComponentType { get;}

        public ConstructorInfo Constructor { private set; get; }
            
        public ParameterInfo[] ConstructorParameterInfo { private set; get; }
            
            
        public object Instantiate(ComponentStorage storage)
        {
            if (ConstructorParameterInfo == null || ConstructorParameterInfo.Length == 0)
                return Constructor.Invoke(null);
            return null;
        }

        private void GetConstructor()
        {
            ConstructorParameterInfo = new ParameterInfo[0];
            foreach (var constructor in ComponentType.GetConstructors())
            {
                if (constructor.GetCustomAttributes(typeof(Autowired), false).Length > 0)
                {
                    Constructor = constructor;
                    ConstructorParameterInfo = constructor.GetParameters();
                    return;
                }
                if (constructor.GetParameters().Length == 0)
                    Constructor = constructor;
            }
        }
            
        public ComponentContainer(Type componentType)
        {
            ComponentType = componentType;
            GetConstructor();
        }
    }

}