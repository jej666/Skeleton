using System;
using System.Linq;
using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal static class TableInfo
    {
        internal static string GetColumnName<T>(Expression<Func<T, object>> selector)
        {
            return GetColumnName(selector.Body.GetMemberExpression());
        }

        internal static string GetColumnName(Expression expression)
        {
            var member = expression.GetMemberExpression();
            var name = member.Member.Name;
            return name;
        }

        internal static string GetTableName<T>()
        {
            return GetTableName(typeof(T));
        }

        internal static string GetTableName(Type type)
        {
            return type.Name;
        }

        internal static string GetTableName(MemberExpression expression)
        {
            var reflectedType = expression.Member.ReflectedType;

            if (reflectedType != null && reflectedType.IsGenericType)
                reflectedType = reflectedType.GenericTypeArguments.First();

            return GetTableName(reflectedType);
        }
    }
}