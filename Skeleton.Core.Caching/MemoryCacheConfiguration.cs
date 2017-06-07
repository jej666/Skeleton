using Skeleton.Abstraction;
using Skeleton.Common;
using System;

namespace Skeleton.Core.Caching
{
    public sealed class MemoryCacheConfiguration : HideObjectMethodsBase, ICacheConfiguration
    {
        public DateTimeOffset? AbsoluteExpiration { get; private set; }
        public DateTimeOffset CreationTime { get; set; }
        public TimeSpan? SlidingExpiration { get; private set; }

        public void SetAbsoluteExpiration(TimeSpan relative)
        {
            if (relative <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(
                    nameof(relative), relative,
                    "The relative expiration value must be positive.");

            SetAbsoluteExpiration(CreationTime + relative);
        }

        public void SetAbsoluteExpiration(DateTimeOffset absolute)
        {
            if (absolute <= CreationTime)
                throw new ArgumentOutOfRangeException(
                    nameof(absolute), absolute,
                    "The absolute expiration value must be in the future.");

            if (!AbsoluteExpiration.HasValue)
                AbsoluteExpiration = absolute;
            else if (absolute < AbsoluteExpiration.Value)
                AbsoluteExpiration = absolute;
        }

        public void SetSlidingExpiration(TimeSpan offset)
        {
            if (offset <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(
                    nameof(offset), offset,
                    "The sliding expiration value must be positive.");

            SlidingExpiration = offset;
        }
    }
}