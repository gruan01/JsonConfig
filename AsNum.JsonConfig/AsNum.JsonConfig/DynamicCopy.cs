using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AsNum.JsonConfig
{
    /// <summary>
    /// 
    /// </summary>
    public static class DynamicCopy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyTo<T>(T source, T target)
        {
            Helper<T>.CopyPropertiesOnly(source, target);
        }

        /// <summary>
        /// 只考贝指定的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源</param>
        /// <param name="target">目标</param>
        /// <param name="only">指定的字段</param>
        public static void CopyToOnly<T>(this T source, T target, params Expression<Func<T, object>>[] only)
        {
            Helper<T>.CopyPropertiesOnly(source, target, only);
        }

        /// <summary>
        /// 除指定的属性外,全考贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="only"></param>
        public static void CopyToExcept<T>(this T source, T target, params Expression<Func<T, object>>[] only)
        {
            Helper<T>.CopyPropertiesExcept(source, target, only);
        }


        private class Helper<T>
        {
            private static Delegate[] Prepare(Type type, bool flag, params Expression<Func<T, object>>[] exps)
            {
                //Type type = typeof(T);
                ParameterExpression source = Expression.Parameter(type, "source");
                ParameterExpression target = Expression.Parameter(type, "target");

                var onlyNames = exps.Select(o => {
                    switch (o.Body.NodeType)
                    {
                        case ExpressionType.MemberAccess:
                            return (o.Body as MemberExpression).Member.Name;
                        case ExpressionType.Convert:
                            return ((o.Body as UnaryExpression).Operand as MemberExpression).Member.Name;
                        default:
                            throw new NotSupportedException();
                    }
                }).ToList();

                var copyProps = from prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                where prop.CanRead && prop.CanWrite
                                && (onlyNames.Count() > 0 &&
                                    flag ? onlyNames.Contains(prop.Name) : !onlyNames.Contains(prop.Name)
                                )
                                let getExpr = Expression.Property(source, prop)
                                let setExpr = Expression.Call(target, prop.GetSetMethod(true), getExpr)
                                select Expression.Lambda(setExpr, source, target).Compile();

                return copyProps.ToArray();
            }

            public static void CopyPropertiesOnly(T source, T target, params Expression<Func<T, object>>[] include)
            {
                var cps = Prepare(source.GetType(), true, include);
                foreach (var copyProp in cps)
                {
                    copyProp.DynamicInvoke(source, target);
                }
            }

            public static void CopyPropertiesExcept(T source, T target, params Expression<Func<T, object>>[] excepts)
            {
                var cps = Prepare(source.GetType(), false, excepts);
                foreach (var copyProp in cps)
                    copyProp.DynamicInvoke(source, target);
            }
        }
    }
}
