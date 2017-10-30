csMACnz.SeaOrDew
================

[![License](http://img.shields.io/:license-mit-blue.svg)](http://csmacnz.mit-license.org)
[![NuGet](https://img.shields.io/nuget/v/csMACnz.SeaOrDew.svg)](https://www.nuget.org/packages/csMACnz.SeaOrDew)
[![NuGet](https://img.shields.io/nuget/dt/csMACnz.SeaOrDew.svg)](https://www.nuget.org/packages/csMACnz.SeaOrDew)

[![AppVeyor Build status](https://img.shields.io/appveyor/ci/MarkClearwater/SeaOrDew.svg)](https://ci.appveyor.com/project/MarkClearwater/SeaOrDew)
[![Coverage Status](https://coveralls.io/repos/github/csMACnz/SeaOrDew/badge.svg?branch=master)](https://coveralls.io/github/csMACnz/SeaOrDew?branch=master)

This is a helper library of interfaces and executors to Run Commands and Queries. This is based on ideas from [Uncle Bob on Architecture](https://8thlight.com/blog/uncle-bob/2012/08/13/the-clean-architecture.html)], focussing on a helper library for focusing on Use Cases. This library works well to support a Ports and Adapters (Hexagonal) Architecture approach.

To use
------

### AspNetCore Setup ###

The simplest way get going is register all Handlers from an Assembly in Startup:

``` cs
//Startup.cs
//public void ConfigureServices(IServiceCollection services)
services.AddSeaOrDewHandlers(options =>
{
    //Choose the Assembly that contains your I*Handler implementations
    options.LoadAllHandlersFromAssembly(typeof(Startup).GetTypeInfo().Assembly);
    options.UseLifetimeScope(ServiceLifetime.Scoped);
});
```

In your controllers, simply inject your handlers and call handle.

``` cs
public class MyController
{
    private readonly QueryHandler _queryHandler;
    private readonly CommandHandler = _commandHandler;

    public MyController(QueryHandler queryHandler, CommandHandler commandHandler)
    {
        _queryHandler = queryHandler;
        _commandHandler = commandHandler;
    }

    [HttpGet]
    public async Task<object> GetUsers(int id)
    {
        //Set your parameters for the usercase
        LoadUsersQuery query = new LoadUsersQuery();

        //If you are using the optional Interfaces, you can use the extensions with inference:
        //LoadUsersResult result = await _queryHandler.Handle(query);

        //If you are not using the interfaces, you can explicity declare the Query/Result types.
        LoadUsersResult result = await _queryHandler.Handle<LoadUsersQuery, LoadUsersResult>(query);

        return new
        {
            Users = result.Users
        };
    }

    [HttpPost("{id}")]
    public async Task<ResolvedRuleModel> CreateUser([FromBody]CreateUserModel model)
    {
        //Set your parameters for the usercase
        CreateUserCommand command = new CreateUserCommand();

        //If you are using the optional Interfaces, you can use the extensions with inference:
        //CreateUserCommandResult result = await _commandHandler.Handle(command);

        //IF you are using the CommandResult, but your own TError, you would explicitly use:
        //CreateUserCommandResult result = await _commandHandler.Handle<CreateUserCommand, CommandResult<HttpStatusCode>>(command);

        //If you are not using the interfaces, you can explicity declare the Query/Result types.
        CreateUserCommandResult result = await _commandHandler.Handle<CreateUserCommand, CommandResult<CommandError>>(command);

        return new
        {
            Users = result.Users
        };
    }

    [HttpPost("{id}/lock")]
    public async Task<ResolvedRuleModel> LockUserAccount()
    {
        //Set your parameters for the usercase
        LockUserAccountCommand command = new LockUserAccountCommand();

        //If you are using the optional Interfaces, you can use the extensions with inference:
        //LockUserAccountCommandResult result = await _commandHandler.Handle(command);

        //If you are not using the interfaces, you can explicity declare the Query/Result types.
        LockUserAccountCommandResult result = await _commandHandler.Handle<CreateUserCommand,LockUserAccountCommandResult>(command);

        return new
        {
            Users = result.Users
        };
    }
}
```

### Queries ###

Queries take arguments and return results. These use cases are the Query side
of CQRS, and should not expect to have side effects.

``` cs
public class LoadUsersQueryHandler : IQueryHandler<LoadUsersQuery, LoadUsersResult>
{
    public async Task<TestResult> Handle(TestQuery query)
    {
        List<User> users = new List<User>();

        //TODO: Actually Load Users

        return new LoadUsersQueryResult();
    }
}

//The IQuery interfact is optional, but using it lights up some extension method helpers
public class LoadUsersQuery: IQuery<LoadUsersResult>
{
    public bool IncludeArchived { get; set; }
}

public class LoadUsersResult {
    public List<User> Users { get; set; }
}
```

### Commands ###

Commands usually only need to notify if they succeeded or failed.
Failures are accompanied by an error message and perhaps an error code and exception as well.
This is the opinionated "pit of success" solution. (For advanced usage, see example 2 and 3)


Example1:

``` cs
//Implicit `CommandResult` result type, with `CommandError` payload
public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    public async Task<CommandResult<CommandError>> Handle(CreateUserCommand command)
    {
        //TODO: Actually do some work to execute the command

        if(SomeErrorConditionIsTrue)
        {
            //Errors Automatically cast from the result payload type to CommandResult
            return new CommandError(123, "Something went wrong");

            //you could also use the more verbose
            //return new CommandResult<CommandError>(new CommandError(123, "Something went wrong"));
        }

        //Helper "DSL" result for the success case
        return Handler.Success;

        //you could also use the more verbose
        //return CommandResult<CommandError>.Success

        //or you could add `using static csMACnz.SeaOrDew.Handler` and just
        //return Success;`
    }
}

//The IQuery interfact is optional, but using it lights up some extension method helpers
public class CreateUserCommand: ICommand
{
    //presumably a model or set of properties is configured here
}
```

Example2:

You don't have to use the CommandError type, you may wish to use your own `Problem.cs` type with your own codes and more error details. You can use the more detailed approach:

``` cs

//To use this, the handler still works if you use the `ICommand<TPRoblem>` on your command:
var result = await _commandHandler.Handle(new CreateUserCommandHandler());
if(!result.IsSuccess)
{
    var error = result.Problem;
    //...
}

//Implicit `CommandResult` result type, with `CommandError` payload
public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, HttpStatusCode>
{
    public async Task<CommandResult<HttpStatusCode>> Handle(CreateUserCommand command)
    {
        //TODO: Actually do some work to execute the command

        if(SomeErrorConditionIsTrue)
        {
            //Errors Automatically cast from the result payload type to CommandResult
            return HttpStatusCode.PreconditionFailed;

            //you could also use the more verbose
            //return new CommandResult<HttpStatusCode>(HttpStatusCode.PreconditionFailed);
        }

        //Helper "DSL" result for the success case
        return Handler.Success;

        //you could also use the more verbose
        //return CommandResult<HttpStatusCode>.Success

        //or you could add `using static csMACnz.SeaOrDew.Handler` and just
        //return Success;`
    }
}

//The IQuery interfact is optional, but using it lights up some extension method helpers
public class CreateUserCommand: ICommand<HttpStatusCode>
{
    //presumably a model or set of properties is configured here
}
```

`HttpStatusCode` could be any custom class, instead.

Example3:

The most advanced way to use this, is to have a completely custom response type:

``` cs

//To use this, the handler still works if you use the `ICustomCommand<TResult>` on your command:
var result = await _commandHandler.Handle(new LockUserAccountCommand());

//alternatively you can use
//var result = _commandHandler.Handle<LockUserAccountCommand, LockUserAccountCommandResult>(new LockUserAccountCommand());


//Explicit `LockUserAccountCommandResult` result type
public class LockUserAccountCommandHandler : ICustomCommandHandler<LockUserAccountCommand, LockUserAccountCommandResult>
{
    public async Task<LockUserAccountCommandResult> Handle(LockUserAccountCommand command)
    {
        //TODO: Actually do some work to execute the command

        //You are on your own with this one...
        return new LockUserAccountCommandResult();
    }
}


public class LockUserAccountCommand: ICustomCommand<LockUserAccountCommandResult>
{
    //custom command properties
}

public class LockUserAccountCommandResult
{
    //Custom properties, probably
}
```

Just to spell it out, the `ICustomCommand<TResult>`, `ICommand<TError>` and
`ICommand` interfaces are optional, but if you provide them, it lights up
type inference extensions on the handler. Reasons to leave these out include
wanting to avoid referencing this library from a contracts project, and wanting to force explicit usage of Handle<,> generic arguments.

[WARN]:If you leave these interfaces out, the extensions will assume you are using
CommandResult and CommandError default types unless you explicity set the
Generic types (`handler.Handle<TCommand, TResult>`)

It is recommended to test your command resolution from your IServiceProvider with unit and/or integration tests to make sure resolution will work as expected, with the correct types.