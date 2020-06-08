using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace DDMonitor.Core
{
    public class EntityInfo<T> where T : class
    {
        static EntityInfo()
        {
            initProperty();
        }

        public static Dictionary<string, PropertyInfo> PropertyMap { get; set; } = new Dictionary<string, PropertyInfo> { };

        static void initProperty()
        {
            var type = typeof(T);
            foreach (var p in type.GetProperties())
            {
                if (p.GetCustomAttribute<NotMappedAttribute>() == null)
                {
                    PropertyMap[p.Name] = p;
                }
            }
        }

        public static PropertyInfo GetProperty(string name)
        {
            if (PropertyMap.ContainsKey(name))
            {
                return PropertyMap[name];
            }
            throw new ArgumentException("Unknown property ", nameof(name));
        }
    }
}
