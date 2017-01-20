namespace Skeleton.Infrastructure.Repository.ExpressionTree
{
    internal sealed class MemberNode : Node
    {
        internal string FieldName { get; set; }
        internal string TableName { get; set; }
    }
}