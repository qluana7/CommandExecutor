# CommandExecutor

### Feature
- Executor register all method that inherit ICommandModule.
- Can execute commands with only string.
- String will automatically change to parameter type.
- Provides set arguments count for array.

### Example
First, declare executor class.
```cs
// in Main method

Executor executor = new(new ExecutorConfiguration() {
    IgnoreCase = true,
    IgnoreExtraArguments = false,
    GetPrivateMethod = false
});
```

Second, create class that inheriting ICommandModule.
```cs
// outisde of Main class

public class BasicCommand : ICommandModule
{
    // You can set command name and alias.
    [Command]
    public void Hello(string person)
    {
        Console.WriteLine($"Hello, {person}!");
    }
}
```

Third, register class.
```cs
// in Main method

executor.RegisterCommands<BasicCommand>();
```

Last, execute commands with string.
```cs
// in Main method

executor.Execute("Hello World");
```

Then, it execute 'Hello' method with arguments "World"

[See Example Code](/Test/Program.cs)

### Current Staute of New Features
- [X] Execute with string and object[] for arguments
  - [X] Add Command class that has information of command and can execute.
  - [X] Add FindCommand method which returns Command class
- [X] Checking attribute whether execute with custom attribute
  - [X] Class execute checking system (virautl method)
  - [X] Make execute check attribute


- [ ] Support override method   // Postpone to next version

### Contact
- Discord : 단비#1004
- Gmail : emailluana@gmail.com

### Compile Information
Framework version : Standard 2.1 / .NET 5.0

### Reference
**Referred the source of the following repo**
- [DSharpPlus.CommandsNext](https://github.com/DSharpPlus/DSharpPlus/tree/master/DSharpPlus.CommandsNext)
