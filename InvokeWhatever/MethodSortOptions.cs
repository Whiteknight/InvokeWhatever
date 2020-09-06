namespace InvokeWhatever
{
    public class MethodSortOptions
    {
        public static MethodSortOptions Default { get; } = new MethodSortOptions();

        public MethodSortOptions()
        {
            ParameterlessScore = 1;
            ExactTypeMatch = 10;
            TypeMatch = 9;
            BoxingTypeMatch = 5;
            ObjectTypeMatch = 2;
            NoMatchDefaultValue = 1;
        }

        // There are no arguments, but there is a parameterless member variant. This match is 
        // acceptable but has the lowest possible value
        public int ParameterlessScore { get; set; }

        // We have a perfect typewise match for an argument->parameter. 
        public int ExactTypeMatch { get; set; }

        // We have a nearly-perfect typewise match, the member is requesting a type which is
        // assignable from the argument type. This is good, but not as good as a perfect match
        public int TypeMatch { get; set; }

        // We have a match where the argument value is a primative or struct value, and it can be
        // assigned to the parameter through boxing. This is an acceptable match, but more costly
        // than a regular assignment
        public int BoxingTypeMatch { get; set; }

        // The parameter is type "object", so anything matches there, and we'll just stuff in
        // whatever arguments are available.
        public int ObjectTypeMatch { get; set; }

        // We do not have a matching argument, but the parameter has a default value we can use.
        public int NoMatchDefaultValue { get; set; }
    }
}
