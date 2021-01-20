using System;
using System.Collections.Generic;
using AusmaProgram.Expressions.Parser;

namespace AusmaProgram.Expressions.Tokenizer
{
    public class ExpressionTokenizer
    {

        public string expression;
        private int currentPosition;
        private List<ExpressionToken> expressionTokens = new List<ExpressionToken>();

        private char CurrentChar => IsEndOfExpression() ? '\0' : expression[currentPosition];

        public int ExpressionLength => expression.Length;

        private bool IsEndOfExpression()
        {
            return currentPosition >= ExpressionLength;
        }

        private bool IsOperator()
        {
            return CurrentChar == '+'
                || CurrentChar == '-'
                || CurrentChar == '*'
                || CurrentChar == '/';
        }

        private bool IsComparisonOperator()
        {
            return CurrentChar == '=';
        }

        private void NextChar()
        {
            if (!IsEndOfExpression())
                currentPosition++;
        }

        private void ParseNumber()
        {
            string stringNumber = String.Empty;
            ValueToken currentToken = new ValueToken
            {
                PositionStart = currentPosition,
                Type = TokenType.IntegerValue
            };
            while (char.IsDigit(CurrentChar))
            {
                stringNumber += CurrentChar;
                NextChar();
            }
            if (CurrentChar == '.')
            {
                currentToken.Type = TokenType.DecimalValue;
                stringNumber += CurrentChar;
                NextChar();
                if (!char.IsDigit(CurrentChar))
                {
                    throw new ArgumentException($"Unexpected '{CurrentChar}' at position {currentPosition}.\nDigit was expected.");
                }
                while (char.IsDigit(CurrentChar))
                {
                    stringNumber += CurrentChar;
                    NextChar();
                }
            }
            currentToken.SystemValue = stringNumber;
            expressionTokens.Add(currentToken);
            NextToken();
        }

        private void ParseOperator()
        {
            OperatorToken currentToken = new OperatorToken
            {
                PositionStart = currentPosition
            };
            switch (CurrentChar)
            {
                case '+':
                    currentToken.Type = TokenType.PlusOperator;
                    break;
                case '-':
                    currentToken.Type = TokenType.MinusOperator;
                    break;
                case '*':
                    currentToken.Type = TokenType.MultiplyOperator;
                    break;
                case '/':
                    currentToken.Type = TokenType.DivideOperator;
                    break;
            }
            expressionTokens.Add(currentToken);
            NextChar();
            NextToken();
        }

        private void ParseParenthesis()
        {
            OperatorToken currentToken = new OperatorToken
            {
                PositionStart = currentPosition
            };
            switch (CurrentChar)
            {
                case '(':
                    currentToken.Type = TokenType.OpenParenthesis;
                    break;
                case ')':
                    currentToken.Type = TokenType.CloseParenthesis;
                    break;
            }
            expressionTokens.Add(currentToken);
            NextChar();
            NextToken();
        }

        private void ParseLessComparisonOperator()
        {
            OperatorToken currentToken = new OperatorToken
            {
                PositionStart = currentPosition
            };
            NextChar();
            if (CurrentChar == '=')
            {
                currentToken.Type = TokenType.LessOrEqualOperator;
                NextChar();
            }
            else
            {
                currentToken.Type = TokenType.LessOperator;
            }
            expressionTokens.Add(currentToken);
            NextToken();
        }

        private void ParseGreaterComparisonOperator()
        {
            OperatorToken currentToken = new OperatorToken
            {
                PositionStart = currentPosition
            };
            NextChar();
            if (CurrentChar == '=')
            {
                currentToken.Type = TokenType.GreaterOrEqualOperator;
                NextChar();
            }
            else
            {
                currentToken.Type = TokenType.GreaterOperator;
            }
            expressionTokens.Add(currentToken);
            NextToken();
        }

        public void ParseEqualComparisonOperator()
        {
            OperatorToken currentToken = new OperatorToken
            {
                PositionStart = currentPosition,
                Type = TokenType.EqualOperator
            };
            expressionTokens.Add(currentToken);
            NextChar();
            NextToken();
        }

        public void ParseString()
        {
            ValueToken currentToken = new ValueToken
            {
                PositionStart = currentPosition,
                Type = TokenType.StringValue
            };
            NextChar();
            string stringValue = String.Empty;
            while (!(CurrentChar == '\'' || CurrentChar == '\0'))
            {
                stringValue += CurrentChar;
                NextChar();
            }
            if (CurrentChar == '\0')
            {
                throw new ArgumentException("String value must be enclosed by ' character!");
            }
            currentToken.SystemValue = stringValue;
            expressionTokens.Add(currentToken);
            NextChar();
            NextToken();
        }

        private void ParseIdentifier()
        {
            string identifier = string.Empty;
            int positionStart = currentPosition;
            while (CurrentChar == '_' || char.IsLetterOrDigit(CurrentChar))
            {
                identifier += CurrentChar;
                NextChar();
            }
            switch (identifier)
            {
                case "AND":
                    expressionTokens.Add(new OperatorToken(TokenType.AndOperator, positionStart));
                    NextToken();
                    break;
                case "OR":
                    expressionTokens.Add(new OperatorToken(TokenType.OrOperator, positionStart));
                    NextToken();
                    break;
                case "NOT":
                    expressionTokens.Add(new OperatorToken(TokenType.NotOperator, positionStart));
                    NextToken();
                    break;
                case "TRUE":
                case "FALSE":
                    ValueToken booleanToken = new ValueToken
                    {
                        Type = TokenType.BooleanValue,
                        StringValue = identifier,
                        SystemValue = (identifier == "TRUE"),
                        PositionStart = positionStart
                    };
                    expressionTokens.Add(booleanToken);
                    NextToken();
                    break;
                case "LOE":
                    expressionTokens.Add(new OperatorToken(TokenType.LessOrEqualOperator, positionStart));
                    NextToken();
                    break;
                case "GOE":
                    expressionTokens.Add(new OperatorToken(TokenType.GreaterOrEqualOperator, positionStart));
                    NextToken();
                    break;
                default:
                    ExpressionToken identifierToken = new ExpressionToken();
                    identifierToken.StringValue = identifier;
                    identifierToken.Type = TokenType.Identifier;
                    identifierToken.PositionStart = positionStart;
                    expressionTokens.Add(identifierToken);
                    NextToken();
                    break;
            }
        }

        private void ParseComma()
        {
            ExpressionToken currentToken = new ExpressionToken();
            currentToken.Type = TokenType.Comma;
            currentToken.PositionStart = currentPosition;
            expressionTokens.Add(currentToken);
            NextChar();
            NextToken();
        }

        public void NextToken()
        {
            if (IsEndOfExpression())
            {
                return;
            }
            else if (char.IsDigit(CurrentChar))
            {
                ParseNumber();
            }
            else if (IsOperator())
            {
                ParseOperator();
            }
            else if (CurrentChar == '(' || CurrentChar == ')')
            {
                ParseParenthesis();
            }
            else if (CurrentChar == '<')
            {
                ParseLessComparisonOperator();
            }
            else if (CurrentChar == '>')
            {
                ParseGreaterComparisonOperator();
            }
            else if (CurrentChar == '=')
            {
                ParseEqualComparisonOperator();
            }
            else if (CurrentChar == '\'')
            {
                ParseString();
            }
            else if (char.IsLetter(CurrentChar) || CurrentChar == '_')
            {
                ParseIdentifier();
            }
            else if (CurrentChar == ',')
            {
                ParseComma();
            }
            else if (CurrentChar == ' ')
            {
                NextChar();
                NextToken();
            }
            else
            {
                throw new ArgumentException($"Unexpected '{CurrentChar}' at position {currentPosition}.");
            }
        }

        public List<ExpressionToken> GetTokens()
        {
            NextToken();
            ExpressionParser parser = new ExpressionParser(expressionTokens);
            return expressionTokens;
        }

        public ExpressionTokenizer(string expression)
        {
            this.expression = expression;
            currentPosition = 0;
        }

    }
}