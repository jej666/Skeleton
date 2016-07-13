using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace Skeleton.Web.Client
{
    public class CrudHttpClient<TDto, TId> :
        HttpClientBase<TDto> where TDto : class
    {
        public CrudHttpClient(string serviceBaseAddress, string addressSuffix)
            : base(serviceBaseAddress, addressSuffix)
        {
        }

        public IEnumerable<TDto> Get()
        {
            var responseMessage = JsonHttpClient.GetAsync(AddressSuffix).Result;
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.Content
                                  .ReadAsAsync<IEnumerable<TDto>>()
                                  .Result;
        }

        public TDto Get(TId id)
        {
            var responseMessage = JsonHttpClient.GetAsync(AddressSuffix + id).Result;
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.Content.ReadAsAsync<TDto>().Result;
        }

        public PagedResult<TDto> Get(int pageSize, int pageNumber)
        {
            var requestUri = AddressSuffix  +
                string.Format("Page/?pageSize={0}&pageNumber={1}", pageSize,pageNumber);
            var responseMessage = JsonHttpClient.GetAsync(requestUri).Result;

            responseMessage.EnsureSuccessStatusCode();

            var content = responseMessage.Content
                                        .ReadAsStringAsync()
                                        .Result;
            var data = JsonConvert.DeserializeObject<PagedResult<TDto>>(content);

            return data;
        }

        public TDto Add(TDto model)
        {
            var objectContent = CreateJsonObjectContent(model);
            var responseMessage = JsonHttpClient.PostAsync(AddressSuffix, objectContent).Result;

            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.Content.ReadAsAsync<TDto>().Result;
        }

        public IEnumerable<TDto> Add(IEnumerable<TDto> dtos)
        {
            var responseMessage = JsonHttpClient.PostAsJsonAsync(AddressSuffix + "AddMany", dtos).Result;

            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.Content.ReadAsAsync<IEnumerable<TDto>>().Result;
        }

        public bool Update(TId id, TDto model)
        {
            var objectContent = CreateJsonObjectContent(model);
            var responseMessage = JsonHttpClient.PutAsync(AddressSuffix + id, objectContent).Result;

            return responseMessage.IsSuccessStatusCode;
        }

        public bool Delete(TId id)
        {
            var responseMessage = JsonHttpClient.DeleteAsync(AddressSuffix + id).Result;

            return responseMessage.IsSuccessStatusCode;
        }
    }
}
