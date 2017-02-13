using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Repository.ExpressionTree
{
    internal sealed class OperationNode : NodeBase
    {
        internal NodeBase Left { get; set; }
        internal ExpressionType Operator { get; set; }
        internal NodeBase Right { get; set; }
    }
}