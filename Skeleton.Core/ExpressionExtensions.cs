using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Skeleton.Core
{
    public static class ExpressionExtensions
    {
        public static BinaryExpression GetBinaryExpression(this Expression expression)
        {
            expression.ThrowIfNull();

            var binaryExpression = expression as BinaryExpression;
            if (binaryExpression != null)
                return binaryExpression;

            throw new ArgumentException("Binary expression expected");
        }

        public static object GetExpressionValue(this Expression expression)
        {
            expression.ThrowIfNull();

            var constantExpression = expression as ConstantExpression;
            if (constantExpression != null)
                return constantExpression.Value;

            var methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression != null)
                return methodCallExpression.InvokeMethodCall();

            var memberExpression = expression as MemberExpression;
            if (memberExpression != null)
            {
                var memberValue = memberExpression.Expression.GetExpressionValue();
                return memberExpression.GetValue(memberValue);
            }

            if (expression.NodeType == ExpressionType.Convert)
            {
                var member = GetMemberExpression(expression);
                var instance = GetExpressionValue(member.Expression);

                return member.GetValue(instance);
            }
            return null;
        }

        private static object GetValue(this MemberExpression expression, object instance)
        {
            var memberInfo = expression.Member;
            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
                return propertyInfo.GetValue(instance);

            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
                return fieldInfo.GetValue(instance);

            return null;
        }

        public static MemberExpression GetMemberExpression(this Expression expression)
        {
            expression.ThrowIfNull();

            while (true)
            {
                if (expression.NodeType == ExpressionType.MemberAccess)
                    return expression as MemberExpression;

                if (expression.NodeType == ExpressionType.Convert)
                {
                    var unaryExpression = expression as UnaryExpression;
                    if (unaryExpression != null)
                        expression = unaryExpression.Operand;
                    continue;
                }

                throw new ArgumentException("Member expression expected");
            }
        }

        public static PropertyInfo GetPropertyAccess(this LambdaExpression expression)
        {
            expression.ThrowIfNull();

            if (expression.Parameters.Count != 1)
                throw new ArgumentException("Expression must have at least 1 parameter");

            var propertyInfo =
                expression
                    .Parameters
                    .Single()
                    .MatchSimplePropertyAccess(expression.Body);

            if (propertyInfo == null)
                throw new ArgumentException("PropertyInfo cannot be null");

            return propertyInfo;
        }

        public static object InvokeMethodCall(this MethodCallExpression callExpression)
        {
            callExpression.ThrowIfNull();

            var arguments = callExpression.Arguments.Select(GetExpressionValue).ToArray();
            var obj = callExpression.Object != null
                ? GetExpressionValue(callExpression.Object)
                : arguments.First();

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
                    return null;

                var propertyInfo = memberExpression.Member as PropertyInfo;

                if (propertyInfo == null)
                    return null;

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

            return (propertyInfos != null) && (propertyInfos.Length == 1) ? propertyInfos[0] : null;
        }

        private static Expression RemoveConvert(this Expression expression)
        {
            while ((expression != null)
                   && ((expression.NodeType == ExpressionType.Convert)
                       || (expression.NodeType == ExpressionType.ConvertChecked)))
                expression = RemoveConvert(((UnaryExpression)expression).Operand);

            return expression;
        }
    }
}