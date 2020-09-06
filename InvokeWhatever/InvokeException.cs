namespace InvokeWhatever
{
    [System.Serializable]
    public class InvokeException : System.Exception
    {
        public InvokeException() { }
        public InvokeException(string message) : base(message) { }
        public InvokeException(string message, System.Exception inner) : base(message, inner) { }
        protected InvokeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
