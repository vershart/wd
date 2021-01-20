using System;

namespace AusmaProgram.Expressions.Tokenizer
{
    public class ExpressionToken
    {

        public string StringValue { get; set; }

        public TokenType Type { get; set; }

        public int PositionStart { get; set; }

        public int PositionEnd { get; set; }

        public static ExpressionToken operator +(ExpressionToken et1, ExpressionToken et2)
        {
            if (!(et1 is ValueToken vt1))
            {
                throw new ArgumentException($"+ operator is valid only for ValueTokens.\n{et1} is not a ValueToken");
            }
            if (!(et2 is ValueToken vt2))
            {
                throw new ArgumentException($"+ operator is valid only for ValueTokens.\n{et2} is not a ValueToken");
            }

            if (vt1.Type == vt2.Type && vt1.Type == TokenType.IntegerValue)
            {
                return new ValueToken(Convert.ToInt64(vt1.SystemValue) + Convert.ToInt64(vt2.SystemValue), TokenType.IntegerValue);
            }
            else if ((vt1.Type == vt2.Type && vt1.Type == TokenType.DecimalValue) ||
                    (vt1.Type == TokenType.DecimalValue && vt2.Type == TokenType.IntegerValue) ||
                    (vt1.Type == TokenType.IntegerValue && vt2.Type == TokenType.DecimalValue))
            {
                return new ValueToken(Convert.ToDecimal(vt1.SystemValue) + Convert.ToDecimal(vt2.SystemValue), TokenType.DecimalValue);
            }
            else
            {
                return new ValueToken(vt1.SystemValue.ToString() + vt2.SystemValue.ToString(), TokenType.StringValue);
            }
        }

        public static ExpressionToken operator -(ExpressionToken et1, ExpressionToken et2)
        {
            if (!(et1 is ValueToken vt1))
            {
                throw new ArgumentException($"- operator is valid only for ValueTokens.\n{et1} is not a ValueToken");
            }
            if (!(et2 is ValueToken vt2))
            {
                throw new ArgumentException($"- operator is valid only for ValueTokens.\n{et2} is not a ValueToken");
            }

            if (vt1.Type == vt2.Type && vt1.Type == TokenType.IntegerValue)
            {
                return new ValueToken(Convert.ToInt64(vt1.SystemValue) - Convert.ToInt64(vt2.SystemValue), TokenType.IntegerValue);
            }
            else if (vt1.Type == TokenType.DecimalValue || vt2.Type == TokenType.DecimalValue)
            {
                return new ValueToken(Convert.ToDecimal(vt1.SystemValue) - Convert.ToDecimal(vt2.SystemValue), TokenType.DecimalValue);
            }
            else
            {
                throw new ArgumentException($"- operator is valid only for numeric values.\nFirst token: {vt1}\nSecond token: {vt2}");
            }
        }

        public static ExpressionToken operator -(ExpressionToken et1)
        {
            if (!(et1 is ValueToken vt1))
            {
                throw new ArgumentException($"Unary - operator is valid only for ValueTokens.\n{et1} is not a ValueToken");
            }
            if (!(vt1.Type == TokenType.IntegerValue || vt1.Type == TokenType.DecimalValue))
            {
                throw new ArgumentException($"Unary - operator is valid only for numeric values.\n{vt1} is not {nameof(TokenType.NumericValue)}");
            }
            if (vt1.Type == TokenType.IntegerValue)
            {
                if (vt1.SystemType == typeof(short))
                {
                    return new ValueToken(-(short)vt1.SystemValue, TokenType.IntegerValue);
                }  
                else if (vt1.SystemType == typeof(int))
                {
                    return new ValueToken(-(int)vt1.SystemValue, TokenType.IntegerValue);
                }  
                else
                {
                    return new ValueToken(-(long)vt1.SystemValue, TokenType.IntegerValue);
                }         
            }
            else
            {
                if (vt1.SystemType == typeof(double))
                {
                    return new ValueToken(-(double)vt1.SystemValue, TokenType.NumericValue);
                }
                else
                {
                    return new ValueToken(-(decimal)vt1.SystemValue, TokenType.NumericValue);
                }
            }
        }

        public static ExpressionToken operator *(ExpressionToken et1, ExpressionToken et2)
        {
            if (!(et1 is ValueToken vt1))
            {
                throw new ArgumentException($"* operator is valid only for ValueTokens.\n{et1} is not a ValueToken");
            }
            if (!(et2 is ValueToken vt2))
            {
                throw new ArgumentException($"* operator is valid only for ValueTokens.\n{et2} is not a ValueToken");
            }

            if (vt1.Type == vt2.Type && vt1.Type == TokenType.NumericValue)
            {
                return new ValueToken((double)vt1.SystemValue * (double)vt2.SystemValue, TokenType.NumericValue);
            }
            else if (vt1.Type == vt2.Type && vt1.Type == TokenType.IntegerValue)
            {
                if (vt1.SystemType == typeof(short) && vt2.SystemType == typeof(short))
                {
                    return new ValueToken((short)vt1.SystemValue * (short)vt2.SystemValue, TokenType.IntegerValue);
                }
                else if (vt1.SystemType == typeof(int) && vt2.SystemType == typeof(int))
                {
                    return new ValueToken((int)vt1.SystemValue * (int)vt2.SystemValue, TokenType.IntegerValue);
                }
                else
                {
                    return new ValueToken((long)vt1.SystemValue * (long)vt2.SystemValue, TokenType.IntegerValue);
                }
            }
            else
            {
                throw new ArgumentException($"* operator is valid only for numeric values.\nFirst token: {vt1}\nSecond token: {vt2}");
            }
        }

        public static ExpressionToken operator /(ExpressionToken et1, ExpressionToken et2)
        {
            if (!(et1 is ValueToken vt1))
            {
                throw new ArgumentException($"/ operator is valid only for ValueTokens.\n{et1} is not a ValueToken");
            }
            if (!(et2 is ValueToken vt2))
            {
                throw new ArgumentException($"/ operator is valid only for ValueTokens.\n{et2} is not a ValueToken");
            }

                return new ValueToken(Convert.ToDecimal(vt1.SystemValue) / Convert.ToDecimal(vt2.SystemValue), TokenType.NumericValue);
            
            //throw new ArgumentException($"/ operator is valid only for numeric values.\nFirst token: {vt1}\nSecond token: {vt2}");
        }

        public static ExpressionToken AreTokensEqual(ExpressionToken et1, ExpressionToken et2)
        {
            if (!(et1 is ValueToken vt1))
            {
                throw new ArgumentException($"= operator is valid only for ValueTokens.\n{et1} is not a ValueToken");
            }
            if (!(et2 is ValueToken vt2))
            {
                throw new ArgumentException($"= operator is valid only for ValueTokens.\n{et2} is not a ValueToken");
            }

            if (vt1.SystemValue is IComparable && vt2.SystemValue is IComparable)
            {
                return new ValueToken((vt1.SystemValue as IComparable).CompareTo(vt2.SystemValue) == 0, TokenType.BooleanValue);
            }

            // if (vt1.Type == vt2.Type && vt1.Type == TokenType.NumericValue)
            // {
            //     return new ValueToken((double)vt1.SystemValue == (double)vt2.SystemValue, TokenType.BooleanValue);
            // }
            // else if (vt1.Type == vt2.Type && vt1.Type == TokenType.StringValue)
            // {
            //     return new ValueToken(vt1.SystemValue.ToString() == vt2.SystemValue.ToString(), TokenType.BooleanValue);
            // }
            // else if (vt1.Type == vt2.Type && vt1.Type == TokenType.BooleanValue)
            // {
            //     return new ValueToken((bool)vt1.SystemValue == (bool)vt2.SystemValue, TokenType.BooleanValue);
            // }
            else
            {
                throw new ArgumentException($"/ operator is valid only for numeric values.\nFirst token: {vt1}\nSecond token: {vt2}");
            }
        }

        public static ExpressionToken AreTokensNotEqual(ExpressionToken et1, ExpressionToken et2)
        {
            if (!(et1 is ValueToken vt1))
            {
                throw new ArgumentException($"!= operator is valid only for ValueTokens.\n{et1} is not a ValueToken");
            }
            if (!(et2 is ValueToken vt2))
            {
                throw new ArgumentException($"!= operator is valid only for ValueTokens.\n{et2} is not a ValueToken");
            }

            if (vt1.Type == vt2.Type && vt1.Type == TokenType.NumericValue)
            {
                return new ValueToken(value: (double)vt1.SystemValue != (double)vt2.SystemValue, tokenType: TokenType.BooleanValue);
            }
            else if (vt1.Type == vt2.Type && vt1.Type == TokenType.StringValue)
            {
                return new ValueToken(value: vt1.SystemValue.ToString() != vt2.SystemValue.ToString(), tokenType: TokenType.BooleanValue);
            }
            else if (vt1.Type == vt2.Type && vt1.Type == TokenType.BooleanValue)
            {
                return new ValueToken(value: (bool)vt1.SystemValue != (bool)vt2.SystemValue, tokenType: TokenType.BooleanValue);
            }
            else
            {
                throw new ArgumentException($"/ operator is valid only for numeric values.\nFirst token: {vt1}\nSecond token: {vt2}");
            }
        }

        public static ExpressionToken operator <(ExpressionToken et1, ExpressionToken et2)
        {
            if (!(et1 is ValueToken vt1))
            {
                throw new ArgumentException($"< operator is valid only for ValueTokens.\n{et1} is not a ValueToken");
            }
            if (!(et2 is ValueToken vt2))
            {
                throw new ArgumentException($"< operator is valid only for ValueTokens.\n{et2} is not a ValueToken");
            }

            if (vt1.Type == vt2.Type && vt1.Type == TokenType.NumericValue)
            {
                return new ValueToken((double)vt1.SystemValue < (double)vt2.SystemValue, TokenType.BooleanValue);
            }
            else
            {
                throw new ArgumentException($"< operator is valid only for numeric values.\nFirst token: {vt1}\nSecond token: {vt2}");
            }
        }

        public static ExpressionToken operator <=(ExpressionToken et1, ExpressionToken et2)
        {
            if (!(et1 is ValueToken vt1))
            {
                throw new ArgumentException($"<= operator is valid only for ValueTokens.\n{et1} is not a ValueToken");
            }
            if (!(et2 is ValueToken vt2))
            {
                throw new ArgumentException($"<= operator is valid only for ValueTokens.\n{et2} is not a ValueToken");
            }

            if (vt1.Type == vt2.Type && vt1.Type == TokenType.NumericValue)
            {
                return new ValueToken((double)vt1.SystemValue <= (double)vt2.SystemValue, TokenType.BooleanValue);
            }
            else
            {
                throw new ArgumentException($"<= operator is valid only for numeric values.\nFirst token: {vt1}\nSecond token: {vt2}");
            }
        }

        public static ExpressionToken operator >(ExpressionToken et1, ExpressionToken et2)
        {
            if (!(et1 is ValueToken vt1))
            {
                throw new ArgumentException($"< operator is valid only for ValueTokens.\n{et1} is not a ValueToken");
            }
            if (!(et2 is ValueToken vt2))
            {
                throw new ArgumentException($"< operator is valid only for ValueTokens.\n{et2} is not a ValueToken");
            }

            if (vt1.Type == vt2.Type && vt1.Type == TokenType.NumericValue)
            {
                return new ValueToken((double)vt1.SystemValue > (double)vt2.SystemValue, TokenType.BooleanValue);
            }
            else
            {
                throw new ArgumentException($"> operator is valid only for numeric values.\nFirst token: {vt1}\nSecond token: {vt2}");
            }
        }

        public static ExpressionToken operator >=(ExpressionToken et1, ExpressionToken et2)
        {
            if (!(et1 is ValueToken vt1))
            {
                throw new ArgumentException($">= operator is valid only for ValueTokens.\n{et1} is not a ValueToken");
            }
            if (!(et2 is ValueToken vt2))
            {
                throw new ArgumentException($">= operator is valid only for ValueTokens.\n{et2} is not a ValueToken");
            }

            if (vt1.Type == vt2.Type && vt1.Type == TokenType.NumericValue)
            {
                return new ValueToken((double)vt1.SystemValue >= (double)vt2.SystemValue, TokenType.BooleanValue);
            }
            else
            {
                throw new ArgumentException($">= operator is valid only for numeric values.\nFirst token: {vt1}\nSecond token: {vt2}");
            }
        }

        public static ExpressionToken operator |(ExpressionToken et1, ExpressionToken et2)
        {
            if (!(et1 is ValueToken vt1))
            {
                throw new ArgumentException($"OR operator is valid only for ValueTokens.\n{et1} is not a ValueToken");
            }
            if (!(et2 is ValueToken vt2))
            {
                throw new ArgumentException($"OR operator is valid only for ValueTokens.\n{et2} is not a ValueToken");
            }

            if (vt1.Type == vt2.Type && vt1.Type == TokenType.BooleanValue)
            {
                return new ValueToken((bool)vt1.SystemValue || (bool)vt2.SystemValue, TokenType.BooleanValue);
            }
            else
            {
                throw new ArgumentException($"OR operator is valid only for numeric values.\nFirst token: {vt1}\nSecond token: {vt2}");
            }
        }

        public static ExpressionToken operator &(ExpressionToken et1, ExpressionToken et2)
        {
            if (!(et1 is ValueToken vt1))
            {
                throw new ArgumentException($"AND operator is valid only for ValueTokens.\n{et1} is not a ValueToken");
            }
            if (!(et2 is ValueToken vt2))
            {
                throw new ArgumentException($"AND operator is valid only for ValueTokens.\n{et2} is not a ValueToken");
            }

            if (vt1.Type == vt2.Type && vt1.Type == TokenType.BooleanValue)
            {
                return new ValueToken((bool)vt1.SystemValue && (bool)vt2.SystemValue, TokenType.BooleanValue);
            }
            else
            {
                throw new ArgumentException($"AND operator is valid only for numeric values.\nFirst token: {vt1}\nSecond token: {vt2}");
            }
        }

        public static ExpressionToken operator !(ExpressionToken et1)
        {
            if (!(et1 is ValueToken vt1))
            {
                throw new ArgumentException($"NOT operator is valid only for ValueTokens.\n{et1} is not a ValueToken.");
            }
            if (vt1.Type == TokenType.BooleanValue)
            {
                vt1.SystemValue = !(bool)vt1.SystemValue;
                return vt1;
            }
            throw new ArgumentException($"NOT operator is valid only for boolean values.\nPassed argument: {et1} is not of type boolean.");
        }

        public override string ToString()
        {
            return $"Token [Type = {this.Type}, Position = {PositionStart}] : {StringValue}";
        }

    }
}