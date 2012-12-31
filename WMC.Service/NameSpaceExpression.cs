using System;
using System.IO;
using System.Text;

namespace WMC.Service
{
    public class NameSpaceExpression : IExpression
    {
        private string _namespaceName;

        private ClassExpression _classExpression;
        public NameSpaceExpression(string namespaceName, string namespaceContent)
        {
            _namespaceName = namespaceName;
            StringReader stringReader = new StringReader(namespaceContent);

        }

        public string Interpret()
        {
            StringBuilder sb =new StringBuilder();
            string header = "namespace " + _namespaceName + "\n{";
            sb.Append(header);
            using (StringReader sr = new StringReader(_classExpression.Interpret()))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.Append(Indent.FourSpace).AppendLine(line);
                }
            }
            
            const string ending = "\n}";
            sb.Append(ending);
            return sb.ToString();
        }
    }
}