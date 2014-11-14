using StructureMap;

namespace Hangfire.StructureMap
{
    public static class StructureMapBootstrapperConfigurationExtensions
    {
        /// <summary>
        /// Tells bootstrapper to use the specified StructureMap container as a global job activator.
        /// </summary>
        /// <param name="configuration">Configuration</param>
        /// <param name="container">StructureMap container that will be used to activate jobs</param>
        public static void UseStructureMapActivator(this IBootstrapperConfiguration configuration, IContainer container)
        {
            configuration.UseActivator(new StructureMapJobActivator(container));
        }
    }
}
