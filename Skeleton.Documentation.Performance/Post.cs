using Skeleton.Core.Domain;
using System;

namespace Skeleton.Documentation.Performance
{
    public class Post : EntityBase<Post>
    {
        public Post()
            : base(e => e.PostId)
        {
        }

        public int? Counter1 { get; set; }
        public int? Counter2 { get; set; }
        public int? Counter3 { get; set; }
        public int? Counter4 { get; set; }
        public int? Counter5 { get; set; }
        public int? Counter6 { get; set; }
        public int? Counter7 { get; set; }
        public int? Counter8 { get; set; }
        public int? Counter9 { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; }
    }
}