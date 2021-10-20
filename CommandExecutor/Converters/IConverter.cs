using System;

namespace CommandExecutor.Converters
{
    public interface IConverter
    { }

    public interface IConverter<T> : IConverter
    {
        T Convert(string value);
    }
}