using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal class SingleOperationNode : Node
    {
        internal Node Child { get; set; }
        internal ExpressionType Operator { get; set; }
    }
}