using System.Collections.Generic;

namespace AusmaProgram.Expressions.Parser
{
    public class ExpressionFunction
    {

        public string Name { get; set; }

        public List<ExpressionFunctionArgument> Arguments { get; private set; }

        public ExpressionFunction(string functionName, List<ExpressionFunctionArgument> functionArguments)
        {
            this.Name = functionName;
            this.Arguments = functionArguments;
        }

        public ExpressionFunction()
        {
            this.Arguments = new List<ExpressionFunctionArgument>();
        }

    }
}