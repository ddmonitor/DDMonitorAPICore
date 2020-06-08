using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDMonitor.Core.Linq
{

    public static class TreeMaker
    {

        public static IEnumerable<TreeItem<T>> Tree<T>(this IEnumerable<T> source,
            Func<T, int> idSelector,
            Func<T, string> nameSelector,
            Func<T, int?> parentSelector,
            Func<T, string> codeSelector = null)
            where T : class
        {
            List<TreeItem<T>> list;
            if (codeSelector != null)
            {
                list = source
                    .OrderBy(e => codeSelector(e))
                    .Select(e => new TreeItem<T>
                    {
                        Id = idSelector(e),
                        Code = codeSelector(e),
                        Name = nameSelector(e),
                        ParentId = parentSelector(e),
                    }).ToList();
            }
            else
            {
                list = source.Select(e => new TreeItem<T>
                {
                    Id = idSelector(e),
                    Name = nameSelector(e),
                    ParentId = parentSelector(e),
                }).ToList();
            }

            var tree = new List<TreeItem<T>>();
            foreach (var node in list)
            {
                if (node.ParentId == null)
                {
                    tree.Add(node);
                }
                else
                {
                    var parent = list.FirstOrDefault(e => e.Id == node.ParentId);
                    if (parent == null)
                    {
                        Console.WriteLine($"找不到实体{{ id = {node.Id} }}的父级 {node.ParentId}，已作为根节点处理");
                        tree.Add(node);
                    }
                    else
                    {
                        parent.Children.Add(node);
                    }
                }
            }

            return tree;
        }
    }
}
