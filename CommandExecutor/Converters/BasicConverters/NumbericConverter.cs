namespace CommandExecutor.Converters.BasicConverters
{
    internal class SbyteConvert : IConverter<sbyte>
    {
        public sbyte Convert(string value)
            => sbyte.Parse(value);
    }

    internal class ByteConverter : IConverter<byte>
    {
        public byte Convert(string value)
            => byte.Parse(value);
    }

    internal class ShortConverter : IConverter<short>
    {
        public short Convert(string value)
            => short.Parse(value);
    }
    
    internal class UshortConverter : IConverter<ushort>
    {
        public ushort Convert(string value)
            => ushort.Parse(value);
    }

    internal class IntConverter : IConverter<int>
    {
        public int Convert(string value)
            => int.Parse(value);
    }

    internal class UintConverter : IConverter<uint>
    {
        public uint Convert(string value)
            => uint.Parse(value);
    }

    internal class Longconverter : IConverter<long>
    {
        public long Convert(string value)
            => long.Parse(value);
    }

    internal class UlongConverter : IConverter<ulong>
    {
        public ulong Convert(string value)
            => ulong.Parse(value);
    }

    internal class FloatConverter : IConverter<float>
    {
        public float Convert(string value)
            => float.Parse(value);
    }

    internal class DoubleConverter : IConverter<double>
    {
        public double Convert(string value)
            => double.Parse(value);
    }

    internal class DecimalConverter : IConverter<decimal>
    {
        public decimal Convert(string value)
            => decimal.Parse(value);
    }

    internal class CharConverter : IConverter<char>
    {
        public char Convert(string value)
            => char.Parse(value);
    }

    internal class BoolConverter : IConverter<bool>
    {
        public bool Convert(string value)
            => bool.Parse(value);
    }
}