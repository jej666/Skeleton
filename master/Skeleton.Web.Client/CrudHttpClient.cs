using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace Skeleton.Web.Client
{
    public class CrudHttpClient<TDto> :
        HttpClientBase where TDto : class
    {
        public CrudHttpClient(string host, string path)
            : this (host, path, 80)
        {
        }

        public CrudHttpClient(string host, string path, int port)
           : base(new RestUriBuilder(host, path, port))
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IEnumerable<TDto> GetAll()
        {
            var requestUri = UriBuilder.GetAll();
            var content = Get(requestUri).Content;

            return content
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

        public PagedResult<TDto> Page(int pageSize, int pageNumber)
        {
            var requestUri = UriBuilder.Page(pageSize, pageNumber);
            var content = Get(requestUri).Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<PagedResult<TDto>>(content);
        }

        public TDto Add(TDto dto)
        {
            var requestUri = UriBuilder.Add();
            var content = Post(requestUri, dto).Content;
            
            return content.ReadAsAsync<TDto>().Result;
        }

        public IEnumerable<TDto> Add(IEnumerable<TDto> dtos)
        {
            var requestUri = UriBuilder.AddMany();
            var content = Post(requestUri, dtos).Content;

            return content.ReadAsAsync<IEnumerable<TDto>>().Result;
        }

        public bool Update(TDto dto)
        {
            var requestUri = UriBuilder.Update();
            var response = Post(requestUri, dto);
            
            return response.IsSuccessStatusCode;
        }

        public bool Update(IEnumerable<TDto> dtos)
        {
            var requestUri = UriBuilder.UpdateMany();
            var response = Post(requestUri, dtos);

            return response.IsSuccessStatusCode;
        }

        public bool Delete(object id)
        {
            var requestUri = UriBuilder.Delete(id);          
            var response = Get(requestUri);
            
            return response.IsSuccessStatusCode;
        }

        public bool Delete(IEnumerable<TDto> dtos)
        {
            var requestUri = UriBuilder.DeleteMany();
            var response = Post(requestUri, dtos);

            return response.IsSuccessStatusCode;
        }
    }
}