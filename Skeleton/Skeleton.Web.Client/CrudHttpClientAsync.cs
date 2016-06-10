using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public class ReadOnlyHttpClientAsync<TEntity, TIdentity> :
        HttpClientBase<TEntity, TIdentity> where TEntity : class
    {
        public ReadOnlyHttpClientAsync(string serviceBaseAddress, string addressSuffix)
            : base(serviceBaseAddress, addressSuffix)
        {
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var responseMessage = await JsonHttpClient.GetAsync(AddressSuffix);
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadAsAsync<IEnumerable<TEntity>>();
        }

        public async Task<TEntity> GetAsync(TIdentity id)
        {
            var responseMessage = await JsonHttpClient.GetAsync(AddressSuffix + id.ToString());
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadAsAsync<TEntity>();
        }

        public async Task<TEntity> PostAsync(TEntity model)
        {
            var objectContent = CreateJsonObjectContent(model);
            var responseMessage = await JsonHttpClient.PostAsync(AddressSuffix, objectContent);

            return await responseMessage.Content.ReadAsAsync<TEntity>();
        }

        public async Task PutAsync(TIdentity id, TEntity model)
        {
            var objectContent = CreateJsonObjectContent(model);
            await JsonHttpClient.PutAsync(AddressSuffix + id.ToString(), objectContent);
        }

        public async Task DeleteAsync(TIdentity id)
        {
            await JsonHttpClient.DeleteAsync(AddressSuffix + id.ToString());
        }
    }
}
