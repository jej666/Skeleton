namespace Skeleton.Infrastructure.Repository.SqlBuilder.ExpressionTree
{
    using System.Linq.Expressions;

    internal class OperationNode : Node
    {
        internal Node Left { get; set; }
        internal ExpressionType Operator { get; set; }
        internal Node Right { get; set; }
    }
}