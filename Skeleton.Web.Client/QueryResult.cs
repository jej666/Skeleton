﻿using System.Collections.Generic;

namespace Skeleton.Web.Client
{
    public sealed class QueryResult<TDto> where TDto : class
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public string PrevPageLink { get; set; }
        public string NextPageLink { get; set; }
        public IEnumerable<TDto> Items { get; set; }
    }
}