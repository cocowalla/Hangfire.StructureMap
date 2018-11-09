using System;
using StructureMap;
using Hangfire.Annotations;

namespace Hangfire.StructureMap
{
    /// <summary>
    /// Global Configuration extensions for StructureMap job activation
    /// </summary>
    public static class GlobalConfigurationExtensions
    {
        /// <summary>
        /// Use the specified StructureMap container to create a <see cref="StructureMapJobActivator"/> for resolving job dependencies
        /// </summary>
        /// <param name="configuration">Global configuration</param>
        /// <param name="container">Container used to create nested containers, which will in turn build job dependencies
        /// during the activation process
        /// </param>
        /// <returns>An instance of <see cref="IGlobalConfiguration{StructureMapJobActivator}"/>, configured to use StructureMap
        /// to resolve job dependencies
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="configuration"/>, <paramref name="container"/></exception>
        public static IGlobalConfiguration<StructureMapJobActivator> UseStructureMapActivator([NotNull] this IGlobalConfiguration configuration, 
            [NotNull] IContainer container)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (container == null) throw new ArgumentNullException(nameof(container));

            return configuration.UseActivator(new StructureMapJobActivator(container));
        }
    }
}
