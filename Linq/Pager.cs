using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DDMonitor.Core.Linq
{
    public static class Pager
    {

        /// <summary>
        /// 将查询结果进行分页
        /// </summary>
        /// <typeparam name="T">查询实体类型</typeparam>
        /// <param name="source">原始LINQ查询</param>
        /// <param name="current">当前页，默认1</param>
        /// <param name="size">每页数量， 默认20</param>
        /// <returns></returns>
        public static PageResult<T> Page<T>(this IEnumerable<T> source, int current = 1, int size = 20)
        {
            var originSize = size;
            
            var total = source.Count();

            if (current < 1) current = 1;
            if (size > total) size = total;
            var count = size * (current - 1);
            var pages = (int)Math.Ceiling(total * 1.0 / size);

            return new PageResult<T>()
            {
                Current = current,
                Size = originSize,
                Total = total,
                Pages = pages,
                Data = source.Skip(count).Take(size)
            };
        }
    }

}
