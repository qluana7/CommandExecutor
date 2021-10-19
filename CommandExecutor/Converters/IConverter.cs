using System;

namespace CommandExecutor
{
    public interface IConverter
    { }

    public interface IConverter<T> : IConverter
    {
        T Convert(string value);
    }
}