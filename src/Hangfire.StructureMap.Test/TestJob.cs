
namespace Hangfire.StructureMap.Test
{
    public class TestJob
    {
        public TestJob(int dependency)
        {
            this.Dependency = dependency;
        }

        public int Dependency { get; private set; }
    }
}
