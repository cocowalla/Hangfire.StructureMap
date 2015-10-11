using System;
using StructureMap;

namespace Hangfire.StructureMap
{
    public class StructureMapJobActivator : JobActivator
    {
        private readonly IContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapJobActivator"/>
        /// class with a given StructureMap container
        /// </summary>
        /// <param name="container">Container that will be used to create instances of classes during 
        /// the job activation process</param>
        public StructureMapJobActivator(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }

        /// <inheritdoc />
        public override object ActivateJob(Type jobType)
        {
            return container.GetInstance(jobType);
        }    
    }
}
