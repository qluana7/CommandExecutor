using System;

namespace CommandExecutor.Converters
{
    /// <summary>
    /// The base converter interface without generic
    /// </summary>
    public interface IConverter
    { }
    
    /// <summary>
    /// The converter interface with generic.
    /// We will update that it can add custom converter later.
    /// </summary>
    /// <typeparam name="T">The generic type for convert</typeparam>
    public interface IConverter<T> : IConverter
    {
        T Convert(string value);
    }
}