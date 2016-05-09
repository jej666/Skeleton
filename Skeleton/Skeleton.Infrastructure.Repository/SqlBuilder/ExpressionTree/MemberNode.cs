namespace Skeleton.Infrastructure.Repository.SqlBuilder.ExpressionTree
{
    internal class MemberNode : Node
    {
        internal string FieldName { get; set; }
        internal string TableName { get; set; }
    }
}