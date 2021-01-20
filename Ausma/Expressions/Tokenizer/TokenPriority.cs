namespace AusmaProgram.Expressions.Tokenizer
{
    public enum TokenPriority
    {
        None = 0,
        Concat = 1,
        Or = 2,
        And = 3,
        Not = 4,
        Equality = 5,
        PlusMinus = 6,
        MultiplyDivide = 7,
        UnaryMinus = 8
    }
}