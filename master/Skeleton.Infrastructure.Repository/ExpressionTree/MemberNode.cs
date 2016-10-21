namespace Skeleton.Infrastructure.Repository.ExpressionTree
{
    public sealed class MemberNode : Node
    {
        public string FieldName { get; set; }
        public string TableName { get; set; }
    }
}