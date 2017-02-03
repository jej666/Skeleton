using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace Skeleton.Web.Client
{
    public class CrudHttpClient<TDto> :
        HttpClientBase where TDto : class
    {
        public CrudHttpClient(string serviceBaseAddress, string addressSuffix)
            : base(serviceBaseAddress, addressSuffix)
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IEnumerable<TDto> GetAll()
        {
            var requestUri = CreateUri("GetAll");
            var responseMessage = JsonHttpClient.GetAsync(requestUri).Result;
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.Content
                .ReadAsAsync<IEnumerable<TDto>>()
                .Result;
        }

        public TDto FirstOrDefault(object id)
        {
            var requestUri = CreateUri("Get/" + id);
            var responseMessage = JsonHttpClient.GetAsync(requestUri).Result;
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.Content.ReadAsAsync<TDto>().Result;
        }

        public PagedResult<TDto> Page(int pageSize, int pageNumber)
        {
            var requestUri = CreateUri(
                             $"Page/?pageSize={pageSize}&pageNumber={pageNumber}");
            var responseMessage = JsonHttpClient.GetAsync(requestUri).Result;
            responseMessage.EnsureSuccessStatusCode();

            var content = responseMessage.Content
                .ReadAsStringAsync()
                .Result;

            return JsonConvert.DeserializeObject<PagedResult<TDto>>(content);
        }

        public TDto Add(TDto dto)
        {
            var request = CreateUri("Add");
            var objectContent = CreateJsonObjectContent(dto);
            var responseMessage = JsonHttpClient.PostAsync(request, objectContent).Result;
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.Content.ReadAsAsync<TDto>().Result;
        }

        public IEnumerable<TDto> Add(IEnumerable<TDto> dtos)
        {
            var request = CreateUri("AddMany");
            var content = CreateJsonObjectContent(dtos);
            var response = JsonHttpClient.PostAsync(request, content).Result;
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsAsync<IEnumerable<TDto>>().Result;
        }

        public bool Update(TDto dto)
        {
            var request = CreateUri("Update");
            var content = CreateJsonObjectContent(dto);
            var response = JsonHttpClient.PostAsync(request, content).Result;
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }

        public bool Update(IEnumerable<TDto> dtos)
        {
            return Post(dtos, "UpdateMany");
        }

        public bool Delete(object id)
        {
            var request = CreateUri("Delete/" + id);
            var response = JsonHttpClient.GetAsync(request).Result;
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }

        public bool Delete(IEnumerable<TDto> dtos)
        {
            return Post(dtos, "DeleteMany");
        }

        private bool Post(IEnumerable<TDto> dtos, string action)
        {
            var request = CreateUri(action);
            var response = JsonHttpClient.PostAsJsonAsync(request, dtos).Result;
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }
    }
}