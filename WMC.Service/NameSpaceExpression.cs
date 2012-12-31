using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace WMC.Service
{
    public sealed class NameSpaceExpression : IExpression
    {
        private readonly string _namespaceName;
        private readonly ClassExpression _classExpression;
        public NameSpaceExpression(string namespaceName, string namespaceContent)
        {
            _namespaceName = namespaceName;
            StringReader stringReader = new StringReader(namespaceContent);
            string line = stringReader.ReadLine();

            if (string.IsNullOrWhiteSpace(line))
            {
                Debug.WriteLine("WMC.Service: First line after namespace is empty");

            }
            else
            {
                line = line.Trim();

                string[] tokens = line.Split(' ');
                if (tokens.Length != 2)
                {
                    Debug.WriteLine("WMC.Service: Line: " + line +
                                    " (after namespace) do no have 2 tokens seprated by space");
                    return;
                }

                if (tokens[0] == "c")
                {
                    _classExpression = new InternalClassExpression(tokens[1], stringReader.ReadToEnd());
                }
                else if (tokens[0] == "pc")
                {
                    _classExpression = new PublicClassExpression(tokens[1], stringReader.ReadToEnd());
                }

                if (_classExpression == null)
                {
                    throw new ArgumentException("WMC.Service: Line: " + line +
                                                " (after namespace) is not class definition");
                }
            }

        }

        public string Interpret()
        {
            StringBuilder sb = new StringBuilder();
            string header = "namespace " + _namespaceName + Environment.NewLine + "{" + Environment.NewLine;
            sb.Append(header);
            if (_classExpression != null)
            {
                using (StringReader sr = new StringReader(_classExpression.Interpret()))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.Append(Indent.FourSpace).AppendLine(line);
                    }
                }
            }
            string ending = Environment.NewLine + "}";
            sb.Append(ending);
            return sb.ToString();
        }
    }
}