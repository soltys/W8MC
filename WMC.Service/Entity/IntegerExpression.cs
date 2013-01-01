using System;

namespace WMC.Service
{
    public class IntegerExpression : PropertyExpression
    {
        public IntegerExpression(string propertyName):base("int", propertyName)
        {
            
        }
    }
}