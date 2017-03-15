using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Orm.ExpressionTree
{
    internal sealed class SingleOperationNode : NodeBase
    {
        internal NodeBase Child { get; set; }
        internal ExpressionType Operator { get; set; }
    }
}