using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace InvokeWhatever
{
    public class MethodSorter
    {
        // No match, this is not an acceptable candidate
        private const int NoMatch = -1;
        private readonly MethodSortOptions _options;

        public MethodSorter(MethodSortOptions options)
        {
            _options = options;
        }

        public MethodInvokeSuggestion GetBestMethod(IEnumerable<MethodBase> methods, object[] availableArguments)
        {
            //var argsLookup = availableArguments
            //    .GroupBy(arg => arg.GetType())
            //    .ToDictionary(byType => byType.Key, byType => byType.ToArray());
            var suggestions = methods
                .Select(method => SuggestBestArgumentList(method, availableArguments))
                .Where(suggestion => suggestion != null)
                .ToList();

            if (suggestions.Count == 0)
                return null;

            var bestSuggestion = suggestions.Aggregate((a, b) => a.Score > b.Score ? a : b);
            return bestSuggestion;
        }

        private MethodInvokeSuggestion SuggestBestArgumentList(MethodBase member, object[] availableArguments)
        {
            var parameters = member.GetParameters();
            if (parameters.Length == 0)
                return new MethodInvokeSuggestion(member, _options.ParameterlessScore, new object[0]);

            var usedArgs = new bool[availableArguments.Length];
            int totalScore = 0;
            var arguments = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var (score, value) = GetArgumentValue(availableArguments, usedArgs, parameter);
                if (score == NoMatch)
                    return null;
                arguments[i] = value;
                totalScore += score;
            }

            return new MethodInvokeSuggestion(member, totalScore, arguments);
        }

        private (int score, object value) GetArgumentValue(object[] availableArguments, bool[] usage, ParameterInfo parameter)
        {
            Debug.Assert(availableArguments.Length == usage.Length);
            if (availableArguments.Length == 0)
            {
                if (parameter.HasDefaultValue)
                    return (_options.NoMatchDefaultValue, parameter.DefaultValue);
                return (NoMatch, null);
            }

            // TODO: We should match all "object" parameters last, from whatever arguments are left unassigned?

            for (int i = 0; i < availableArguments.Length; i++)
            {
                if (usage[i])
                    continue;

                var argument = availableArguments[i];
                var argumentType = argument.GetType();
                if (argumentType == parameter.ParameterType)
                {
                    usage[i] = true;
                    return (_options.ExactTypeMatch, argument);
                }
                if (parameter.ParameterType == typeof(object))
                {
                    usage[i] = true;
                    if (argumentType.IsValueType)
                        return (_options.BoxingTypeMatch, argument);
                    return (_options.ObjectTypeMatch, argument);
                }
                if (parameter.ParameterType.IsAssignableFrom(argumentType))
                {
                    usage[i] = true;
                    if (argumentType.IsValueType && !parameter.ParameterType.IsValueType)
                        return (_options.BoxingTypeMatch, argument);
                    return (_options.TypeMatch, argument);
                }
                // TODO: Optionally, if the two types are numeric and we can cast between them, do it
                // TODO: Optionally, if the target type is a string, can we stringify something?
            }
            if (parameter.HasDefaultValue)
                return (_options.NoMatchDefaultValue, parameter.DefaultValue);
            return (NoMatch, null);
        }
    }
}
