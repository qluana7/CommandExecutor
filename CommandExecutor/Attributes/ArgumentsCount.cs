using System;

namespace CommandExecutor.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ArgumentsCountAttribute : Attribute
    {
        public ArgumentsCountAttribute(int counts)
        {
            this.Count = counts;
        }
        
        public int Count { get; }
    }
    
    public static class CountOption
    {
        public const int Infinity = -1;
    }
}