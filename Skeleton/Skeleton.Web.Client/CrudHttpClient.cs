using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public class CrudHttpClient<TEntity, TIdentity> :
        HttpClientBase<TEntity, TIdentity> where TEntity : class
    {
        public CrudHttpClient(string serviceBaseAddress, string addressSuffix)
            : base(serviceBaseAddress, addressSuffix)
        {
        }

        public IEnumerable<TEntity> GetAll()
        {
            var responseMessage = JsonHttpClient.GetAsync(AddressSuffix).Result;
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.Content
                                  .ReadAsAsync<IEnumerable<TEntity>>()
                                  .Result;
        }

        public TEntity Get(TIdentity id)
        {
            var responseMessage = JsonHttpClient.GetAsync(AddressSuffix + id.ToString()).Result;
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage.Content.ReadAsAsync<TEntity>().Result;
        }

        public TEntity Post(TEntity model)
        {
            var objectContent = CreateJsonObjectContent(model);
            var responseMessage = JsonHttpClient.PostAsync(AddressSuffix, objectContent).Result;

            return responseMessage.Content.ReadAsAsync<TEntity>().Result;
        }

        public void Put(TIdentity id, TEntity model)
        {
            var objectContent = CreateJsonObjectContent(model);
            var responseMessage = JsonHttpClient.PutAsync(AddressSuffix + id.ToString(), objectContent).Result;
        }

        public void Delete(TIdentity id)
        {
            var responseMessage = JsonHttpClient.DeleteAsync(AddressSuffix + id.ToString()).Result;
        }
    }
}
