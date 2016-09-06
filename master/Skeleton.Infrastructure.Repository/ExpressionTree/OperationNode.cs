using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Repository.ExpressionTree
{
    internal class OperationNode : Node
    {
        internal Node Left { get; set; }
        internal ExpressionType Operator { get; set; }
        internal Node Right { get; set; }
    }
}