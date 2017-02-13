using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Repository.ExpressionTree
{
    internal sealed class SingleOperationNode : NodeBase
    {
        internal NodeBase Child { get; set; }
        internal ExpressionType Operator { get; set; }
    }
}