class Program
{
    private static void Main(string[] args)
    {
        var precedenceMap = Constants.PrecedenceMap();

        List<string> normalizedTokens;
        while (true)
        {
            var expression = Parser.GetExpression();
            var tokens = Tokenizer.GetTokens(expression);

            if (TokenValidator.ValidateTokens(tokens, precedenceMap))
            {
                normalizedTokens = NormalizeTokens.Normalize(tokens);
                break;
            }
        }

        var postfix = ShuntingYard.InfixToPostfix(normalizedTokens, precedenceMap);
        var tree = ExpressionTree.BuildTree(postfix, precedenceMap, out var variableNames);

        do
        {
            var variables = VariableInputParser.GetVariableValues(variableNames);
            var result = Evaluator.Evaluate(tree, variables);
            OutputFormatter.PrintResult(result);
        }
        while (VariableInputParser.PromptForReEvaluation());
    }
}