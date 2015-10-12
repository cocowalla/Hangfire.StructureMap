namespace Hangfire.StructureMap.Test
{
    using System;

    using Moq;

    using global::StructureMap;

    using Xunit;

    public class StructureMapJobActivatorTest
    {
        private readonly IContainer _container;

        public StructureMapJobActivatorTest()
        {
            _container = new Container();
        }

        [Fact]
        public void Ctor_Should_Throw_When_Container_Is_Null()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new StructureMapJobActivator(null));
            Assert.Equal("Value cannot be null.\r\nParameter name: container", exception.Message);
        }

        [Fact]
        public void Class_Is_Based_On_JobActivator()
        {
            var activator = CreateActivator();
            Assert.IsAssignableFrom<JobActivator>(activator);
        }

        [Fact]
        public void ActivateJob_Calls_StructureMap()
        {
            _container.Inject("called");
            var activator = CreateActivator();
            var result = activator.ActivateJob(typeof(string));
            Assert.Equal("called", result);
        }

        [Fact]
        public void Instance_Registered_With_Transient_Scope_Is_Not_Disposed_On_Scope_Disposal()
        {
            var disposable = new BackgroundJobDependency();
            _container.Inject(disposable);
            var activator = CreateActivator();

            using (var scope = activator.BeginScope())
            {
                var instance = scope.Resolve(typeof(BackgroundJobDependency));
                Assert.Same(instance, disposable);
                Assert.False(((BackgroundJobDependency)instance).Disposed);
            }

            Assert.False(disposable.Disposed);
        }

        [Fact]
        public void Instance_Registered_With_Singleton_Scope_Is_Not_Disposed_On_Scope_Disposal()
        {
            var disposable = new BackgroundJobDependency();
            _container.Configure(expression => expression.For<BackgroundJobDependency>().Singleton().Use(disposable));
            var activator = CreateActivator();

            using (var scope = activator.BeginScope())
            {
                var instance = scope.Resolve(typeof(BackgroundJobDependency));
                Assert.Same(instance, disposable);
                Assert.False(((BackgroundJobDependency)instance).Disposed);
            }

            Assert.False(disposable.Disposed);
        }

        [Fact]
        public void In_BackgroundJobScope_Registers_Same_Service_Instance_For_The_Same_Scope_Instance()
        {
            _container.Configure(expression => expression.For<object>().BackgroundJobScoped().Use(() => new object()));
            var activator = CreateActivator();

            using (var scope = activator.BeginScope())
            {
                var instance1 = scope.Resolve(typeof(object));
                var instance2 = scope.Resolve(typeof(object));

                Assert.Same(instance1, instance2);
            }
        }

        [Fact]
        public void In_BackgroundJobScope_Registers_Different_Service_Instances_For_Different_Scope_Instances()
        {
            _container.Configure(expression => expression.For<object>().Use(() => new object()));
            var activator = CreateActivator();

            object instance1;
            using (var scope1 = activator.BeginScope()) instance1 = scope1.Resolve(typeof(object));
            object instance2;
            using (var scope2 = activator.BeginScope()) instance2 = scope2.Resolve(typeof(object));

            Assert.NotSame(instance1, instance2);
        }

        [Fact]
        public void Instance_Registered_With_BackgroundJobScope_Is_Disposed_On_Scope_Disposal()
        {
            BackgroundJobDependency disposable;
            _container.Configure(expression => expression.For<BackgroundJobDependency>().BackgroundJobScoped());
            var activator = CreateActivator();

            using (var scope = activator.BeginScope())
            {
                disposable = (BackgroundJobDependency)scope.Resolve(typeof(BackgroundJobDependency));
                Assert.False(disposable.Disposed);
            }

            Assert.True(disposable.Disposed);
        }

        [Fact]
        public void Instance_Registered_With_BackgroundJobScope_Is_Reused_For_Other_Objects()
        {
            _container.Configure(expression => expression.For<BackgroundJobDependency>().BackgroundJobScoped());
            var activator = CreateActivator();

            using (var scope = activator.BeginScope())
            {
                var instance = (TestJob)scope.Resolve(typeof(TestJob));
                Assert.Same(instance.BackgroundJobDependency, instance.SameDependencyObject.BackgroundJobDependency);
            }
        }

        [Fact]
        public void Instance_Registered_With_TransientScope_Is_Not_Reused_For_Other_Objects()
        {
            _container.Configure(expression => expression.For<UniqueDependency>().AlwaysUnique());
            var activator = CreateActivator();

            using (var scope = activator.BeginScope())
            {
                var instance = (TestJob)scope.Resolve(typeof(TestJob));
                Assert.NotSame(instance.UniqueDependency, instance.SameDependencyObject.UniqueDependency);
            }
        }

        [Fact]
        public void Use_StructureMapActivator_Passes_Correct_Activator()
        {
#pragma warning disable 618
            var configuration = new Mock<IBootstrapperConfiguration>();
            var container = new Mock<IContainer>();

            configuration.Object.UseStructureMapActivator(container.Object);
            configuration.Verify(bootstrapperConfiguration => bootstrapperConfiguration.UseActivator(It.IsAny<StructureMapJobActivator>()));
#pragma warning restore 618
        }

        private StructureMapJobActivator CreateActivator()
        {
            return new StructureMapJobActivator(_container);
        }
    }
}
