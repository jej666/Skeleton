using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace Skeleton.Web.Client
{
    public class CrudHttpClient<TDto> :
        HttpClientBase where TDto : class
    {
        public CrudHttpClient(string serviceBaseAddress, string addressSuffix)
            : base(serviceBaseAddress, addressSuffix)
        {
        }

        public IEnumerable<TDto> GetAll()
        {
            var responseMessage = JsonHttpClient.GetAsync(AddressSuffix + "GetAll").Result;
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.Content
                .ReadAsAsync<IEnumerable<TDto>>()
                .Result;
        }

        public TDto FirstOrDefault(object id)
        {
            var responseMessage = JsonHttpClient.GetAsync(AddressSuffix + "Get/" + id).Result;
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.Content.ReadAsAsync<TDto>().Result;
        }

        public PagedResult<TDto> Page(int pageSize, int pageNumber)
        {
            var requestUri = AddressSuffix +
                             $"Page/?pageSize={pageSize}&pageNumber={pageNumber}";
            var responseMessage = JsonHttpClient.GetAsync(requestUri).Result;
            responseMessage.EnsureSuccessStatusCode();

            var content = responseMessage.Content
                .ReadAsStringAsync()
                .Result;
            return JsonConvert.DeserializeObject<PagedResult<TDto>>(content);
        }

        public TDto Add(TDto dto)
        {
            var objectContent = CreateJsonObjectContent(dto);
            var responseMessage = JsonHttpClient.PostAsync(AddressSuffix + "Add", objectContent).Result;
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.Content.ReadAsAsync<TDto>().Result;
        }

        public IEnumerable<TDto> Add(IEnumerable<TDto> dtos)
        {
            var objectContent = CreateJsonObjectContent(dtos);
            var responseMessage = JsonHttpClient.PostAsync(AddressSuffix + "AddMany", objectContent).Result;
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.Content.ReadAsAsync<IEnumerable<TDto>>().Result;
        }

        public bool Update(TDto dto)
        {
            var objectContent = CreateJsonObjectContent(dto);
            var responseMessage = JsonHttpClient.PostAsync(AddressSuffix + "Update", objectContent).Result;
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.IsSuccessStatusCode;
        }

        public bool Update(IEnumerable<TDto> dtos)
        {
            return Post(dtos, "UpdateMany");
        }

        public bool Delete(object id)
        {
            var responseMessage = JsonHttpClient.GetAsync(AddressSuffix + "Delete/" + id).Result;
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.IsSuccessStatusCode;
        }

        public bool Delete(IEnumerable<TDto> dtos)
        {
            return Post(dtos, "DeleteMany");
        }

        private bool Post(IEnumerable<TDto> dtos, string action)
        {
            var responseMessage = JsonHttpClient.PostAsJsonAsync(AddressSuffix + action, dtos).Result;
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.IsSuccessStatusCode;
        }
    }
}