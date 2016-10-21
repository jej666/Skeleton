using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Repository.ExpressionTree
{
    public sealed class OperationNode : Node
    {
        public Node Left { get; set; }
        public ExpressionType Operator { get; set; }
        public Node Right { get; set; }
    }
}