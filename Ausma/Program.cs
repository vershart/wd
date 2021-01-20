using System;
using AusmaProgram.Expressions.Tokenizer;

namespace Ausma
{
    class InteractiveConsole
    {

        private static ExpressionTokenizer et;

        static void Main(string[] args)
        {
            Console.WriteLine("Expression tokenizer and evaluator");
            Console.WriteLine();


            Console.Write("Expression: ");


            string uInput = Console.ReadLine();
            while (uInput != "exit")
            {
                try
                {
                    et = new ExpressionTokenizer(uInput);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                foreach (ExpressionToken token in et.GetTokens())
                {
                    Console.WriteLine(token);
                }
                Console.WriteLine();
                Console.Write("Expression: ");
                uInput = Console.ReadLine();
            }
        }
    }
}
