using Autumn.Engine;

namespace Autumn
{
    /// <summary>
    /// Autumn application
    /// </summary>
    public class Application
    {
        private ApplicationContext context;
        
        public void Start() => Start(new string[0]);

        public void Start(string[] args) => Start(new ApplicationContext(), args);
        
        public void Start(ApplicationContext context, string[] args)
        {
            //Initialize context
            
            //Collect Configurations
            //Collect Beans
            //Collect Components
            
            //Construct Components
            //Invoke PostConstruct
        }
    }
}