namespace Hangfire.StructureMap
{
    using System;

    using Hangfire.Annotations;

    using global::StructureMap;

    /// <summary>
    /// Global Configuration extensions.
    /// </summary>
    public static class GlobalConfigurationExtensions
    {
        /// <summary>
        /// Tells global configuration to use the specified StructureMap container as a job activator.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="container">The container.</param>
        /// <returns>An instance of <see cref="IGlobalConfiguration{StructureMapJobActivator}"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// configuration
        /// or
        /// container
        /// </exception>
        public static IGlobalConfiguration<StructureMapJobActivator> UseStructureMapActivator([NotNull] this IGlobalConfiguration configuration, [NotNull] IContainer container)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (container == null) throw new ArgumentNullException("container");

            return configuration.UseActivator(new StructureMapJobActivator(container));
        }
    }
}
