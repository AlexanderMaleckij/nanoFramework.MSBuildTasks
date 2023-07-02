using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using Microsoft.Build.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace nanoFramework.MSBuildTasks.Tasks
{
    // https://github.com/dotnet/arcade/blob/main/Documentation/Mechanics/MSBuildTaskDependencyInjection.md
    [ExcludeFromCodeCoverage]
    public abstract partial class MSBuildTaskBase : Task
    {
        /// <summary>
        /// This is the name of the method on the implementation of this class that behaves as "Execute" would in a basic
        /// implementation of an MSBuild Task. 
        /// </summary>
        private const string ExecuteMethodName = "ExecuteTask";

        /// <summary>
        /// An MSBuild Task uses the Execute method as main method as an entry point into the task's functionality. 
        /// It is extended in this abstract class, behind the scenes, to configure dependency injection. 
        /// </summary>
        /// <returns></returns>
        public override sealed bool Execute()
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            using (var provider = serviceCollection.BuildServiceProvider())
            {
                return InvokeExecute(provider);
            }
        }

        /// <summary>
        /// A helper method that takes the service provider contains and extrapolates the dependencies referenced in the
        /// ExecuteTask method and then subsequently invokes the ExecuteTask method. 
        /// 
        /// This method can also be called from tests so that the service provider can contain any mocks that will be used
        /// in the test. 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public bool InvokeExecute(ServiceProvider provider)
        {
            return (bool)GetExecuteMethod().Invoke(this, GetExecuteArguments(provider));
        }

        /// <summary>
        /// Default service configuration. This method can be overridden in the implemented class so that 
        /// it can configure the dependencies that the implemented task requires. 
        /// 
        /// This method can also be called from the tests to complete the service collection with any dependencies 
        /// that aren't configured for the tests. 
        /// </summary>
        /// <param name="collection"></param>
        public abstract void ConfigureServices(IServiceCollection collection);

        /// <summary>
        /// Uses some reflection magic to look up the types of dependencies that to be injected into the implemented task. 
        /// </summary>
        /// <returns></returns>
        public Type[] GetExecuteParameterTypes()
        {
            return GetType().GetMethod(ExecuteMethodName).GetParameters().Select(p => p.ParameterType).ToArray();
        }

        /// <summary>
        /// Looks up all the dependencies from the service provider that are defined as parameters in ExecuteTask
        /// so they can be injected into the ExecuteTask method when it's called. 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        private object[] GetExecuteArguments(ServiceProvider serviceProvider)
        {
            return GetExecuteParameterTypes().Select(t => serviceProvider.GetRequiredService(t)).ToArray();
        }

        /// <summary>
        /// Gets the ExecuteTask method that is in the implementation of the MSBuild Task. 
        /// </summary>
        /// <returns></returns>
        private MethodInfo GetExecuteMethod()
        {
            return GetType().GetMethod(ExecuteMethodName);
        }
    }
}
