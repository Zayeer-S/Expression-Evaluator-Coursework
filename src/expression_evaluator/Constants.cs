static class Constants {
    public static IReadOnlyDictionary<string, int> PrecedenceMap()
    {
        return new Dictionary<string, int>
        {
            { "or", 1 },
            { "and", 2 },
            { "not", 3 },
            { "<", 4 },
            { "≤", 4 },
            { "<=", 4 },
            { ">", 4 },
            { "≥", 4 },
            { ">=", 4 },
            { "=", 4 },
            { "≠", 4 },
            { "!=", 4 },
            { "+", 5 },
            { "-", 5 },
            { "*", 6 },
            { "×", 6 },
            { "/", 6 },
            { "÷", 6 },
            { "^", 7 },
            { "UNARY_MINUS", 8 },
            { "UNARY_PLUS", 8 }
        };
    }
}