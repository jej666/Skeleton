using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Repository.ExpressionTree
{
    public sealed class SingleOperationNode : Node
    {
        public Node Child { get; set; }
        public ExpressionType Operator { get; set; }
    }
}