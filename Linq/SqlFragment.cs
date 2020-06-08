using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DDMonitor.Core.Linq
{

	public static class SqlFragment
    {
        public static IQueryable<T> FromSqlFragment<T>(this IQueryable<T> source, string sql)
        {
            var expr = new SqlFragmentExpression(sql);
            return source.Provider.CreateQuery<T>(expr);
        }
    }
}
