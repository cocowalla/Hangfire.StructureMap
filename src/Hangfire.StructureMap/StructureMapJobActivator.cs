namespace Hangfire.StructureMap
{
    using System;
    using System.Linq;

    using global::StructureMap;
    using global::StructureMap.Pipeline;

    /// <summary>
    /// StructureMap Job Activator.
    /// </summary>
    public class StructureMapJobActivator : JobActivator
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapJobActivator"/>
        /// class with a given StructureMap container
        /// </summary>
        /// <param name="container">Container that will be used to create instances of classes during
        /// the job activation process</param>
        public StructureMapJobActivator(IContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            _container = container;
        }

        /// <inheritdoc />
        public override object ActivateJob(Type jobType)
        {
            return _container.GetInstance(jobType);
        }

        /// <inheritdoc />
        public override JobActivatorScope BeginScope()
        {
            return new StructureMapDependencyScope(_container.GetNestedContainer());
        }

        private class StructureMapDependencyScope : JobActivatorScope
        {
            private readonly IContainer _container;

            public StructureMapDependencyScope(IContainer container)
            {
                _container = container;
            }

            public override object Resolve(Type type)
            {
                return _container.GetInstance(type);
            }

            public override void DisposeScope()
            {
                var containerLifecycleInstanceRef = _container.Model.AllInstances.FirstOrDefault(@ref => @ref.Lifecycle is ContainerLifecycle);

                if (containerLifecycleInstanceRef != null) containerLifecycleInstanceRef.Lifecycle.EjectAll(_container.Model.Pipeline);

                _container.Dispose();
            }
        }
    }
}
