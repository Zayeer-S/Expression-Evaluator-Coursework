using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography;
using System.Xml.Schema;

static class Tokenizer
{
    public static List<string> GetTokens(string expression)
    {
        var tokens = TokenizeExpression(expression);

        if (ValidateTokens(tokens))
        {
            return tokens;
        }
        else
        {
            return [];
        }
    }

    private static List<string> TokenizeExpression(string expression)
    {
        var tokens = new List<string>();
        var i = 0;

        while (i < expression.Length)
        {
            var currentChar = expression[i];

            if (currentChar == ' ')
            {
                i++;
                continue;
            }

            if (i + 1 < expression.Length)
            {
                string twoChar = expression.Substring(i, 2);

                if (twoChar == "<=" || twoChar == ">=" || twoChar == "!=")
                {
                    tokens.Add(twoChar);
                    i += 2;
                    continue;
                }
            }

            if (char.IsLetter(currentChar))
            {
                tokens.Add(GetKeywordTokens(expression, ref i));
                continue;
            }

            if (char.IsDigit(currentChar))
            {
                tokens.Add(GetDigitTokens(expression, ref i));
                continue;
            }

            tokens.Add(currentChar.ToString());
            i++;
        }

        return BalanceParantheses(tokens);
    }

    #region TokenizeExpression Helpers

    private static string GetKeywordTokens(string expression, ref int i)
    {
        var startPos = i;

        while (i < expression.Length && char.IsLetter(expression[i]))
        {
            i++;
        }

        return expression[startPos..i].ToLower();    
    }

    private static string GetDigitTokens(string expression, ref int i)
    {
        var startPos = i;
        var hasDecimal = false;

        while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
        {
            if (expression[i] == '.')
            {
                if (hasDecimal)
                {
                    break;
                }
                hasDecimal = true;
            }
            i++;
        }
        return expression[startPos..i];
    }

    private static List<string> BalanceParantheses(List<string> tokens)
    {
        var openedCount = tokens.Count(t => t == "(");
        var closedCount = tokens.Count(t => t == ")");
        for (var j = 0; j < openedCount - closedCount; j++)
        {
            tokens.Add(")");
        }
        return tokens;
    }
    #endregion

    private static bool ValidateTokens(List<string> tokens)
    {
        if (tokens.Count == 0)
        {
            Console.WriteLine("Error: Expression cannot be empty");
            return false;
        }

        var LOGICAL_OPS = Constants.LogicalOperatorsMap().Keys;
        var COMPARISON_OPS = Constants.ComparisonOperatorsMap().Keys;
        var ADDITIVE_OPS = Constants.AdditiveOperatorsMap().Keys;
        var MULTIPLICATIVE_OPS = Constants.MultiplicativeOperatorsMap().Keys;
        var EXPONENTIAL_OPS = Constants.ExponentianOperatorsMap().Keys;

        var BINARY_OPS = new HashSet<string>(
            COMPARISON_OPS
            .Concat(ADDITIVE_OPS)
            .Concat(MULTIPLICATIVE_OPS)
            .Concat(EXPONENTIAL_OPS)
            .Concat(["and", "or"])
        );

        var UNARY_OPS = new HashSet<string> { "+", "-", "not" };
        var ALL_OPS = new HashSet<string>(BINARY_OPS.Concat(["not"]));

        var parenthesesCount = 0;
        foreach (var token in tokens)
        {
            if (token == "(")
            {
                parenthesesCount++;
            }
            if (token == ")")
            {
                parenthesesCount--;
                if (parenthesesCount < 0)
                {
                    Console.WriteLine("Error: Parantheses not balanced");
                    return false;
                }
            }
        }

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            var prevToken = i > 0 ? tokens[i - 1] : null;
            var nextToken = i < tokens.Count - 1 ? tokens[i + 1] : null;

            if (!IsValidToken(token, ALL_OPS))
            {
                Console.WriteLine($"Error: Invalid token '{token}'");
                return false;
            }

            if (IsNumber(token))
            {
                if (!IsValidNumber(token))
                {
                    Console.WriteLine($"Error: Invalid number format '{token}'");
                    return false;
                }

                if (nextToken != null && (IsNumber(nextToken) || IsVariable(nextToken)))
                {
                    Console.WriteLine($"Error: Missing operator between '{token}' and '{nextToken}'");
                    return false;
                }
            }

            if (IsVariable(token))
            {
                if (nextToken != null && (IsNumber(nextToken) || IsVariable(nextToken)))
                {
                    Console.WriteLine($"Error: Missing operator between '{token}' and '{nextToken}'");
                    return false;
                }
            }

            if (BINARY_OPS.Contains(token))
            {
                if (prevToken == null || prevToken == "(" || ALL_OPS.Contains(prevToken))
                {
                    Console.WriteLine($"Error: Binary operator '{token}' missing left operand");
                    return false;
                }
                if (nextToken == null || nextToken == ")")
                {
                    Console.WriteLine($"Error: Binary operator '{token}' missing right operand");
                    return false;
                }
            }

            if (token == "not")
            {
                if (nextToken == null || nextToken == ")")
                {
                    Console.WriteLine("Error: 'not' operator missing operand");
                    return false;
                }
            }

            if (token == "(")
            {
                if (nextToken != null && nextToken == ")")
                {
                    Console.WriteLine("Error: Empty parantheses '()'");
                    return false;
                }
                if (nextToken != null && BINARY_OPS.Contains(nextToken))
                {
                    Console.WriteLine($"Error: '(' cannot be followed by binary operator '{nextToken}'");
                    return false;
                }
            }

            if (token == ")")
            {
                if (nextToken != null && (IsNumber(nextToken) || IsVariable(nextToken)))
                {
                    Console.WriteLine($"Error: Missing operator between ')' and '{nextToken}'");
                    return false;
                }
            }
        }

        var firstToken = tokens[0];

        if (BINARY_OPS.Contains(firstToken) && firstToken != "+" && firstToken != "-")
        {
            Console.WriteLine($"Error: Expression cannot start with binary operator '{firstToken}'");
            return false;
        }

        var lastToken = tokens[tokens.Count - 1];
        if (ALL_OPS.Contains(lastToken))
        {
            Console.WriteLine($"Error: Expression cannot end with operator '{lastToken}'");
            return false;
        }

        return true;
    }

    #region  ValidateTokens Helpers
    private static bool IsValidToken(string token, HashSet<string> allOps)
    {
        if (token == "(" || token == ")")
        {
            return true;
        }
        if (allOps.Contains(token))
        {
            return true;
        }
        if (IsNumber(token))
        {
            return true;
        }
        if (IsVariable(token))
        {
            return true;
        }
        if (token == "≤" || token == "≥" || token == "≠" || token == "×" || token == "÷")
        {
            return true;
        }
        return false;
    }

    private static bool IsNumber(string token)
    {
        return double.TryParse(token, out _);
    }

    private static bool IsValidNumber(string token)
    {
        var decimalCount = token.Count(c => c == '.');
        if (decimalCount > 1)
        {
            return false;
        }

        if (token.StartsWith(".") || token.EndsWith("."))
        {
            return false;
        }

        return double.TryParse(token, out _);
    }

    private static bool IsVariable(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }
        if (!char.IsLetter(token[0]))
        {
            return false;
        }

        var keywords = new HashSet<string> { "and", "or", "not" };
        return !keywords.Contains(token.ToLower());
    }
    #endregion
}