Hangfire.StructureMap
=====================

[![Windows Build Status](https://ci.appveyor.com/api/projects/status/github/cocowalla/Hangfire.StructureMap?svg=true)](https://ci.appveyor.com/project/ionx-solutions/Hangfire.StructureMap)
[![Linux Build status](https://api.travis-ci.org/cocowalla/Hangfire.StructureMap.svg)](https://travis-ci.org/cocowalla/Hangfire.StructureMap)
[![NuGet](https://img.shields.io/nuget/v/Hangfire.StructureMap.svg)](https://www.nuget.org/packages/Hangfire.StructureMap)

This package provides [StructureMap](http://structuremap.github.io/) support for [Hangfire](http://hangfire.io), allowing nested StructureMap containers to resolve job type instances and their dependencies, and to manage the lifetime of resolved instances.

Getting started
---------------

Install the [Hangfire.StructureMap](https://www.nuget.org/packages/Hangfire.StructureMap) package from NuGet:

```powershell
Install-Package Hangfire.StructureMap
```

To configure Hangfire to use StructureMap, configure your container and call the `IGlobalConfiguration` extension method, `UseStructureMapActivator`:

```csharp
var container = new Container();
// container.Configure...

GlobalConfiguration.Configuration.UseStructureMapActivator(container);
```

After configuration, when jobs are started a StructureMap-based implementation of the `JobActivator` class is used to resolve job type instances and all of their dependencies.

Object Lifecycles
-----------------

*Hangfire.StructureMap* doesn't rely on a specific object lifecycle - you can configure your dependencies as `Singleton`, `ContainerScoped`, `Transient`, `AlwaysUnique` or `ThreadLocal` as normal.

*Hangfire.StructureMap* creates a [*nested container*](http://structuremap.github.io/the-container/nested-containers/) for each job execution, so using `ContainerScoped` will scope dependency lifetimes to that of the job.

```csharp
container.For<IRepository>().ContainerScoped().Use<GenericRepository>();
```

The nested container is disposed when jobs ends, and all dependencies that implement the `IDisposable` interface are also automatically disposed  (`Singleton` scoped instances are of course an exception).
