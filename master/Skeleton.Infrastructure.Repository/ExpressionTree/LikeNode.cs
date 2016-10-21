namespace Skeleton.Infrastructure.Repository.ExpressionTree
{
    public sealed class LikeNode : Node
    {
        public MemberNode MemberNode { get; set; }
        public LikeMethod Method { get; set; }
        public string Value { get; set; }
    }
}