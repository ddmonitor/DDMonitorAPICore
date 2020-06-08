using System.Collections.Generic;

namespace DDMonitor.Core.Linq
{
    public class TreeItem<T> where T : class
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public T Data { get; set; }

        public int? ParentId { get; set; }

        public List<TreeItem<T>> Children { get; } = new List<TreeItem<T>>();
    }

}
