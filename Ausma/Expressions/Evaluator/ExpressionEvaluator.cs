
using AusmaProgram.Expressions.Parser;
using AusmaProgram.Expressions.Tokenizer;

namespace AusmaProgram.Expressions.Evaluator
{
    public class ExpressionEvaluator
    {
        
        private static ExpressionResult CallFunction(ExpressionFunction function)
        {
            ExpressionResult result = new ExpressionResult();
            return result;
        }

        public static ExpressionResult ExecuteFunction(ExpressionFunction function)
        {
            ExpressionResult result = new ExpressionResult();

            foreach (ExpressionFunctionArgument arg in function.Arguments)
            {
                ExpressionParser parser = new ExpressionParser(arg.ArgumentTokens);
                ExpressionResult argument = parser.Execute();
            }

            return result;
        }
    }
}