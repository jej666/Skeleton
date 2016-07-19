using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public class ReadOnlyHttpClientAsync<TDto, TId> :
        HttpClientBase where TDto : class
    {
        public ReadOnlyHttpClientAsync(string serviceBaseAddress, string addressSuffix)
            : base(serviceBaseAddress, addressSuffix)
        {
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var responseMessage = await JsonHttpClient.GetAsync(AddressSuffix);
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadAsAsync<IEnumerable<TDto>>();
        }

        public async Task<TDto> GetAsync(TId id)
        {
            var responseMessage = await JsonHttpClient.GetAsync(AddressSuffix + id);
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadAsAsync<TDto>();
        }

        public async Task<TDto> PostAsync(TDto model)
        {
            var objectContent = CreateJsonObjectContent(model);
            var responseMessage = await JsonHttpClient.PostAsync(AddressSuffix, objectContent);

            return await responseMessage.Content.ReadAsAsync<TDto>();
        }

        public async Task PutAsync(TId id, TDto model)
        {
            var objectContent = CreateJsonObjectContent(model);
            await JsonHttpClient.PutAsync(AddressSuffix + id, objectContent);
        }

        public async Task DeleteAsync(TId id)
        {
            await JsonHttpClient.DeleteAsync(AddressSuffix + id);
        }
    }
}
