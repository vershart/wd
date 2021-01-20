namespace AusmaProgram.Expressions.Tokenizer
{
    public enum TokenType
    {
        None,
        PlusOperator,
        MinusOperator,
        MultiplyOperator,
        DivideOperator,
        OpenParenthesis,
        CloseParenthesis,
        NotOperator,
        AndOperator,
        OrOperator,
        LessOperator,
        LessOrEqualOperator,
        EqualOperator,
        GreaterOrEqualOperator,
        GreaterOperator,
        StringValue,
        NumericValue, // Remove this
        IntegerValue,
        DecimalValue,
        BooleanValue,
        Identifier,
        Comma
    }
}