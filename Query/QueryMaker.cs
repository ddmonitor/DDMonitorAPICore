using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DDMonitor.Core.Util;
using System.Text.Json;
using System.Linq.Expressions;
using System.Reflection;

namespace DDMonitor.Core.Query
{

    public static class QueryMaker
    {
        /// <summary>查询实体</summary>
        public static IQueryable<T> Query<T>(this IQueryable<T> source, IEnumerable<QueryItemVO> queryVO) where T : class
        {
            var queries = queryVO.Select(q => new QueryItem<T>(q));
            foreach (var condition in queries)
            {
                var prop = condition.Property;
                switch (condition.Condition)
                {
                    case QueryCondition.Equal:
                        source = source.Where(MakeEqual<T>(prop, condition.Value));
                        break;
                    case QueryCondition.NotEqual:
                        source = source.Where(MakeNotEqual<T>(prop, condition.Value));
                        break;
                    case QueryCondition.Greater:
                        source = source.Where(MakeGreater<T>(prop, condition.Value));
                        break;
                    case QueryCondition.Less:
                        source = source.Where(MakeLess<T>(prop, condition.Value));
                        break;
                    case QueryCondition.GreaterOrEqual:
                        source = source.Where(MakeGreaterOrEqual<T>(prop, condition.Value));
                        break;
                    case QueryCondition.LessOrEqual:
                        source = source.Where(MakeLessOrEqual<T>(prop, condition.Value));
                        break;
                    case QueryCondition.Like:
                        source = source.Where(MakeLike<T>(prop, condition.Value));
                        break;
                    case QueryCondition.In:
                        source = source.Where(MakeIn<T>(prop, condition.Value));
                        break;
                    default:
                        break;
                }
            }
            return source;
        }

        static Expression<Func<T, bool>> GetExpression<T>(Expression<Func<T, bool>> expression)
        {
            return expression;
        }

        public static Expression<Func<T, bool>> MakeEqual<T>(string prop, object value) where T : class
        {
            if (value != null)
            {
                var type = value.GetType();
                if (EntityUtil.EquatableTypes.Contains(type))
                {
                    var param = Expression.Parameter(typeof(T), "i");
                    var propType = EntityInfo<T>.GetProperty(prop).PropertyType;
                    var realType = propType;

                    return Expression.Lambda<Func<T, bool>>(
                        Expression.Equal(
                            Expression.PropertyOrField(param, prop),
                            // 针对Nullable
                            Expression.Convert(Expression.Constant(value), propType)
                        ),
                        param
                    );
                }
                throw new ArgumentException("Not supported type"); 
            }
            return GetExpression<T>(i => EF.Property<object>(i, prop) == null);
        }

        public static Expression<Func<T, bool>> MakeNotEqual<T>(string prop, object value) where T : class
        {
            if (value != null)
            {
                var type = value.GetType();
                if (EntityUtil.EquatableTypes.Contains(type))
                {
                    var param = Expression.Parameter(typeof(T), "i");
                    var propType = EntityInfo<T>.GetProperty(prop).PropertyType;
                    var realType = propType;

                    return Expression.Lambda<Func<T, bool>>(
                        Expression.NotEqual(
                            Expression.PropertyOrField(param, prop),
                            // 针对Nullable
                            Expression.Convert(Expression.Constant(value), propType)
                        ),
                        param
                    );
                }
                throw new ArgumentException("Not supported type");
            }
            return GetExpression<T>(i => EF.Property<object>(i, prop) != null);
        }

        public static Expression<Func<T, bool>> MakeGreater<T>(string prop, object value) where T : class
        {
            if (value != null)
            {
                var type = value.GetType();
                if (EntityUtil.ComparableTypes.Contains(type))
                {
                    var param = Expression.Parameter(typeof(T), "i");
                    var propType = EntityInfo<T>.GetProperty(prop).PropertyType;

                    return Expression.Lambda<Func<T, bool>>(
                        Expression.GreaterThan(
                            Expression.PropertyOrField(param, prop),
                            // 针对Nullable
                            Expression.Convert(Expression.Constant(value), propType)
                        ),
                        param
                    );
                }
                throw new ArgumentException("Not supported type");
            }
            throw new ArgumentNullException(nameof(value));
        }

        public static Expression<Func<T, bool>> MakeLess<T>(string prop, object value) where T : class
        {
            if (value != null)
            {
                var type = value.GetType();
                if (EntityUtil.ComparableTypes.Contains(type))
                {
                    var param = Expression.Parameter(typeof(T), "i");
                    var propType = EntityInfo<T>.GetProperty(prop).PropertyType;

                    return Expression.Lambda<Func<T, bool>>(
                        Expression.LessThan(
                            Expression.PropertyOrField(param, prop),
                            // 针对Nullable
                            Expression.Convert(Expression.Constant(value), propType)
                        ),
                        param
                    );
                }
                throw new ArgumentException("Not supported type");
            }
            throw new ArgumentNullException(nameof(value));
        }

        public static Expression<Func<T, bool>> MakeGreaterOrEqual<T>(string prop, object value) where T : class
        {
            if (value != null)
            {
                var type = value.GetType();
                if (EntityUtil.ComparableTypes.Contains(type))
                {
                    var param = Expression.Parameter(typeof(T), "i");
                    var propType = EntityInfo<T>.GetProperty(prop).PropertyType;

                    return Expression.Lambda<Func<T, bool>>(
                        Expression.GreaterThanOrEqual(
                            Expression.PropertyOrField(param, prop),
                            // 针对Nullable
                            Expression.Convert(Expression.Constant(value), propType)
                        ),
                        param
                    );
                }
                throw new ArgumentException("Not supported type");
            }
            throw new ArgumentNullException(nameof(value));
        }

        public static Expression<Func<T, bool>> MakeLessOrEqual<T>(string prop, object value) where T : class
        {
            if (value != null)
            {
                var type = value.GetType();
                if (EntityUtil.ComparableTypes.Contains(type))
                {
                    var param = Expression.Parameter(typeof(T), "i");
                    var propType = EntityInfo<T>.GetProperty(prop).PropertyType;

                    return Expression.Lambda<Func<T, bool>>(
                        Expression.LessThanOrEqual(
                            Expression.PropertyOrField(param, prop),
                            // 针对Nullable
                            Expression.Convert(Expression.Constant(value), propType)
                        ),
                        param
                    );
                }
                throw new ArgumentException("Not supported type");
            }
            throw new ArgumentNullException(nameof(value));
        }

        public static Expression<Func<T, bool>> MakeLike<T>(string prop, object value) where T : class
        {
            if (value != null)
            {
                var type = value.GetType();
                if (type == typeof(string))
                {
                    return GetExpression<T>(i => EF.Property<string>(i, prop).Contains(value as string));
                }
                throw new ArgumentException("Not supported type");
            }
            throw new ArgumentNullException(nameof(value));
        }

        public static Expression<Func<T, bool>> MakeIn<T>(string prop, object value) where T : class
        {
            if (value != null)
            {
                var type = value.GetType();
                if (type.IsGenericType)
                {
                    var baseType = type.GetGenericTypeDefinition();
                    var typeArgs = type.GetGenericArguments();
                    if (typeArgs.Length == 1 && baseType.GetInterface("IEnumerable`1") != null)
                    {
                        var itemType = typeArgs[0];
                        var param = Expression.Parameter(typeof(T), "i");
                        var propType = EntityInfo<T>.GetProperty(prop).PropertyType;

                        var method = typeof(Enumerable)
                            .GetMethods(BindingFlags.Public | BindingFlags.Static)
                            .Where(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
                            .FirstOrDefault()
                            .MakeGenericMethod(itemType);
                        return Expression.Lambda<Func<T, bool>>(
                            Expression.Call(method,
                                Expression.Convert(Expression.Constant(value), typeof(IEnumerable<>).MakeGenericType(itemType)),
                                Expression.Convert(Expression.PropertyOrField(param, prop), itemType)                       
                            ),
                            param
                        );
                    }
                }
                throw new ArgumentException("Not supported type");
            }
            throw new ArgumentNullException(nameof(value));
        }

    }

}

