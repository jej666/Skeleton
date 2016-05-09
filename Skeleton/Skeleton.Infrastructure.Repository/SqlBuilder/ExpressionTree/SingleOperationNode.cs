namespace Skeleton.Infrastructure.Repository.SqlBuilder.ExpressionTree
{
    using System.Linq.Expressions;

    internal class SingleOperationNode : Node
    {
        internal Node Child { get; set; }
        internal ExpressionType Operator { get; set; }
    }
}