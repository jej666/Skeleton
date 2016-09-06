using System;
using System.Linq;
using System.Linq.Expressions;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Repository.ExpressionTree
{
    internal static class ExpressionResolver
    {
        internal static Node Resolve(ConstantExpression constantExpression)
        {
            return new ValueNode {Value = constantExpression.Value};
        }

        internal static Node Resolve(UnaryExpression unaryExpression)
        {
            return new SingleOperationNode
            {
                Operator = unaryExpression.NodeType,
                Child = Resolve((dynamic) unaryExpression.Operand)
            };
        }

        internal static Node Resolve(BinaryExpression binaryExpression)
        {
            return new OperationNode
            {
                Left = Resolve((dynamic) binaryExpression.Left),
                Operator = binaryExpression.NodeType,
                Right = Resolve((dynamic) binaryExpression.Right)
            };
        }

        internal static Node Resolve(MethodCallExpression callExpression)
        {
            return Resolve(callExpression, TableInfo.GetColumnName(callExpression.Object));
        }

        internal static Node Resolve(MethodCallExpression callExpression, string columnName)
        {
            LikeMethod callFunction;
            if (Enum.TryParse(callExpression.Method.Name, true, out callFunction))
            {
                var member = callExpression.Object as MemberExpression;
                var fieldValue = callExpression.Arguments.First().GetExpressionValue();

                if (fieldValue != null)
                    return new LikeNode
                    {
                        MemberNode = new MemberNode
                        {
                            TableName = TableInfo.GetTableName(member),
                            FieldName = columnName
                        },
                        Method = callFunction,
                        Value = fieldValue.ToString()
                    };
            }

            var value = callExpression.InvokeMethodCall();
            return new ValueNode {Value = value};
        }

        internal static Node Resolve(MemberExpression memberExpression, MemberExpression rootExpression = null)
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
                    return Resolve(memberExpression.Expression as MemberExpression, rootExpression);

                case ExpressionType.Call:
                case ExpressionType.Constant:
                    return new ValueNode {Value = rootExpression.GetExpressionValue()};

                default:
                    throw new ArgumentException("Expected member expression");
            }
        }

        private static void Resolve(Expression expression)
        {
            throw new ArgumentException(
                "The provided expression '{0}' is currently not supported"
                    .FormatWith(expression.NodeType));
        }
    }
}