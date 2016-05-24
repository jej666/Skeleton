using System;
using System.Linq;
using System.Linq.Expressions;
using Skeleton.Common.Extensions;

namespace Skeleton.Infrastructure.Repository.SqlBuilder.ExpressionTree
{
    internal static class ExpressionResolver
    {
        internal static Node ResolveQuery(ConstantExpression constantExpression)
        {
            return new ValueNode {Value = constantExpression.Value};
        }

        internal static Node ResolveQuery(UnaryExpression unaryExpression)
        {
            return new SingleOperationNode
            {
                Operator = unaryExpression.NodeType,
                Child = ResolveQuery((dynamic) unaryExpression.Operand)
            };
        }

        internal static Node ResolveQuery(BinaryExpression binaryExpression)
        {
            return new OperationNode
            {
                Left = ResolveQuery((dynamic) binaryExpression.Left),
                Operator = binaryExpression.NodeType,
                Right = ResolveQuery((dynamic) binaryExpression.Right)
            };
        }

        internal static Node ResolveQuery(MethodCallExpression callExpression)
        {
            return ResolveQuery(callExpression, TableInfo.GetColumnName(callExpression.Object));
        }

        internal static Node ResolveQuery(MethodCallExpression callExpression, string columnName)
        {
            LikeMethod callFunction;
            if (Enum.TryParse(callExpression.Method.Name, true, out callFunction))
            {
                var member = callExpression.Object as MemberExpression;
                var fieldValue = callExpression.Arguments.First().GetExpressionValue().ToString();

                return new LikeNode
                {
                    MemberNode = new MemberNode
                    {
                        TableName = TableInfo.GetTableName(member),
                        FieldName = columnName
                    },
                    Method = callFunction,
                    Value = fieldValue
                };
            }
            var value = callExpression.InvokeMethodCall();
            return new ValueNode {Value = value};
        }

        internal static Node ResolveQuery(MemberExpression memberExpression, MemberExpression rootExpression = null)
        {
            rootExpression = rootExpression ?? memberExpression;
            switch (memberExpression.Expression.NodeType)
            {
                case ExpressionType.Parameter:
                    return new MemberNode
                    {
                        TableName = TableInfo.GetTableName(rootExpression),
                        FieldName = TableInfo.GetColumnName(rootExpression)
                    };

                case ExpressionType.MemberAccess:
                    return ResolveQuery(memberExpression.Expression as MemberExpression, rootExpression);

                case ExpressionType.Call:
                case ExpressionType.Constant:
                    return new ValueNode {Value = rootExpression.GetExpressionValue()};

                default:
                    throw new ArgumentException("Expected member expression");
            }
        }

        private static void ResolveQuery(Expression expression)
        {
            throw new ArgumentException(
                "The provided expression '{0}' is currently not supported"
                    .FormatWith(expression.NodeType));
        }
    }
}