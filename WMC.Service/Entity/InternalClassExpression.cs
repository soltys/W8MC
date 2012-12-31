using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMC.Service
{
    class InternalClassExpression:ClassExpression
    {
        public InternalClassExpression(string className,  string classContent) : base(className, "internal", classContent)
        {
        }
    }
}
