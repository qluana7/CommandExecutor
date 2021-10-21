using System;
using System.Collections;

namespace CommandExecutor.Attributes
{
    /// <summary>
    /// The attribute that provides setting count of arguments.
    /// If this attribute set, parameter type must be array. (Not <see cref="IEnumerable"/>)
    /// But, the counts value set to <see cref="CountOption.Infinity"/> or -1,
    /// parameter type also can be string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ArgumentsCountAttribute : Attribute
    {
        /// <summary>
        /// Initialize object with <paramref name="counts"/>
        /// </summary>
        /// <param name="counts">The count of argument. You can set <see cref="CountOption.Infinity"/> or -1, which like params keyword</param>
        public ArgumentsCountAttribute(int counts)
        {
            this.Count = counts;
        }
        
        /// <summary>
        /// The count of argument.
        /// </summary>
        public int Count { get; }
    }
    
    /// <summary>
    /// Provides const value of -1 for <see cref="ArgumentsCountAttribute"/>
    /// </summary>
    public static class CountOption
    {
        /// <summary>
        /// Same as params.
        /// </summary>
        public const int Infinity = -1;
    }
}