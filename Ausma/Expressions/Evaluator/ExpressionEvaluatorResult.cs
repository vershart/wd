using System;

namespace AusmaProgram.Expressions.Evaluator
{
    public class ExpressionResult
    {
        public object Result { get; internal set; }
        public Type ResultType { get; internal set; }
    }
}