using System;

namespace AusmaProgram.Expressions.Tokenizer
{
    public class ValueToken : ExpressionToken
    {
        
        private object systemValue = null;

        private object TryConvertInteger()
        {
            short int16 = 0;
            if (short.TryParse(StringValue, out int16))
            {
                return int16;
            }
            int int32 = 0;
            if (int.TryParse(StringValue, out int32))
            {
                return int32;
            }
            long int64 = 0;
            if (long.TryParse(StringValue, out int64))
            {
                return int64;
            }
            throw new ArgumentException($"{StringValue} cannot be converted to short, int or long type!");
        }

        public object TryConvertDecimal()
        {
            double float32 = 0;
            if (double.TryParse(StringValue, out float32))
            {
                return float32;
            }
            decimal float64 = 0;
            if (decimal.TryParse(StringValue, out float64))
            {
                return float64;
            }
            throw new ArgumentException($"{StringValue} cannot be converted to double or decimal type!");
        }

        public object SystemValue
        {
            get
            {
                return systemValue;
            }
            set
            {
                this.StringValue = value.ToString();
                switch (Type)
                {
                    case TokenType.IntegerValue:
                        systemValue = TryConvertInteger();
                        break;
                    case TokenType.DecimalValue:
                        systemValue = TryConvertDecimal();
                        break;
                    case TokenType.NumericValue:
                        systemValue = Convert.ToDouble(StringValue);
                        break;
                    case TokenType.BooleanValue:
                        systemValue = value;
                        break;
                    case TokenType.StringValue:
                        systemValue = StringValue;
                        break;
                    default:
                        systemValue = Type;
                        break;
                }
            }
        }

        public Type SystemType
        {
            get 
            {
                return systemValue?.GetType();
            }
        }

        public override string ToString()
        {
            return $"ValueToken [Type = {this.Type}, SystemType = {this.SystemType}, Value = {this.systemValue}]";
        }

        public ValueToken(object value, TokenType tokenType)
        {
            this.Type = tokenType;
            this.SystemValue = value;
        }

        public ValueToken()
        {

        }

    }
}