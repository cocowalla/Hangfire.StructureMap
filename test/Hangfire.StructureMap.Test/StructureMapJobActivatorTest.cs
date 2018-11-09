using System;
using FakeItEasy;
using Shouldly;
using StructureMap;
using Xunit;

namespace Hangfire.StructureMap.Test
{
    public class StructureMapJobActivatorTest
    {
        private readonly IContainer container;

        public StructureMapJobActivatorTest()
        {
            this.container = new Container();
        }

        [Fact]
        public void Ctor_Should_Throw_When_Container_Is_Null()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Should.Throw<ArgumentNullException>(() => new StructureMapJobActivator(null));
        }

        [Fact]
        public void Class_Is_Based_On_JobActivator()
        {
            var activator = CreateActivator();

            // ReSharper disable once IsExpressionAlwaysTrue
            var isJobActivator = activator is JobActivator;
            isJobActivator.ShouldBeTrue();
        }

        [Fact]
        public void ActivateJob_Resolves_Instance_Using_StructureMap()
        {
            var dependency = new BackgroundJobDependency();
            this.container.Inject(dependency);
            var activator = CreateActivator();

            var result = activator.ActivateJob(typeof(BackgroundJobDependency));
            result.ShouldBe(dependency);
        }

        [Fact]
        public void Container_Scoped_Instance_Is_Disposed_When_Job_Scope_Is_Disposed()
        {
            this.container.Configure(c => c.For<BackgroundJobDependency>().ContainerScoped());

            BackgroundJobDependency disposable;
            using (var scope = BeginJobScope())
            {
                disposable = (BackgroundJobDependency)scope.Resolve(typeof(BackgroundJobDependency));

                disposable.Disposed.ShouldBeFalse();
            }

            // Now the scope is disposed, dependencies should be too
            disposable.Disposed.ShouldBeTrue();
        }

        [Fact]
        public void Singleton_Scoped_Instance_Is_Not_Disposed_When_Job_Scope_Is_Disposed()
        {
            var disposable = new BackgroundJobDependency();
            this.container.Configure(c => c.For<BackgroundJobDependency>().Singleton().Use(disposable));

            using (var scope = BeginJobScope())
            {
                var instance = scope.Resolve(typeof(BackgroundJobDependency)) as BackgroundJobDependency;

                instance.ShouldBe(disposable);
                instance.Disposed.ShouldBeFalse();
            }

            // Singletons should live on after the scope is disposed
            disposable.Disposed.ShouldBeFalse();
        }

        [Fact]
        public void Transient_Scoped_Instance_Is_Disposed_When_Job_Scope_Is_Disposed()
        {
            this.container.Configure(c => c.For<BackgroundJobDependency>().Use(() => new BackgroundJobDependency()));

            BackgroundJobDependency disposable;
            using (var scope = BeginJobScope())
            {
                disposable = scope.Resolve(typeof(BackgroundJobDependency)) as BackgroundJobDependency;

                disposable.ShouldNotBeNull();
                disposable.Disposed.ShouldBeFalse();
            }

            // Now the scope is disposed, dependencies should be too
            disposable.Disposed.ShouldBeTrue();
        }

        [Fact]
        public void AlwaysUnique_Scoped_Instance_Is_Disposed_When_Job_Scope_Is_Disposed()
        {
            this.container.Configure(c => c.For<BackgroundJobDependency>().Use(() => new BackgroundJobDependency()).AlwaysUnique());

            BackgroundJobDependency disposable;
            using (var scope = BeginJobScope())
            {
                disposable = scope.Resolve(typeof(BackgroundJobDependency)) as BackgroundJobDependency;

                disposable.ShouldNotBeNull();
                disposable.Disposed.ShouldBeFalse();
            }

            // Now the scope is disposed, dependencies should be too
            disposable.Disposed.ShouldBeTrue();
        }

        /// <summary>
        /// Injecting an existing object into the Container makes it a de facto singleton (or at least, it's up to
        /// the injector to manage its lifecycle)
        /// <seealso cref="http://structuremap.github.io/registration/existing-objects/"/>
        /// </summary>
        [Fact]
        public void Implicitly_Singleton_Scoped_Instance_Is_Not_Disposed_When_Job_Scope_Is_Disposed()
        {
            var existingInstance = new BackgroundJobDependency();
            this.container.Configure(c => c.For<BackgroundJobDependency>().Use(existingInstance));

            using (var scope = BeginJobScope())
            {
                var disposable = scope.Resolve(typeof(BackgroundJobDependency)) as BackgroundJobDependency;

                disposable.ShouldBe(existingInstance);
                disposable.Disposed.ShouldBeFalse();
            }
            
            existingInstance.Disposed.ShouldBeFalse();
        }

        [Fact]
        public void Container_Scoped_Instances_Are_Not_Reused_Between_Different_Job_Scopes()
        {
            this.container.Configure(c => c.For<object>().Use(() => new object()).ContainerScoped());

            object instance1;
            using (var scope1 = BeginJobScope())
            {
                instance1 = scope1.Resolve(typeof(object));
            }

            object instance2;
            using (var scope2 = BeginJobScope())
            {
                instance2 = scope2.Resolve(typeof(object));
            }

            instance1.ShouldNotBe(instance2);
        }

        [Fact]
        public void Container_Scoped_Instance_Is_Reused_Within_Same_Job_Scope()
        {
            this.container.Configure(c => c.For<BackgroundJobDependency>().ContainerScoped());

            using (var scope = BeginJobScope())
            {
                var instance = (TestJob)scope.Resolve(typeof(TestJob));

                instance.BackgroundJobDependency.ShouldBe(instance.SameDependencyObject.BackgroundJobDependency);
            }
        }

        [Fact]
        public void AlwaysUnique_Scoped_Instance_Is_Not_Reused_Within_Same_Job_Scope()
        {
            this.container.Configure(c => c.For<UniqueDependency>().AlwaysUnique());

            using (var scope = BeginJobScope())
            {
                var instance = (TestJob)scope.Resolve(typeof(TestJob));

                instance.UniqueDependency.ShouldNotBe(instance.SameDependencyObject.UniqueDependency);
            }
        }

        private JobActivatorScope BeginJobScope()
        {
            var activator = CreateActivator();
#if NET452
#pragma warning disable CS0618 // Type or member is obsolete
            return activator.BeginScope();
#pragma warning restore CS0618 // Type or member is obsolete
#else
            return activator.BeginScope(null);
#endif
        }

#if NET452
        [Fact]
        public void Bootstrapper_Use_StructureMapActivator_Passes_Correct_Activator()
        {
#pragma warning disable 618
            var configuration = A.Fake<IBootstrapperConfiguration>();

            configuration.UseStructureMapActivator(this.container);

            A.CallTo(() => configuration.UseActivator(A<StructureMapJobActivator>.That.IsNotNull())).MustHaveHappened();
#pragma warning restore 618
        }
#endif

        private StructureMapJobActivator CreateActivator() => new StructureMapJobActivator(this.container);
    }
}
