using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace Skeleton.Web.Client
{
    public class CrudHttpClient<TDto> :
        JsonHttpClient where TDto : class
    {
        public CrudHttpClient(string host, string path)
            : this(host, path, Constants.DefaultHttpPort)
        {
        }

        public CrudHttpClient(string host, string path, int port)
            : base(new RestUriBuilder(host, path, port))
        {
        }

        public CrudHttpClient(UriBuilder uriBuilder)
            : base(new RestUriBuilder(uriBuilder))
        {
        }

        public CrudHttpClient(IRestUriBuilder uriBuilder)
            : base(uriBuilder)
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IEnumerable<TDto> GetAll()
        {
            var requestUri = UriBuilder.GetAll();
            var response = Get(requestUri);

            return response.Content
                .ReadAsAsync<IEnumerable<TDto>>()
                .Result;
        }

        public TDto FirstOrDefault(object id)
        {
            var requestUri = UriBuilder.FirstOrDefault(id);
            var content = Get(requestUri).Content;

            return content
                .ReadAsAsync<TDto>()
                .Result;
        }

        public QueryResult<TDto> Query(Query query)
        {
            var requestUri = UriBuilder.Query(query);
            var content = Get(requestUri).Content;

            return content
                .ReadAsAsync<QueryResult<TDto>>()
                .Result;
        }

        public TDto Create(TDto dto)
        {
            var requestUri = UriBuilder.Create();
            var content = Post(requestUri, dto).Content;

            return content
                .ReadAsAsync<TDto>()
                .Result;
        }

        public IEnumerable<TDto> Create(IEnumerable<TDto> dtos)
        {
            var requestUri = UriBuilder.BatchCreate();
            var content = Post(requestUri, dtos).Content;

            return content
                .ReadAsAsync<IEnumerable<TDto>>()
                .Result;
        }

        public bool Update(TDto dto)
        {
            var requestUri = UriBuilder.Update();
            var response = Put(requestUri, dto);

            return response.IsSuccessStatusCode;
        }

        public bool Update(IEnumerable<TDto> dtos)
        {
            var requestUri = UriBuilder.BatchUpdate();
            var response = Post(requestUri, dtos);

            return response.IsSuccessStatusCode;
        }

        public bool Delete(object id)
        {
            var requestUri = UriBuilder.Delete(id);
            var response = base.Delete(requestUri);

            return response.IsSuccessStatusCode;
        }

        public bool Delete(IEnumerable<TDto> dtos)
        {
            var requestUri = UriBuilder.BatchDelete();
            var response = Post(requestUri, dtos);

            return response.IsSuccessStatusCode;
        }
    }
}