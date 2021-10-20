using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandExecutor.Attributes;
using CommandExecutor.Converters;
using CommandExecutor.Exceptions;
using CommandExecutor.Structures;

namespace CommandExecutor
{
    public class Executor
    {
        public Executor(ExecutorConfiguration configuration)
        {
            this._commands = new List<ICommandModule>();
            this.Configuration = configuration;
        }
        
        private readonly List<ICommandModule> _commands;
        
        internal ExecutorOptions Options => this.Configuration.ToOptions();
        
        public ExecutorConfiguration Configuration { private get; set; }
        
        public ICommandModule[] Commands => _commands.ToArray();
        
        public void RegisterCommands<T>() where T : ICommandModule, new()
            => this._commands.Add(new T());
        
        public void UnregisterCommands<T>() where T : ICommandModule, new()
        {
            ICommandModule mod = this._commands.FirstOrDefault(l => l is T);
            if (mod == null) throw new NullReferenceException("There's no registered class matching with give class");
            this._commands.Remove(mod);
        }
        
        public void Execute(string content)
        {
            string[] c = content.Split(' ');
            Execute(c[0], c.Skip(1).ToArray());
        }
        
        internal void Execute(string command, string[] args)
        {
            MethodInfo method = null;
            ICommandModule module = null;
            
            for (int i = 0; i < this._commands.Count; i++)
            {
                MethodInfo info = _commands[i].GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | (Options.HasFlag(ExecutorOptions.GetPrivateMethod) ? BindingFlags.NonPublic : 0))
                    .FirstOrDefault(l => Options.HasFlag(ExecutorOptions.IgnoreCase) ?
                                (l.Name.ToLower() == command.ToLower()) :
                                (l.Name == command));
                                
                if (info == null) continue;
                
                method = info;
                module = _commands[i];
                
                break;
            }
            
            if (method == null || module == null)
                throw new CommandNotFoundException(new CommandRequest()
                {
                    Command = command,
                    Arguments = args
                });
            
            object[] paras = ConvertParameter(method, args);
            
            method.Invoke(module, paras.Any() ? paras.ToArray() : null);
        }
        
        private object[] ConvertParameter(MethodInfo method, string[] args)
        {
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
                    if (!Options.HasFlag(ExecutorOptions.IgnoreExtraArguments))
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
