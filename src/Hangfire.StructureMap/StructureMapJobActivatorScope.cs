using System;
using StructureMap;

namespace Hangfire.StructureMap
{
    internal class StructureMapDependencyScope : JobActivatorScope
    {
        private readonly IContainer container;

        public StructureMapDependencyScope(IContainer container)
        {
            this.container = container;
        }

        public override object Resolve(Type type)
        {
            return this.container.GetInstance(type);
        }

        public override void DisposeScope()
        {
            this.container.Dispose();
        }
    }
}
