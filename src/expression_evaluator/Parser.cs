static class Parser 
{
    public static string GetExpression()
    {
        while (true) 
        {
            var expression = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(expression))
            {
                return expression.Trim();
            }
            else
            {
                Console.WriteLine("Expression cannot be empty");
            }
        }
    }
}