using System.Collections.Generic;
using AusmaProgram.Expressions.Tokenizer;

namespace AusmaProgram.Expressions.Parser
{
    public class ExpressionFunctionArgument
    {
        
        public List<ExpressionToken> ArgumentTokens { get; set; }

        public ExpressionFunctionArgument()
        {
            ArgumentTokens = new List<ExpressionToken>();
        }

    }
}