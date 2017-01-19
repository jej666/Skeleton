using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Skeleton.Web.Client
{
    public class AsyncCrudHttpClient<TDto> :
        HttpClientBase where TDto : class
    {
        public AsyncCrudHttpClient(string serviceBaseAddress, string addressSuffix)
            : base(serviceBaseAddress, addressSuffix)
        {
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var requestUri = CreateUri("GetAll");
            var response = await JsonHttpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<IEnumerable<TDto>>();
        }

        public async Task<TDto> FirstOrDefaultAsync(object id)
        {
            var requestUri = CreateUri("Get/" + id);
            var response = await JsonHttpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<TDto>();
        }

        public async Task<TDto> AddAsync(TDto model)
        {
            var requestUri = CreateUri("Add");
            var content = CreateJsonObjectContent(model);
            var response = await JsonHttpClient.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<TDto>();
        }

        public async Task<bool> UpdateAsync(TDto model)
        {
            var requestUri = CreateUri("Update");
            var content = CreateJsonObjectContent(model);
            var response = await JsonHttpClient.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(object id)
        {
            var requestUri = CreateUri("Delete/" + id);
            var response = await JsonHttpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }

        public async Task<PagedResult<TDto>> PageAsync(int pageSize, int pageNumber)
        {
            var requestUri = CreateUri(
                             $"Page/?pageSize={pageSize}&pageNumber={pageNumber}");
            var response = await JsonHttpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PagedResult<TDto>>(content);
        }

        public async Task<IEnumerable<TDto>> AddAsync(IEnumerable<TDto> dtos)
        {
            var requestUri = CreateUri("AddMany");
            var response = await JsonHttpClient.PostAsJsonAsync(requestUri, dtos);
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsAsync<IEnumerable<TDto>>().Result;
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
            var requestUri = CreateUri(action);
            var response = await JsonHttpClient.PostAsJsonAsync(requestUri, dtos);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }
    }
}