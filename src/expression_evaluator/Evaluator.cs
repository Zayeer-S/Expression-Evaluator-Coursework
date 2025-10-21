static class Evaluator
{
    public static double Evaluate(ExpressionTreeNode node, Dictionary<string, double> variables)
    {
        if (double.TryParse(node.Value, out var number))
        {
            return number;
        }

        if (Constants.PrecedenceMap().ContainsKey(node.Value))
        {
        }
        else if (char.IsLetter(node.Value[0]))
        {
            return variables[node.Value.ToLower()];
        }

        return node.Value switch
        {
            "+" => Evaluate(node.Left!, variables) + Evaluate(node.Right!, variables),
            "-" => Evaluate(node.Left!, variables) - Evaluate(node.Right!, variables),
            "*" => Evaluate(node.Left!, variables) * Evaluate(node.Right!, variables),
            "/" => Evaluate(node.Left!, variables) / Evaluate(node.Right!, variables),
            "^" => Math.Pow(Evaluate(node.Left!, variables), Evaluate(node.Right!, variables)),

            "UNARY_MINUS" => -Evaluate(node.Left!, variables),
            "UNARY_PLUS" => Evaluate(node.Left!, variables),

            "<" => Evaluate(node.Left!, variables) < Evaluate(node.Right!, variables) ? 1.0 : 0.0,
            "<=" => Evaluate(node.Left!, variables) <= Evaluate(node.Right!, variables) ? 1.0 : 0.0,
            ">" => Evaluate(node.Left!, variables) > Evaluate(node.Right!, variables) ? 1.0 : 0.0,
            ">=" => Evaluate(node.Left!, variables) >= Evaluate(node.Right!, variables) ? 1.0 : 0.0,
            "=" => Math.Abs(Evaluate(node.Left!, variables) - Evaluate(node.Right!, variables)) < 1e-10 ? 1.0 : 0.0,
            "!=" => Math.Abs(Evaluate(node.Left!, variables) - Evaluate(node.Right!, variables)) >= 1e-10 ? 1.0 : 0.0,

            "and" => (IsTrue(Evaluate(node.Left!, variables)) && IsTrue(Evaluate(node.Right!, variables))) ? 1.0 : 0.0,
            "or" => (IsTrue(Evaluate(node.Left!, variables)) || IsTrue(Evaluate(node.Right!, variables))) ? 1.0 : 0.0,
            "not" => IsTrue(Evaluate(node.Left!, variables)) ? 0.0 : 1.0,

            _ => throw new InvalidOperationException($"Unknown operator: {node.Value}")
        };
    }

    private static bool IsTrue(double value)
    {
        return Math.Abs(value) > 1e-10;
    }
}