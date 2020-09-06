namespace InvokeWhatever
{
    public class FuncSuccessResult : IInvokeResult
    {
        public FuncSuccessResult(object value)
        {
            Value = value;
        }

        public bool Success => true;

        public bool HasValue => true;

        public object Value { get; private set; }

        public string ResultDescription => $"Method invoked successfully with return value of type {Value.GetType().Name}";
    }
}
