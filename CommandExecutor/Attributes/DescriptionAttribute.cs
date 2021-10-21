using System;

namespace CommandExecutor.Attributes
{
    /// <summary>
    /// The attribute that provides set description for method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// Initialize object with description.
        /// </summary>
        /// <param name="desc">The description of method</param>
        public DescriptionAttribute(string desc)
        {
            this.Description = desc;
        }
        
        /// <summary>
        /// The description of method.
        /// </summary>
        public string Description { get; }
    }
}