Hangfire.StructureMap
=====================

[StructureMap](http://structuremap.github.io/) support for [Hangfire](http://hangfire.io). Provides an implementation of the `JobActivator` class and plugin family expression extensions, allowing you to use StructureMap IoC container to **resolve job type instances** as well as **control the lifetime** of the related dependencies.

Installation
--------------

*Hangfire.StructureMap* is available as a NuGet Package. Type the following command into NuGet Package Manager Console window to install it:

```
Install-Package Hangfire.StructureMap
```

Usage
------

The package provides an extension method for the `IGlobalConfiguration` interface, so you can enable StructureMap integration using the `GlobalConfiguration` class.

```csharp
var container = new Container();
// container.Configure...

GlobalConfiguration.Configuration.UseStructureMapActivator(container);
```

After invoking the methods above, StructureMap-based implementation of the `JobActivator` class will be used to resolve job type instances and all their dependencies during the background processing.

### Re-using Dependencies

Sometimes it is necessary to re-use instances that are already created, such as database connection, unit of work, etc. Thanks to the [custom lifecycles based on ILifecycle](http://structuremap.github.io/object-lifecycle/custom-lifecycles/) feature of StructureMap, you are able to do this by implementing a custom lifecycle.

*Hangfire.StructureMap* relies on the built-in StructureMap lifecycle, `ContainerLifecycle`, to allow you to limit the object scope to the **current background job processing**, just call the `LifecycleIs` extension method in your create plugin family expression logic:

```csharp
container.For<IDatabase>().LifecycleIs<ContainerLifecycle>().Use<Database>();
```

### Deterministic Disposal

All the dependencies that implement the `IDisposable` interface are disposed as soon as current background job is performed, but **only when they were registered with the `ContainerLifecycle` lifecycle**. For other cases, StructureMap itself is responsible for disposing instances, so please read about the [object lifecycles](http://structuremap.github.io/object-lifecycle/).

For most typical cases, you can call the `LifecycleIs<>` method on a job plugin family expression and implement the `Dispose` method that will dispose all the dependencies manually:

```csharp
public class JobClass : IDisposable
{
    private readonly Dependency _dependency;

    public JobClass(Dependency dependency) { /* ... */ }

    public Dispose()
    {
        _dependency.Dispose();
    }
}
```

```csharp
container.For<JobClass>().LifecycleIs<ContainerLifecycle>();
```

HTTP Request warnings
-----------------------

Services registered with `HttpContextScoped()` directive or the `HttpContextLifecycle` lifecycle **will be unavailable** during job activation, you should re-register these services without this hint.

**`HttpContext.Current` is also not available during the job performance. Don't use it!**

Notes
------

This package takes advantage of [nested containers](http://structuremap.github.io/the-container/nested-containers/).  The `StructureMapJobActivator` creates a nested container for the `StructureMapDependencyScope` which is based on Hangfire's `JobActivatorScope`
