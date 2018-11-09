using System;

namespace Hangfire.StructureMap.Test
{
    public class TestJob
    {
        public BackgroundJobDependency BackgroundJobDependency { get; }
        public UniqueDependency UniqueDependency { get; }
        public ObjectDependsOnSameDependency SameDependencyObject { get; }

        public TestJob(BackgroundJobDependency backgroundJobDependency, UniqueDependency uniqueDependency, 
            ObjectDependsOnSameDependency sameDependencyObject)
        {
            this.BackgroundJobDependency = backgroundJobDependency;
            this.UniqueDependency = uniqueDependency;
            this.SameDependencyObject = sameDependencyObject;
        }
    }

    public class ObjectDependsOnSameDependency
    {
        public BackgroundJobDependency BackgroundJobDependency { get; }
        public UniqueDependency UniqueDependency { get; }

        public ObjectDependsOnSameDependency(BackgroundJobDependency backgroundJobDependency, UniqueDependency uniqueDependency)
        {
            this.BackgroundJobDependency = backgroundJobDependency;
            this.UniqueDependency = uniqueDependency;
        }
    }

    public class BackgroundJobDependency : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            this.Disposed = true;
        }
    }

    public class UniqueDependency : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            this.Disposed = true;
        }
    }
}
