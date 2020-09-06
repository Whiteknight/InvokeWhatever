using System;

namespace InvokeWhatever
{
    public class ActionSuccessResult : IInvokeResult
    {
        public bool Success => true;

        public bool HasValue => false;

        public object Value => throw new InvalidOperationException("Method invoke was successfull but return type was void. There is no result value.");

        public string ResultDescription => "Method invoked successfully with no return value";
    }
}
