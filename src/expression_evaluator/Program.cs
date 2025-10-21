class Program
{
    private static void Main(string[] args)
    {
        var precedenceMap = Constants.PrecedenceMap();

        Console.WriteLine("Enter expression: ");
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

        var isBooleanExpression = Classifier.IsBooleanExpression(normalizedTokens);
        var hasNumericVariables = Classifier.HasNumericVariables(normalizedTokens);

        do
        {
            try
            {
                var variables = VariableInput.GetVariableValues(variableNames, !hasNumericVariables);
                var result = Evaluator.Evaluate(tree, variables);
                OutputFormatter.PrintResult(result, isBooleanExpression);
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"\n{ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\n{ex.Message}");
            }
        }
        while (VariableInput.PromptForReEvaluation());
    }
}