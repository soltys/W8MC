using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace WMC.Service
{
    public abstract class ClassExpression : IExpression
    {
        private string _className;
        List<PropertyExpression> _classContent = new List<PropertyExpression>();
        private string _modifier;

        protected ClassExpression(string className, string modifier, string classContent)
        {
            _className = className;
            _modifier = modifier;

            StringReader sr = new StringReader(classContent);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrWhiteSpace(line))
                {
                    Debug.WriteLine("WMC.Service: Line was empty");
                    continue;
                }

                string[] tokens = line.Split(' ');
                if (tokens.Length != 2)
                {
                    Debug.WriteLine("WMC.Service: Line: "+ line + " do no have 2 tokens seprated by space");
                    continue;
                }

                switch (tokens[0])
                {
                    case "i":
                        _classContent.Add(new IntegerExpression(tokens[1]));
                        break;
                    case "d":
                        _classContent.Add(new DoubleExpression(tokens[1]));
                        break;
                    case "f":
                        _classContent.Add(new FloatExpression(tokens[1]));
                        break;
                    case "s":
                        _classContent.Add(new StringExpression(tokens[1]));
                        break;
                    case "dt":
                        _classContent.Add(new DateTimeExpression(tokens[1]));
                        break;
                    case "ts":
                        _classContent.Add(new TimeSpanExpression(tokens[1]));
                        break;
                    default:
                        Debug.Write("Not recognized type of property, ignored. line: "+ line);
                        break;
                }
               
            }
            
        }


        public virtual string Interpret()
        {

            string header = _modifier + " class " + _className + " : BindableBase" + Environment.NewLine + "{"+Environment.NewLine;

            const string ending = "}";

            StringBuilder sb = new StringBuilder();
            sb.Append(header);
            foreach (var classParameter in _classContent)
            {
                using(StringReader sr = new StringReader(classParameter.Interpret()))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.Append(Indent.FourSpace).AppendLine(line);
                    }
                }
                
            }
            sb.Append(ending);

            return sb.ToString();
        }
    }

    public class TimeSpanExpression : PropertyExpression
    {
        public TimeSpanExpression(string propertyName):base(propertyName, "TimeSpan")
        {
        }
    }
}