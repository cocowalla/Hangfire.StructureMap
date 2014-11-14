Hangfire.StructureMap
======

[Hangfire](http://hangfire.io) background job activator based on the [StructureMap](http://structuremap.net) IoC container. 
It allows you to use instance methods of classes that define parametrized constructors:

```csharp
public class EmailService
{
	private DbContext context;
	private IEmailSender sender;
	
	public EmailService(DbContext context, IEmailSender sender)
	{
		this.context = context;
		this.sender = sender;
	}
	
	public void Send(int userId, string message)
	{
		var user = this.context.Users.Get(userId);
		this.sender.Send(user.Email, message);
	}
}	

// Somewhere in the code
BackgroundJob.Enqueue<EmailService>(x => x.Send(1, "Hello, world!"));
```

Installation
--------------

Hangfire.StructureMap is available as a NuGet Package. Type the following
command into NuGet Package Manager Console window to install it:

```
Install-Package Hangfire.StructureMap
```

Usage
------

The package provides an extension method for [OWIN bootstrapper](http://docs.hangfire.io/en/latest/users-guide/getting-started/owin-bootstrapper.html):

```csharp
app.UseHangfire(config =>
{
	var container = new Container(x =>
		// Configure your container here
	);
	
	config.UseStructureMapActivator(container);
});
```

In order to use the library outside of web application, set the static `JobActivator.Current` property:

```csharp
var container = new Container(x =>
	// Configure your container here
);

JobActivator.Current = new StructureMapActivator(container);
```

HTTP Request warnings
-----------------------

Services registered with `HttpContextScoped()` directive **will be unavailable** during job activation, you should re-register 
these services without this hint.

`HttpContext.Current` is also **not available** during the job performance. 
Don't use it!

Notes
------

This is pretty much a conversion of [HangFire.Ninject](https://github.com/HangfireIO/Hangfire.Ninject) for [StructureMap](http://structuremap.net).