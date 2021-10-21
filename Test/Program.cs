using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandExecutor;
using CommandExecutor.Attributes;

namespace Test
{
    class Program
    {
        public Executor Executor { get; private set; }
        
        static void Main()
        {
            var prog = new Program();
            prog.Run();
        }
        
        void Run()
        {
            Executor = new(new ExecutorConfiguration() {
                IgnoreCase = true,
                IgnoreExtraArguments = false,
                GetPrivateMethod = false
            });
            
            Executor.RegisterCommands<Commands>();
            
            Executor.Execute("ping hello true");
            Executor.Execute("args 3 5 2 asdf");
            Executor.Execute("inf inf this is inf arg test");
            Executor.Execute("nonparam");
            Executor.Execute("private");
        }
    }
    
    public class Commands : ICommandModule
    {
        [Command("Ping")]
        public void Ping(string s, bool t)
        {
            Console.WriteLine(t ? s : "null");
        }
        
        [Command("args")]
        public void Arg([ArgumentsCount(3)] int[] i, string s)
        {
            Console.WriteLine("[" + string.Join(',', i) + "]");
            Console.WriteLine(s);
        }
        
        [Command("in", new string[] {"inf", "ina"})]
        public void Inf(string s, [ArgumentsCount(CountOption.Infinity)] string args)
        {
            Console.WriteLine(s);
            Console.WriteLine(string.Concat(args));
        }
        
        [Command]
        public void NonParam()
        {
            Console.WriteLine("This is method that has no parameters");
        }
        
        [Command(true)]
        private void Private()
        {
            Console.WriteLine("This is private method");
        }
    }
}
