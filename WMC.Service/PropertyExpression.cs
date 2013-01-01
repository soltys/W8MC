using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMC.Service
{
    public class  PropertyExpression:IExpression
    {
        private string _propertyName;
        private string _propertyType;
        private const string PrivateNamePrefix = "_";

        public PropertyExpression(string propertyType, string propertyName)
        {
            _propertyName = propertyName;
            _propertyType = propertyType;
        }

        public string Interpret()
        {
            var sb = new StringBuilder();
            string privateName = PrivateNamePrefix +  _propertyName.Substring(0, 1).ToLowerInvariant() + _propertyName.Substring(1);
            sb.AppendFormat("private {0} {1};{2}",_propertyType,privateName,Environment.NewLine);
            sb.AppendFormat("public {0} {1}{2}", _propertyType, _propertyName,Environment.NewLine);
            sb.AppendLine("{");
            sb.Append(Indent.FourSpace).AppendLine("get");
            sb.Append(Indent.FourSpace).Append("{\n");
            sb.Append(Indent.FourSpace).Append(Indent.FourSpace).AppendLine("return " + privateName + ";");
            sb.Append(Indent.FourSpace).AppendLine("}");
            sb.Append(Indent.FourSpace).AppendLine("set");
            sb.Append(Indent.FourSpace).AppendLine("{");
            sb.Append(Indent.FourSpace).Append(Indent.FourSpace).AppendFormat("SetProperty(ref {0}, value);"+Environment.NewLine, privateName);
            sb.Append(Indent.FourSpace).AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine();
            return sb.ToString();
        }

    }
}
