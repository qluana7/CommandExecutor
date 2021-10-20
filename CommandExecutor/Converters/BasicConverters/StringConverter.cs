namespace CommandExecutor.Converters.BasicConverters
{
    internal class StringConverter : IConverter<string>
    {
        public string Convert(string value)
            => value;
    }
}