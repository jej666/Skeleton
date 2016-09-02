using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Skeleton.Web.Client
{
    public class AsyncCrudHttpClient<TDto, TId> :
        HttpClientBase where TDto : class
    {
        public AsyncCrudHttpClient(string serviceBaseAddress, string addressSuffix)
            : base(serviceBaseAddress, addressSuffix)
        {
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var responseMessage = await JsonHttpClient.GetAsync(AddressSuffix);
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadAsAsync<IEnumerable<TDto>>();
        }

        public async Task<TDto> FirstOrDefaultAsync(TId id)
        {
            var responseMessage = await JsonHttpClient.GetAsync(AddressSuffix + id);
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadAsAsync<TDto>();
        }

        public async Task<TDto> AddAsync(TDto model)
        {
            var objectContent = CreateJsonObjectContent(model);
            var responseMessage = await JsonHttpClient.PostAsync(AddressSuffix, objectContent);
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadAsAsync<TDto>();
        }

        public async Task<bool> UpdateAsync(TId id, TDto model)
        {
            var objectContent = CreateJsonObjectContent(model);
            var responseMessage = await JsonHttpClient.PutAsync(AddressSuffix + id, objectContent);
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(TId id)
        {
            var responseMessage = await JsonHttpClient.DeleteAsync(AddressSuffix + id);
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<PagedResult<TDto>> PageAsync(int pageSize, int pageNumber)
        {
            var requestUri = AddressSuffix +
                             $"Page/?pageSize={pageSize}&pageNumber={pageNumber}";
            var responseMessage = await JsonHttpClient.GetAsync(requestUri);
            responseMessage.EnsureSuccessStatusCode();

            var content = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PagedResult<TDto>>(content);
        }
        
        public async Task<IEnumerable<TDto>> AddAsync(IEnumerable<TDto> dtos)
        {
            var responseMessage = await JsonHttpClient.PostAsJsonAsync(AddressSuffix + "AddMany", dtos);
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.Content.ReadAsAsync<IEnumerable<TDto>>().Result;
        }
        
        public async Task<bool> UpdateAsync(IEnumerable<TDto> dtos)
        {
            return await Post(dtos, "UpdateMany");
        }

        public async Task<bool> DeleteAsync(IEnumerable<TDto> dtos)
        {
            return await Post(dtos, "DeleteMany");
        }

        private async Task<bool> Post(IEnumerable<TDto> dtos, string action)
        {
            var responseMessage = await JsonHttpClient.PostAsJsonAsync(AddressSuffix + action, dtos);
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.IsSuccessStatusCode;
        }
    }
}
