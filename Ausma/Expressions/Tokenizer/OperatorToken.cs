namespace AusmaProgram.Expressions.Tokenizer
{
    public class OperatorToken : ExpressionToken
    {

        public OperatorToken()
            :base()
        {

        }

        public OperatorToken(TokenType type, int positionStart)
            :base()
        {
            this.Type = type;
            this.PositionStart = positionStart;
        }

        public TokenPriority Priority
        {
            get
            {
                switch (this.Type)
                {
                    case TokenType.PlusOperator:
                    case TokenType.MinusOperator:
                        return TokenPriority.PlusMinus;
                    case TokenType.MultiplyOperator:
                    case TokenType.DivideOperator:
                        return TokenPriority.MultiplyDivide;
                    case TokenType.AndOperator:
                        return TokenPriority.And;
                    case TokenType.OrOperator:
                        return TokenPriority.Or;
                    case TokenType.NotOperator:
                        return TokenPriority.Not;
                    case TokenType.LessOperator:
                    case TokenType.LessOrEqualOperator:
                    case TokenType.GreaterOrEqualOperator:
                    case TokenType.GreaterOperator:
                        return TokenPriority.Equality;
                    default:
                        return TokenPriority.None;
                }
            }
        }

    }
}