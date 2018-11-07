using System;
using System.Collections.Generic;
using System.Text;

namespace Topwox.Data.Attributes.Shema
{
    public class NavigationExpressionAttribute : Attribute
    {
        public string Expression { get; set; }
        public NavigationExpressionAttribute(string expression)
        {
            Expression = expression;
        }
    }
}
