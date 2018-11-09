using System;
using StructureMap;

namespace Hangfire.StructureMap
{
    /// <summary>
    /// StructureMap Job Activator - builds job dependencies using nested StructureMap containers
    /// </summary>
    public class StructureMapJobActivator : JobActivator
    {
        private readonly IContainer container;

        /// <summary>
        /// Initialize a new instance of <see cref="StructureMapJobActivator"/> with a StructureMap container
        /// </summary>
        /// <param name="container">Container used to create nested containers, which will in turn build job dependencies
        /// during the activation process
        /// </param>
        public StructureMapJobActivator(IContainer container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <inheritdoc />
        /// <summary>
        /// Activate a job using the parent container
        /// </summary>
        /// <returns>An activated job of type <paramref name="jobType"/></returns>
        public override object ActivateJob(Type jobType)
        {
            return this.container.GetInstance(jobType);
        }

        /// <inheritdoc />
        /// <summary>
        /// Begin a new job activation scope using a nested container
        /// </summary>
        /// <param name="context">Job activator context</param>
        /// <returns>A new job activation scope</returns>
        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            return new StructureMapDependencyScope(this.container.GetNestedContainer());
        }

#pragma warning disable CS0672 // Member overrides obsolete member
        /// <inheritdoc />
        public override JobActivatorScope BeginScope()
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            return new StructureMapDependencyScope(this.container.GetNestedContainer());
        }
    }
}
