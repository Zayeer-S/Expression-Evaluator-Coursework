static class Classifier
{
    public static bool IsBooleanExpression(List<string> tokens)
    {
        var logicalOps = Constants.LogicalOperatorsMap().Keys;
        var comparisonOps = Constants.ComparisonOperatorsMap().Keys;

        return tokens.Any(t => logicalOps.Contains(t) || comparisonOps.Contains(t));
    }
    
    public static bool HasNumericVariables(List<string> tokens)
    {
        var comparisonOps = Constants.ComparisonOperatorsMap().Keys;
        var arithmeticOps = Constants.AdditiveOperatorsMap().Keys
            .Concat(Constants.MultiplicativeOperatorsMap().Keys)
            .Concat(Constants.ExponentianOperatorsMap().Keys);
        
        return tokens.Any(t => comparisonOps.Contains(t) || arithmeticOps.Contains(t));
    }
}