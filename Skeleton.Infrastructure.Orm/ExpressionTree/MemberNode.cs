﻿namespace Skeleton.Infrastructure.Orm.ExpressionTree
{
    internal sealed class MemberNode : NodeBase
    {
        internal string FieldName { get; set; }
        internal string TableName { get; set; }
    }
}