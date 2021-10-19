using System;
using System.Reflection;

namespace CommandExecutor
{
    public class EnumConverter<T> : IConverter<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        public T Convert(string value)
        {
            if (typeof(T).GetTypeInfo().IsEnum)
                return Enum.Parse<T>(value);
            else
                throw new InvalidCastException("Cannot convert as enum with non-enum value");
        }
    }
}