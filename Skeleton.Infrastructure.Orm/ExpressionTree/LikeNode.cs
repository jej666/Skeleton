namespace Skeleton.Infrastructure.Orm.ExpressionTree
{
    internal sealed class LikeNode : NodeBase
    {
        internal MemberNode MemberNode { get; set; }
        internal LikeMethod Method { get; set; }
        internal string Value { get; set; }
    }
}