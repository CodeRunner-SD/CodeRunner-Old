namespace CodeRunner.Pipelines
{
    internal readonly struct ServiceItem
    {
        public ServiceItem(object value, string from)
        {
            Value = value;
            From = from;
        }

        public object Value { get; }

        public string From { get; }
    }
}
