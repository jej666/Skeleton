using Skeleton.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton.Common
{
    public class QueryResult<TDto> where TDto : class
    {
        private readonly IQuery _query;

        public QueryResult(IEnumerable<TDto> items, IQuery query)
        {
            _query = query;
            Items = items;
            TotalCount = items.Count();
            CalculatePagination();
        }

        private void CalculatePagination()
        {
            if (!_query.PageSize.HasValue)
            {
                PageSize = -1;
                PageNumber = -1;
                TotalPages = -1;
            }
            else
            {
                PageSize = _query.PageSize.Value;
                PageNumber = _query.PageNumber.Value;
                TotalPages = (int)Math.Ceiling((double)TotalCount / PageSize);
            }
        }

        public string PrevPageLink { get; set; }

        public string NextPageLink { get; set; }

        public IEnumerable<TDto> Items { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
    }
}
