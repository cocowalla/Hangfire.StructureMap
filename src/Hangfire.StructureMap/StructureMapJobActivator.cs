namespace Hangfire.StructureMap
{
    using System;

    using global::StructureMap;

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
                BackgroundJobLifecycle.DisposeAndClearAll();
                _container.Dispose();
            }
        }
    }
}
