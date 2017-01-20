namespace Skeleton.Infrastructure.Repository.ExpressionTree
{
    internal sealed class LikeNode : Node
    {
        internal MemberNode MemberNode { get; set; }
        internal LikeMethod Method { get; set; }
        internal string Value { get; set; }
    }
}