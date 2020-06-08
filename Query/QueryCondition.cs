using System;
using System.Linq;
using System.Threading.Tasks;

namespace DDMonitor.Core.Query
{
    public enum QueryCondition
    {
        Equal, NotEqual,
        Greater, Less, GreaterOrEqual, LessOrEqual,
        Like, 
        In
    }

}
