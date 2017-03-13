using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public class AsyncCrudHttpClient<TDto> :
        HttpClientBase where TDto : class
    {
        public AsyncCrudHttpClient(string host, string path)
            : this(host, path, Constants.DefaultHttpPort)
        {
        }

        public AsyncCrudHttpClient(string host, string path, int port)
            : base(new RestUriBuilder(host, path, port))
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var requestUri = UriBuilder.GetAll();
            var response = await GetAsync(requestUri);

            return await response
                .Content
                .ReadAsAsync<IEnumerable<TDto>>();
        }

        public async Task<TDto> FirstOrDefaultAsync(object id)
        {
            var requestUri = UriBuilder.FirstOrDefault(id);
            var response = await GetAsync(requestUri);

            return await response
                .Content
                .ReadAsAsync<TDto>();
        }

        public async Task<TDto> AddAsync(TDto dto)
        {
            var requestUri = UriBuilder.Add();
            var response = await PostAsync(requestUri, dto);

            return await response
                .Content
                .ReadAsAsync<TDto>();
        }

        public async Task<bool> UpdateAsync(TDto dto)
        {
            var requestUri = UriBuilder.Update();
            var response = await PutAsync(requestUri, dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(object id)
        {
            var requestUri = UriBuilder.Delete(id);
            var response = await base.DeleteAsync(requestUri);

            return response.IsSuccessStatusCode;
        }

        public async Task<PagedResult<TDto>> PageAsync(int pageSize, int pageNumber)
        {
            var requestUri = UriBuilder.Page(pageSize, pageNumber);
            var response = await GetAsync(requestUri);

            return await response
                .Content
                .ReadAsAsync<PagedResult<TDto>>();
        }

        public async Task<IEnumerable<TDto>> AddAsync(IEnumerable<TDto> dtos)
        {
            var requestUri = UriBuilder.AddMany();
            var response = await PostAsync(requestUri, dtos);

            return await response
                .Content
                .ReadAsAsync<IEnumerable<TDto>>();
        }

        public async Task<bool> UpdateAsync(IEnumerable<TDto> dtos)
        {
            var requestUri = UriBuilder.UpdateMany();
            var response = await PostAsync(requestUri, dtos);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(IEnumerable<TDto> dtos)
        {
            var requestUri = UriBuilder.DeleteMany();
            var response = await PostAsync(requestUri, dtos);

            return response.IsSuccessStatusCode;
        }
    }
}