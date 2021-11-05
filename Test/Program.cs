#pragma warning disable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandExecutor;
using CommandExecutor.Attributes;
using CommandExecutor.Structures;

namespace Test
{
    class CustomAttribute : CommandCheckAttribute
    {
        public override bool Check(Command cmd)
        {
            return false;
        }
    }

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
                GetPrivateMethod = true,
                IncludeStaticMethod = false
            });
            
            Executor.RegisterCommands<ScopeCommands>();
            
            Executor.Execute("Test");
            
            Executor.RegisterCommands<Commands>();
            Executor.RegisterCommands<SpecialCommands>();
            
            Executor.Execute("ping hello true");
            Executor.Execute("args 3 5 2 asdf");
            Executor.Execute("inf inf this is inf arg test");
            Executor.Execute("nonparam");
            Executor.Execute("private");
            
            Executor.Execute("non");
            Executor.Execute("false");
        }
    }
    
    public class ScopeCommands : CommandModule
    {
        public override void BeforeExecute() =>
            Console.WriteLine("Before");

        public override void AfterExecute() =>
            Console.WriteLine("After");

        public override bool CheckExecute(Command cmd) {
            Console.WriteLine(cmd.Name); return true;
        }
        
        [Command("Test")/*, CustomAttribute*/]
        public void Test() => Console.WriteLine("GOOD!");
    }
    
    public class SpecialCommands : CommandModule
    {
        public override bool CheckExecute(Command cmd)
        {
            if (cmd.Name == "False") return false;
            return true;
        }
        
        [Command]
        public void Non()
        {
            Console.WriteLine("Execute check passed.");
        }
        
        [Command]
        public void False()
        {
            Console.WriteLine("BUG");
        }
    }
    
    public class Commands : CommandModule
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
