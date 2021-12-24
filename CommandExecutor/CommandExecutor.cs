using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommandExecutor.Attributes;
using CommandExecutor.Converters;
using CommandExecutor.EventArgs;
using CommandExecutor.Exceptions;
using CommandExecutor.Structures;

namespace CommandExecutor
{
    /// <summary>
    /// The class that provides
    /// regisetering commands with <see cref="RegisterCommands{T}"/>
    /// and executing commands with <see cref="Execute(string)"/>.
    /// Set commands execute options with <see cref="Configuration"/>
    /// </summary>
    public class Executor
    {
        /// <summary>
        /// Initialize <see cref="Executor"/> object
        /// </summary>
        public Executor() : this(new ExecutorConfiguration()) { }
        
        /// <summary>
        /// Initialize <see cref="Executor"/> object with <see cref="ExecutorConfiguration"/>
        /// </summary>
        public Executor(ExecutorConfiguration configuration)
        {
            this._commods = new List<CommandModule>();
            this._commands = new List<string>();
            configuration.ExceptionConfiguration._client = this;
            this.Configuration = configuration;
        }
        
        /// <summary>
        /// The event raised on error when command execute
        /// </summary>
        public event EventHandler<CommandExceptionEventArgs> CommandErrored;
        
        /// <summary>
        /// The event raised before method invoke.
        /// It raises after all checking execute.
        /// </summary>
        public event EventHandler<CommandEventArgs> CommandExecuted;
        
        internal bool IsEventNull() => CommandErrored == null;
        
        internal void RaiseCommandErrored(CommandExceptionEventArgs e) => CommandErrored?.Invoke(e.InnerCommand, e);
        
        internal void RaiseCommandExecuted(CommandEventArgs e) => CommandExecuted?.Invoke(e.Command, e);
        
        private readonly List<CommandModule> _commods;
        
        private readonly List<string> _commands;
        
        /// <summary>
        /// Can modify configuration
        /// </summary>
        public ExecutorConfiguration Configuration { get; set; }
        
        /// <summary>
        /// Get commands that registered in executor
        /// </summary>
        public CommandModule[] CommandModules => _commods.ToArray();
        
        /// <summary>
        /// The command name array that registered in executor
        /// </summary>
        public string[] Commands => _commands.ToArray();
        
        private const BindingFlags FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        
        /// <summary>
        /// <see cref="GetAllCommands"/> <see langword="async"/> version.
        /// </summary>
        /// <returns></returns>
        public async Task<Command[]> GetAllCommandsAsync() =>
            await Task.Run(GetAllCommands);
        
        /// <summary>
        /// Return all Command object array that registered in executor
        /// </summary>
        /// <returns>Command array that registered</returns>
        public Command[] GetAllCommands()
        {
            Command[] cmds = new Command[_commands.Count];
            
            for (int i = 0; i < _commands.Count; i++)
                cmds[i] = FindCommand(_commands[i]);
            
            return cmds;
        }
        
        /// <summary>
        /// Get commands in specific class
        /// </summary>
        /// <typeparam name="T">The class that contains commands</typeparam>
        public Command[] GetCommands<T>() where T : CommandModule
        {
            var names = GetCommandsName(typeof(T)).ToArray();
            
            Command[] cmds = new Command[names.Length];
            
            for (int i = 0; i < cmds.Length; i++) {
                cmds[i] = FindCommand(names[i]);
            }
            
            return cmds;
        }
        
        private IEnumerable<string> GetCommandsName(Type t) =>
            t.GetMethods(Configuration.IncludeStaticMethod ? FLAGS | BindingFlags.Static : FLAGS)
                .Select(l => (l, l.GetCustomAttribute<CommandAttribute>()))
                .Where(l => l.Item2 != null)
                .SelectMany(l =>
                    new string[] { string.IsNullOrWhiteSpace(l.Item2.Name) ? l.l.Name : l.Item2.Name }
                        .Concat(l.Item2.Alias ?? Array.Empty<string>()
                )
            );
        
        /// <summary>
        /// Register commands.
        /// The class to register must inherit <see cref="CommandModule"/>.
        /// This register all public methods (if <see cref="ExecutorConfiguration.GetPrivateMethod"/> set true, include private methods)
        /// which has <see cref="CommandAttribute"/>.
        /// Overload methods are not support yet. Command's name and alias must be unique.
        /// </summary>
        /// <typeparam name="T">The class to register. It must inherit <see cref="CommandModule"/></typeparam>
        /// <exception cref="RegisterException"/>
        public void RegisterCommands<T>() where T : CommandModule, new()
        {
            IEnumerable<string> regcmd = this._commods.SelectMany(l => GetCommandsName(l.GetType()));
            
            IEnumerable<string> cmd = GetCommandsName(typeof(T));
            
            if (cmd.Count() != cmd.Distinct().Count())
                throw new RegisterException(typeof(T), "Methods with duplicate names cannot be registered.");
            
            if (regcmd.Any(l => cmd.Contains(l)))
                throw new RegisterException(typeof(T), "Methods with duplicate names cannot be registered.");

            _commands.AddRange(cmd);
            
            this._commods.Add(new T());
        }
        
        /// <summary>
        /// Unregister commands.
        /// </summary>
        /// <exception cref="RegisterException"/>
        /// <typeparam name="T">The class to unregister. It must inherit <see cref="CommandModule"/></typeparam>
        public void UnregisterCommands<T>() where T : CommandModule, new()
        {
            CommandModule mod = this._commods.FirstOrDefault(l => l is T);
            if (mod == null) throw new RegisterException(null, "There's no registered class matching with give class");
            this._commods.Remove(mod);
            
            var cmds = GetCommandsName(typeof(T)).ToArray();
            for (int i = 0; i < cmds.Length; i++)
                if (_commands.Contains(cmds[i]))
                    _commands.Remove(cmds[i]);
        }
        
        /// <summary>
        /// Execute commands with string.
        /// <paramref name="content"/> will split with space.
        /// Then, run <see cref="Execute(string, string[])"/>.
        /// First argument will be first index of splited content
        /// And Second argument will be remaining string of splited content
        /// </summary>
        /// <param name="content">The string to execute</param>
        public void Execute(string content)
        {
            string[] c = content.Split(' ');
            Execute(c[0], c.Length == 1 ? null : c.Skip(1).ToArray());
        }
        
        /// <summary>
        /// Execute commands with commands and arguments.
        /// This will find command with <paramref name="command"/>,
        /// and run command with <paramref name="args"/>
        /// </summary>
        /// <param name="command">The string to search command</param>
        /// <param name="args">The string to pass as arguments</param>
        public void Execute(string command, string[] args) =>
            FindCommand(command).Execute(args == null ? null : string.Join(' ', args));
        
        /// <summary>
        /// Run command with command name object.
        /// DO NOT USE MANUALLY
        /// </summary>
        /// <param name="command">The string to search command</param>
        /// <param name="args">The object array to pass as arguments</param>
        
        // Because of it's risk, We decided to change to internal.
        internal void Execute(string command, object[] args) =>
            FindCommand(command).Execute(args);
        
        /// <summary>
        /// Find command with string.
        /// Search in all registered commands.
        /// </summary>
        /// <param name="cmd">The string to find</param>
        /// <returns>The class that has information of command</returns>
        public Command FindCommand(string cmd)
        {
            (MethodInfo method, CommandModule module) = FindCommand(cmd, null);
            
            Executor client = this;
            return new Command(ref client, module, method);
        }
        
        internal (MethodInfo Method, CommandModule Module) FindCommand(string command, string _)
        {
            MethodInfo method = null;
            CommandModule module = null;
            
            for (int i = 0; i < _commods.Count; i++)
            {
                bool igncase = Configuration.IgnoreCase;
                
                IEnumerable<MethodInfo> infos = _commods[i]
                            .GetType().GetMethods(FLAGS)
                            .Where(l => l.GetCustomAttributes().FirstOrDefault(l => l is CommandAttribute) != null);
                
                MethodInfo info = infos
                                    .FirstOrDefault(l => {
                                        CommandAttribute cmdat = l.GetCustomAttribute<CommandAttribute>();
                                        if (!Configuration.GetPrivateMethod && cmdat.IsPrivate)
                                            return false;
                                        
                                        return new string[] { cmdat.Name ?? l.Name }.Concat(cmdat.Alias ?? Array.Empty<string>())
                                                    .Select(l => igncase ? l.ToLower() : l)
                                                    .Contains(igncase ? command.ToLower() : command);
                                    });
                                
                if (info == null) continue;
                
                method = info;
                module = _commods[i];
                
                break;
            }
            
            if (method == null || module == null)
                throw new CommandNotFoundException(command);
            
            return (method, module);
        }
        
        internal object[] ConvertParameter(MethodInfo method, string[] args)
        {
            if (args == null) args = Array.Empty<string>();
            
            ParameterInfo[] paras = method.GetParameters();
                        
            int pac = paras.Aggregate(0, (t ,n) => {
                ArgumentsCountAttribute acat = n.GetCustomAttribute<ArgumentsCountAttribute>();
                
                if (acat == null) return t + 1;
                
                if (acat.Count == CountOption.Infinity)
                {
                    if (Array.IndexOf(paras, n) != paras.Length - 1)
                        throw new ArgumentException("The parameter that has ArgumentsCountAttribute that has infinity value must be last argument");
                    
                    return args.Length;
                }
                
                if (!(n.ParameterType.IsArray && n.ParameterType != typeof(string)))
                    throw new InvalidCastException("The type of parameter that has ArgumentsCountAttribute that not infinity value must be Array");
                
                return t + acat.Count;
            });
            
            if (pac != args.Length)
            {
                if (pac > args.Length)
                    throw new ArgumentException("Given arguments count is less then required parameters count");
                else
                {
                    if (!Configuration.IgnoreExtraArguments)
                        throw new ArgumentException("Given arguments count is more then required parameters count");
                    else
                        args = args.Take(pac).ToArray();
                }
            }
            
            List<object> arguments = new List<object>();
            
            ArgumentsConverter converter = new ArgumentsConverter();
            
            int current = 0;
            
            for (int i = 0; i < paras.Length; i++)
            {
                ArgumentsCountAttribute acat = paras[i].GetCustomAttribute<ArgumentsCountAttribute>();
                int c = acat?.Count ?? 1;
                
                if (i == paras.Length - 1)
                {
                    if (c == CountOption.Infinity)
                    {
                        if (paras[i].ParameterType == typeof(string))
                            arguments.Add(string.Join(' ', args.Skip(i)));
                        else if (paras[i].ParameterType == typeof(Enumerable))
                            arguments.Add(args.Skip(i).Select(l => converter.Convert(l, paras[i].ParameterType)));
                        else
                            throw new InvalidCastException("The type of parameter that has ArgumentsCountAttribute that not infinity value must be Array or IEnumerable");
                        
                        break;
                    }
                }
                
                ArraySegment<string> seg = new ArraySegment<string>(args, current, c);
                
                IEnumerable<object> objs = seg.Select(l => converter.Convert(l, paras[i].ParameterType.IsArray ? paras[i].ParameterType.GetElementType() : paras[i].ParameterType));
                
                arguments.Add(objs.Count() == 1 ? objs.ElementAt(0) : Utils.CastArray(objs, paras[i].ParameterType.GetElementType()));
                
                current += c;
            }
            
            return arguments.ToArray();
        }
    }
}
