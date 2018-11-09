using System;
using StructureMap;

namespace Hangfire.StructureMap
{
#if NET45
    /// <summary>
    /// Bootstrapper extensions for StructureMap job activation
    /// </summary>
    public static class BootstrapperConfigurationExtensions
    {
        /// <summary>
        /// Use the specified StructureMap container to create a <see cref="StructureMapJobActivator"/> for resolving job dependencies
        /// </summary>
        /// <param name="configuration">Bootstrapper configuration</param>
        /// <param name="container">Container used to create nested containers, which will in turn build job dependencies
        /// during the activation process
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="configuration"/>, <paramref name="container"/></exception>
        [Obsolete("Please use `GlobalConfiguration.Configuration.UseStructureMapActivator` method instead. Will be removed in version 2.0.0.")]
        public static void UseStructureMapActivator(this IBootstrapperConfiguration configuration, IContainer container)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            configuration.UseActivator(new StructureMapJobActivator(container));
        }
    }
#endif
}
