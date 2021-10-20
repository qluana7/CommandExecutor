﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandExecutor;
using CommandExecutor.Attributes;

namespace Test
{
    class Program
    {
        static void Main(string[] _)
        {
            Executor executor = new(new ExecutorConfiguration() {
                IgnoreCase = true,
                GetPrivateMethod = true
            });
            
            executor.RegisterCommands<Commands>();
            
            executor.Execute("ping hello true");
            executor.Execute("arg 3 5 2 asdf");
            executor.Execute("inf inf this is inf arg test");
            executor.Execute("nonparam");
            executor.Execute("private");
        }
    }
    
    public class Commands : ICommandModule
    {
        public void Ping(string s, bool t)
        {
            Console.WriteLine(t ? s : "null");
        }
        
        public void Arg([ArgumentsCount(3)] int[] i, string s)
        {
            Console.WriteLine("[" + string.Join(',', i) + "]");
            Console.WriteLine(s);
        }
        
        public void Inf(string s, [ArgumentsCount(CountOption.Infinity)] string args)
        {
            Console.WriteLine(s);
            Console.WriteLine(string.Concat(args));
        }
        
        public void NonParam()
        {
            Console.WriteLine("This is method that has no parameters");
        }
        
        private void Private()
        {
            Console.WriteLine("This is private method");
        }
    }
}