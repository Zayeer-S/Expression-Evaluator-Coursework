using System.Net;

static class VariableInputParser
{
    public static Dictionary<string, double> GetVariableValues(HashSet<string> variableNames)
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
                Console.WriteLine($"{varName} = ");
                var input = Console.ReadLine();

                if (double.TryParse(input, out var value))
                {
                    variables[varName] = value;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid number. Please try again");
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