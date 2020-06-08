using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DDMonitor.Core.Util
{
    public class EntityUtil
    {
        public static List<Type> NumberTypes { get; set; } = new List<Type>
        {
            typeof(sbyte), typeof(byte),
            typeof(short), typeof(ushort),
            typeof(int), typeof(uint),
            typeof(long), typeof(ulong),

            typeof(float), typeof(double),
            typeof(decimal)
        };

        public static List<Type> EquatableTypes { get; set; } = NumberTypes
            .Concat(new List<Type> 
            {
                typeof(string), typeof(char),
                typeof(bool),
                typeof(DateTime)
            })
            .ToList();

        public static List<Type> ComparableTypes { get; set; } = NumberTypes
            .Concat(new List<Type>
            {
                typeof(string), typeof(char),
                typeof(DateTime)
            })
            .ToList();
        
        public static Expression Property<T>(T entity, string prop) where T : class
        {
            var p = EntityInfo<T>.GetProperty(prop);
            return Expression.Call(typeof(EF), nameof(EF.Property), new[] { p.PropertyType },
                Expression.Constant(entity),
                Expression.Constant(prop));
        }
    }
}
