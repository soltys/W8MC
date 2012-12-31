using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMC.Service
{
    class PublicClassExpression:ClassExpression
    {
        public PublicClassExpression(string className, string classContent) : base(className, "public", classContent)
        {
        }
    }
}
