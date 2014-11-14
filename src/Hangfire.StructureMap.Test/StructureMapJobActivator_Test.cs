using System;
using FluentAssertions;
using StructureMap;
using Xunit;

namespace Hangfire.StructureMap.Test
{
    public class StructureMapJobActivator_Test
    {
        [Fact]
        public void Ctor_Should_Throw_When_Container_Is_Null()
        {
            Action a = () =>
            {
                var activator = new StructureMapJobActivator(null);
            };

            a.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Can_Resolve_Dependencies()
        {
            int expectedValue = 123;

            var container = new Container(x =>
                x.For<TestJob>()
                    .Use<TestJob>()
                    .Ctor<int>()
                    .Is(expectedValue)
            );

            var activator = new StructureMapJobActivator(container);
            var result = (TestJob)activator.ActivateJob(typeof(TestJob));

            result.Dependency.ShouldBeEquivalentTo(expectedValue);
        }
    }
}
