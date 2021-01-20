using System;
using System.Collections.Generic;
using AusmaProgram.Expressions.Evaluator;
using AusmaProgram.Expressions.Tokenizer;

namespace AusmaProgram.Expressions.Parser
{
    public class ExpressionParser
    {

        private List<ExpressionToken> tokens;

        private int currentIndex = 0;

        private int TokensCount
        {
            get
            {
                return tokens.Count;
            }
        }

        private ExpressionToken PreviousToken
        {
            get
            {
                if (currentIndex - 1 > 0 && TokensCount > 0)
                {
                    return tokens[currentIndex - 1];
                }
                else
                {
                    return null;
                }
            }
        }

        internal ExpressionResult Execute()
        {
            throw new NotImplementedException();
        }

        private ExpressionToken CurrentToken
        {
            get
            {
                if (currentIndex < TokensCount)
                {
                    return tokens[currentIndex];
                }
                return null;
            }
        }

        private ExpressionToken UpcomingToken => currentIndex + 1 < TokensCount ? tokens[currentIndex + 1] : null;

        private void NextToken()
        {
            currentIndex++;
        }

        private ExpressionFunction ParseFunction()
        {
            ExpressionFunction function = new ExpressionFunction();
            function.Name = CurrentToken.StringValue; // Add function Identifier
            int functionIndex = currentIndex + 2; // Skip function identifier and open parenthesis to parse function arguments

            // Loop while parenthesis of the main function aren't closed
            int parenthesisOpened = 1;
            // Current function argument tokens
            ExpressionFunctionArgument currentArgument = new ExpressionFunctionArgument();
            while (parenthesisOpened > 0 && functionIndex < TokensCount)
            {
                ExpressionToken currentToken = tokens[functionIndex];
                if (currentToken.Type == TokenType.Comma)
                {
                    if (currentArgument.ArgumentTokens.Count == 0)
                    {
                        throw new ArgumentException($"Function argument was expected, got {currentToken}");
                    }
                    function.Arguments.Add(currentArgument);
                    currentArgument = new ExpressionFunctionArgument();
                    functionIndex++;
                    continue;
                }
                if (currentToken.Type == TokenType.OpenParenthesis)
                {
                    parenthesisOpened++;
                }
                if (currentToken.Type == TokenType.CloseParenthesis)
                {
                    parenthesisOpened--;
                    if (parenthesisOpened == 0)
                    {
                        if (currentArgument.ArgumentTokens.Count == 0 && tokens[functionIndex-1].Type == TokenType.Comma)
                        {
                            throw new ArgumentException($"Function argument was expected, got {currentToken}");
                        }
                        function.Arguments.Add(currentArgument);
                        functionIndex++;
                        break;
                    }
                }
                currentArgument.ArgumentTokens.Add(currentToken);
                functionIndex++;
            }
            if (parenthesisOpened > 0)
            {
                throw new ArgumentException($"')' expected, got EOF");
            }
            currentIndex = functionIndex;
            return function;
        }

        private void ParseVariable()
        {
            NextToken();
        }

        private void ParseIdentifier()
        {
            // Function
            if (UpcomingToken != null && UpcomingToken.Type == TokenType.OpenParenthesis)
            {
                ParseFunction();
            }
            // Variable or constant
            else
            {
                ParseVariable();
            }
        }

        private void ParseIdentifiers()
        {
            while (currentIndex < TokensCount)
            {
                if (CurrentToken == null)
                {
                    break;
                }
                if (CurrentToken.Type == TokenType.Identifier)
                {
                    ParseIdentifier();
                }
            }
            currentIndex = 0;
        }

        private ExpressionToken ParseExpression(ExpressionToken lhs, TokenPriority priority)
        {

            while (true)
            {
                if (lhs.Type == TokenType.MinusOperator)
                {
                    lhs = ParseExpression(UpcomingToken, TokenPriority.UnaryMinus);
                    if (lhs is ValueToken)
                        lhs = -lhs;
                    else
                        throw new ArgumentException($"Unary minus is valid only for ValueToken!{lhs} is not a ValueToken");
                    NextToken();
                    break;
                }
                else if (lhs.Type == TokenType.NotOperator)
                {
                    lhs = ParseExpression(UpcomingToken, TokenPriority.Not);
                    if (lhs is ValueToken)
                        lhs = !lhs;
                    else
                        throw new ArgumentException($"Unary minus is valid only for ValueToken!{lhs} is not a ValueToken");
                    NextToken();
                    break;
                }
                break;
            }

            while (UpcomingToken is OperatorToken && (UpcomingToken as OperatorToken).Priority >= priority)
            {
                OperatorToken op = UpcomingToken as OperatorToken;
                NextToken();
                ExpressionToken rhs = UpcomingToken;
                NextToken();
                while (UpcomingToken is OperatorToken && (UpcomingToken as OperatorToken).Priority > op.Priority)
                {
                    rhs = ParseExpression(rhs, (UpcomingToken as OperatorToken).Priority);
                    NextToken();
                }
                switch (op.Type)
                {
                    case TokenType.PlusOperator:
                        lhs = lhs + rhs;
                        break;
                    case TokenType.MinusOperator:
                        lhs = lhs - rhs;
                        break;
                    case TokenType.DivideOperator:
                        lhs = lhs / rhs;
                        break;
                    case TokenType.MultiplyOperator:
                        lhs = lhs * rhs;
                        break;
                    case TokenType.EqualOperator:
                        lhs = ExpressionToken.AreTokensEqual(lhs, rhs);
                        break;
                    case TokenType.LessOperator:
                        lhs = lhs < rhs;
                        break;
                    case TokenType.GreaterOperator:
                        lhs = lhs > rhs;
                        break;
                    case TokenType.LessOrEqualOperator:
                        lhs = lhs <= rhs;
                        break;
                    case TokenType.GreaterOrEqualOperator:
                        lhs = lhs >= rhs;
                        break;
                    case TokenType.AndOperator:
                        lhs = lhs & rhs;
                        break;
                    case TokenType.OrOperator:
                        lhs = lhs | rhs;
                        break;
                }
            }
            return lhs;
        }

        public ExpressionParser(List<ExpressionToken> tokens)
        {
            this.tokens = tokens;
            //ParseIdentifiers();
            Console.WriteLine(ParseExpression(CurrentToken, TokenPriority.None));
        }

    }
}