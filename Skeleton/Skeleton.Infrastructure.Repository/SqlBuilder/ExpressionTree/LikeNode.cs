namespace Skeleton.Infrastructure.Repository.SqlBuilder.ExpressionTree
{
    internal class LikeNode : Node
    {
        internal MemberNode MemberNode { get; set; }
        internal LikeMethod Method { get; set; }
        internal string Value { get; set; }
    }
}