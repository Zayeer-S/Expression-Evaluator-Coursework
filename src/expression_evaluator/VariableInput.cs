static class VariableInput
{
    public static Dictionary<string, double> GetVariableValues(HashSet<string> variableNames, bool isBooleanExpression)
    {
        var variables = new Dictionary<string, double>();

        if (variableNames.Count == 0)
        {
            return variables;
        }

        Console.WriteLine("\nEnter variable values:");

        foreach (var varName in variableNames.OrderBy(v => v))
        {
            while (true)
            {
                if (isBooleanExpression)
                {
                    Console.Write($"{varName} (true/false) = ");
                    var input = Console.ReadLine()?.Trim().ToLower();

                    if (input == "true" || input == "t" || input == "1")
                    {
                        variables[varName] = 1.0;
                        break;
                    }
                    else if (input == "false" || input == "f" || input == "0")
                    {
                        variables[varName] = 0.0;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 'true' or 'false'.");
                    }
                }
                else
                {
                    Console.Write($"{varName} = ");
                    var input = Console.ReadLine();

                    if (double.TryParse(input, out var value))
                    {
                        variables[varName] = value;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid number. Please try again.");
                    }
                }
            }
        }

        return variables;
    }

    public static bool PromptForReEvaluation()
    {
        Console.WriteLine("\nEvaluate with different values? (y/n):");

        while (true)
        {
            var response = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(response) && response.Length > 0)
            {
                return response == "y" || response == "ye" || response == "yes";
            }
            Console.WriteLine("Enter valid response (y/n):");
        }
    }
}