using System.Reflection;

namespace InvokeWhatever
{
    public class MethodInvokeSuggestion
    {
        public MethodInvokeSuggestion(MethodBase member, int score, object[] arguments)
        {
            Member = member;
            Arguments = arguments;
            Score = score;
        }

        public MethodBase Member { get; private set; }
        public object[] Arguments { get; private set; }
        public int Score { get; private set; }
    }
}
