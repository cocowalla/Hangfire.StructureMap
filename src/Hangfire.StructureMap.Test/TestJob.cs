namespace Hangfire.StructureMap.Test
{
    using System;

    public class TestJob
    {
        public TestJob(BackgroundJobDependency backgroundJobDependency, UniqueDependency uniqueDependency, ObjectDependsOnSameDependency sameDependencyObject)
        {
            BackgroundJobDependency = backgroundJobDependency;
            UniqueDependency = uniqueDependency;
            SameDependencyObject = sameDependencyObject;
        }

        public BackgroundJobDependency BackgroundJobDependency { get; private set; }

        public UniqueDependency UniqueDependency { get; private set; }

        public ObjectDependsOnSameDependency SameDependencyObject { get; private set; }
    }

    public class ObjectDependsOnSameDependency
    {
        public ObjectDependsOnSameDependency(BackgroundJobDependency backgroundJobDependency, UniqueDependency uniqueDependency)
        {
            BackgroundJobDependency = backgroundJobDependency;
            UniqueDependency = uniqueDependency;
        }

        public BackgroundJobDependency BackgroundJobDependency { get; private set; }

        public UniqueDependency UniqueDependency { get; private set; }
    }

    public class BackgroundJobDependency : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Disposed = true;
        }
    }

    public class UniqueDependency : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Disposed = true;
        }
    }
}
