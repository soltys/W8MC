using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WMC.Service
{
    public sealed class Evaluator : IExpression
    {
        private readonly IExpression _expression;

        public Evaluator(string expression)
        {
            if (String.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentException("Expression is null or whitespaces");
            }
            StringReader stringReader = new StringReader(expression);
            string firstLine = stringReader.ReadLine();
            if (string.IsNullOrWhiteSpace(firstLine))
            {
                throw new ArgumentException("Expression do not have any line ended with new line");
            }
            string[] tokens = firstLine.Split(' ');
            if (tokens.Length != 2)
            {
                throw new ArgumentException("Tokens are not equal to 2");
            }

            switch (tokens[0])
            {
                case "c":
                    _expression = new InternalClassExpression(tokens[1], stringReader.ReadToEnd());
                    break;
                case "pc":
                    _expression = new PublicClassExpression(tokens[1], stringReader.ReadToEnd());
                    break;
                case "ns":
                    _expression = new NameSpaceExpression(tokens[1], stringReader.ReadToEnd());
                    break;
            }

            if (_expression == null)
            {
                throw new ArgumentException("Expression do not stats with namespace nor class declaration");
            }
        }
        public string Interpret()
        {
            
                return "using System;" + Environment.NewLine + _expression.Interpret();    
            
        }
    }
}
