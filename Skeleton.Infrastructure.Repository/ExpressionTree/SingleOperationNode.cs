using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Repository.ExpressionTree
{
    internal sealed class SingleOperationNode : Node
    {
        internal Node Child { get; set; }
        internal ExpressionType Operator { get; set; }
    }
}