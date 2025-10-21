static class OutputFormatter
{
    public static void PrintResult(double result)
    {
        Console.WriteLine($"\nResult: {result}");
    }

    public static void PrintTokens(List<string> tokens, string label)
    {
        Console.WriteLine($"\n{label}:");
        Console.WriteLine(string.Join(" ", tokens));
    }

    public static void PrintVariables(Dictionary<string, double> variables)
    {
        if (variables.Count == 0)
        {
            return;
        }

        Console.WriteLine("\nCurrent variable values:");
        foreach (var kvp in variables.OrderBy(v => v.Key))
        {
            Console.WriteLine($"{kvp.Key} = {kvp.Value}");
        }
    }
}