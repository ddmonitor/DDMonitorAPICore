using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DDMonitor.Core.Linq
{
    public static class MySQLPager
    {

        /// <summary>
        /// 将查询结果进行分页，针对MySQL优化
        /// </summary>
        /// <typeparam name="T">查询实体类型</typeparam>
        /// <param name="source">原始LINQ查询</param>
        /// <param name="current">当前页，默认1</param>
        /// <param name="size">每页数量， 默认20</param>
        /// <returns></returns>
        public static PageResult<T> MySQLPage<T>(this IQueryable<T> source, int current = 1, int size = 20)
        {
            var count = size * (current - 1);
            var data = source.FromSqlFragment($" LIMIT {count}, {size}");
            var total = data.Count();
            var pages = (int)Math.Ceiling(total * 1.0 / size);

            return new PageResult<T>()
            {
                Current = current,
                Size = size,
                Total = total,
                Pages = pages,
                Data = data
            };
        }

    }

}
