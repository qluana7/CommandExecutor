using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandExecutor.Converters.BasicConverters;

namespace CommandExecutor.Converters
{
    public class ArgumentsConverter
    {
        public Dictionary<Type, IConverter> Converters { get; }

        public ArgumentsConverter()
        {
            this.Converters = new Dictionary<Type, IConverter>
            {
                [typeof(sbyte)] = new SbyteConvert(),
                [typeof(byte)] = new ByteConverter(),
                [typeof(short)] = new ShortConverter(),
                [typeof(ushort)] = new UshortConverter(),
                [typeof(int)] = new IntConverter(),
                [typeof(uint)] = new UintConverter(),
                [typeof(long)] = new Longconverter(),
                [typeof(ulong)] = new UlongConverter(),
                [typeof(float)] = new FloatConverter(),
                [typeof(double)] = new DoubleConverter(),
                [typeof(decimal)] = new DecimalConverter(),
                [typeof(char)] = new CharConverter(),
                [typeof(string)] = new StringConverter(),
                [typeof(bool)] = new BoolConverter()
            };
        }

        public object Convert<T>(string value)
        {
            var t = typeof(T);
            if (!Converters.Keys.Contains(t))
                throw new InvalidCastException("There is no converter for given type.");
            else if (Converters[t] is not IConverter<T>)
                throw new InvalidOperationException("Invalid converter for given type");
            else
                return (Converters[t] as IConverter<T>).Convert(value);
        }

        public object Convert(string value, Type t)
        {
            var ti = typeof(ArgumentsConverter).GetTypeInfo();
            var m = ti.DeclaredMethods.FirstOrDefault(l => l.ContainsGenericParameters && l.Name == "Convert" && !l.IsStatic);
            return m.MakeGenericMethod(t).Invoke(this, new object[] { value });
        }
    }
}