namespace CommandExecutor
{
    public class SbyteConvert : IConverter<sbyte>
    {
        public sbyte Convert(string value)
            => sbyte.Parse(value);
    }

    public class ByteConverter : IConverter<byte>
    {
        public byte Convert(string value)
            => byte.Parse(value);
    }

    public class ShortConverter : IConverter<short>
    {
        public short Convert(string value)
            => short.Parse(value);
    }
    
    public class UshortConverter : IConverter<ushort>
    {
        public ushort Convert(string value)
            => ushort.Parse(value);
    }

    public class IntConverter : IConverter<int>
    {
        public int Convert(string value)
            => int.Parse(value);
    }

    public class UintConverter : IConverter<uint>
    {
        public uint Convert(string value)
            => uint.Parse(value);
    }

    public class Longconverter : IConverter<long>
    {
        public long Convert(string value)
            => long.Parse(value);
    }

    public class UlongConverter : IConverter<ulong>
    {
        public ulong Convert(string value)
            => ulong.Parse(value);
    }

    public class FloatConverter : IConverter<float>
    {
        public float Convert(string value)
            => float.Parse(value);
    }

    public class DoubleConverter : IConverter<double>
    {
        public double Convert(string value)
            => double.Parse(value);
    }

    public class DecimalConverter : IConverter<decimal>
    {
        public decimal Convert(string value)
            => decimal.Parse(value);
    }

    public class CharConverter : IConverter<char>
    {
        public char Convert(string value)
            => char.Parse(value);
    }

    public class BoolConverter : IConverter<bool>
    {
        public bool Convert(string value)
            => bool.Parse(value);
    }
}