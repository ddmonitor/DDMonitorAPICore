using System.Collections.Generic;

namespace DDMonitor.Core.Linq
{
    public class PageResult<T>
    {
        public int Current { get; set; }

        public int Size { get; set; }

        public int Total { get; set; }

        public int Pages { get; set; }

        public IEnumerable<T> Data { get; set; }
    }
}
