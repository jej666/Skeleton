using System.Collections.Generic;
using System.Reflection;

namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static BinaryExpression GetBinaryExpression(this Expression expression)
        {
            expression.ThrowIfNull(() => expression);

            var binaryExpression = expression as BinaryExpression;
            if (binaryExpression != null)
                return binaryExpression;

            throw new ArgumentException("Binary expression expected");
        }

        public static object GetExpressionValue(this Expression expression)
        {
            expression.ThrowIfNull(() => expression);

            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                    return (expression as ConstantExpression).Value;

                case ExpressionType.Call:
                    return (expression as MethodCallExpression).InvokeMethodCall();

                case ExpressionType.MemberAccess:
                    var memberExpr = expression as MemberExpression;
                    var obj = GetExpressionValue(memberExpr.Expression);
                    return ((dynamic)memberExpr.Member).GetValue(obj);

                case ExpressionType.Convert:
                    var member = GetMemberExpression(expression);
                    var instance = GetExpressionValue(member.Expression);
                    return ((dynamic)member.Member).GetValue(instance);

                default:
                    throw new ArgumentException("Expected constant expression");
            }
        }

        public static MemberExpression GetMemberExpression(this Expression expression)
        {
            expression.ThrowIfNull(() => expression);

            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return expression as MemberExpression;

                case ExpressionType.Convert:
                    return GetMemberExpression((expression as UnaryExpression).Operand);
            }

            throw new ArgumentException("Member expression expected");
        }

        public static PropertyInfo GetPropertyAccess(this LambdaExpression expression)
        {
            expression.ThrowIfNull(() => expression);

            if (expression.Parameters.Count != 1)
                throw new ArgumentException("Expression must have at least 1 parameter");

            var propertyInfo =
                expression
                    .Parameters
                    .Single()
                    .MatchSimplePropertyAccess(expression.Body);

            if (propertyInfo == null)
            {
                throw new ArgumentException("PropertyInfo cannot be null");
            }

            return propertyInfo;
        }

        public static object InvokeMethodCall(this MethodCallExpression callExpression)
        {
            callExpression.ThrowIfNull(() => callExpression);

            var arguments = callExpression.Arguments.Select(GetExpressionValue).ToArray();
            var obj = callExpression.Object != null ? GetExpressionValue(callExpression.Object) : arguments.First();

            return callExpression.Method.Invoke(obj, arguments);
        }

        private static PropertyInfo[] MatchPropertyAccess(
            this Expression parameterExpression,
            Expression propertyAccessExpression)
        {
            var propertyInfos = new List<PropertyInfo>();

            MemberExpression memberExpression;

            do
            {
                memberExpression = RemoveConvert(propertyAccessExpression) as MemberExpression;

                if (memberExpression == null)
                {
                    return null;
                }

                var propertyInfo = memberExpression.Member as PropertyInfo;

                if (propertyInfo == null)
                {
                    return null;
                }

                propertyInfos.Insert(0, propertyInfo);

                propertyAccessExpression = memberExpression.Expression;
            } while (memberExpression.Expression.RemoveConvert() != parameterExpression);

            return propertyInfos.ToArray();
        }

        private static PropertyInfo MatchSimplePropertyAccess(
            this Expression parameterExpression,
            Expression propertyAccessExpression)
        {
            var propertyInfos = MatchPropertyAccess(parameterExpression, propertyAccessExpression);

            return propertyInfos != null && propertyInfos.Length == 1 ? propertyInfos[0] : null;
        }

        private static Expression RemoveConvert(this Expression expression)
        {
            while (expression != null
                   && (expression.NodeType == ExpressionType.Convert
                       || expression.NodeType == ExpressionType.ConvertChecked))
            {
                expression = RemoveConvert(((UnaryExpression)expression).Operand);
            }

            return expression;
        }
    }
}