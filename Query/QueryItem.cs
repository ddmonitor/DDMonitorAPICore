using DDMonitor.Core.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace DDMonitor.Core.Query
{
    public class QueryItem<TModel> where TModel : class
    {
        internal static Dictionary<string, QueryCondition> conditionMap = new Dictionary<string, QueryCondition>
        {
            ["eq"] = QueryCondition.Equal,
            ["ne"] = QueryCondition.NotEqual,
            ["gt"] = QueryCondition.Greater,
            ["lt"] = QueryCondition.Less,
            ["ge"] = QueryCondition.GreaterOrEqual,
            ["le"] = QueryCondition.LessOrEqual,
            ["like"] = QueryCondition.Like,
            ["in"] = QueryCondition.In
        };


        public QueryItem(QueryItemVO vo)
        {
            if (string.IsNullOrEmpty(vo.Property) || string.IsNullOrEmpty(vo.Condition))
            {
                throw new ArgumentNullException(nameof(vo));
            }
            Property = NameCaseConverter.ToPascalCase(vo.Property, NameCaseType.CamelCase);
            if (conditionMap.TryGetValue(vo.Condition, out var condition))
            {
                Condition = condition;
                if (EntityInfo<TModel>.PropertyMap.ContainsKey(Property))
                {
                    Value = parseValue(vo.Value, EntityInfo<TModel>.PropertyMap[Property].PropertyType);
                }
            }
            else
            {
                throw new ArgumentException("Unknown condition: " + vo.Condition);
            }

        }


        object parseValue(object origin, Type type)
        {
            if (origin is JsonElement json)
            {
                // Nullable<>
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var realType = type.GetGenericArguments()[0];
                    return parseValue(origin, realType);
                }

                // Array
                if (json.ValueKind == JsonValueKind.Array)
                {
                    var gt = typeof(List<>).MakeGenericType(type);
                    return JsonSerializer.Deserialize(json.ToString(), gt);
                } 
                else
                {
                    // Null
                    if (json.ValueKind == JsonValueKind.Null || json.ValueKind == JsonValueKind.Undefined)
                    {
                        return null;
                    }
                    // Number
                    if (EntityUtil.NumberTypes.Contains(type))
                    {
                        var t = typeof(JsonElement);
                        var m = t.GetMethod("Get" + type.Name);
                        return m.Invoke(json, new object[] { });
                    }
                    // String
                    else if (type == typeof(string))
                    {
                        return json.GetString();
                    }
                    // Boolean
                    else if (type == typeof(bool))
                    {
                        return json.GetBoolean();
                    }
                }
            }
            return origin;
        }

        public string Property { get; set; }

        public QueryCondition Condition { get; set; }

        public object Value { get; set; }
    }
}
