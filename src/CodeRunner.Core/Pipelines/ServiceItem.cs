namespace CodeRunner.Pipelines
{
    public readonly struct ServiceItem
    {
        public ServiceItem(object value, string source)
        {
            Value = value;
            Source = source;
        }

        public object Value { get; }

        public string Source { get; }
    }
}
