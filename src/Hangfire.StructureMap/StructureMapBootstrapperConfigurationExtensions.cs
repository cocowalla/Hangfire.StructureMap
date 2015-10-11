namespace Hangfire.StructureMap
{
    using System;

    using global::StructureMap;

    /// <summary>
    /// Bootstrapper Configuration Extensions for StructureMap.
    /// </summary>
    public static class StructureMapBootstrapperConfigurationExtensions
    {
        /// <summary>
        /// Tells bootstrapper to use the specified StructureMap container as a global job activator.
        /// </summary>
        /// <param name="configuration">Bootstrapper Configuration</param>
        /// <param name="container">StructureMap container that will be used to activate jobs</param>
        [Obsolete("Please use `GlobalConfiguration.Configuration.UseStructureMapActivator` method instead. Will be removed in version 2.0.0.")]
        public static void UseStructureMapActivator(this IBootstrapperConfiguration configuration, IContainer container)
        {
            configuration.UseActivator(new StructureMapJobActivator(container));
        }
    }
}
