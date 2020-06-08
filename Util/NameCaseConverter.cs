using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDMonitor.Core.Util
{
    public enum NameCaseType
    {
        PascalCase,
        CamelCase,
        SnakeCase,
        KebabCase,
        UnixCase
    }
    public static class NameCaseConverter
    {
        public static string ToPascalCase(this string name, NameCaseType type)
        {
            if (name != null && name.Length > 0)
            {
                if (type == NameCaseType.CamelCase)
                {
                    return name[0].ToString().ToUpper() + name.Substring(1);
                }
            }
            return name;
        }
    }
}
