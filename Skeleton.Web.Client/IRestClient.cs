using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public interface IRestClient : IDisposable
    {
        AuthenticationHeaderValue Authentication { get; set; }
        Uri BaseAddress { get; }
        IRetryPolicy RetryPolicy { get; }

        IRestResponse Delete(IRestRequest request);
        IRestResponse Delete(Action<IRestRequest> requestBuilder);
        Task<IRestResponse> DeleteAsync(IRestRequest request);
        Task<IRestResponse> DeleteAsync(Action<IRestRequest> requestBuilder);

        IRestResponse Get(IRestRequest request);
        IRestResponse Get(Action<IRestRequest> requestBuilder);
        T Get<T>(IRestRequest request);
        T Get<T>(Action<IRestRequest> requestBuilder);
        Task<IRestResponse> GetAsync(IRestRequest request);
        Task<IRestResponse> GetAsync(Action<IRestRequest> requestBuilder);
        Task<T> GetAsync<T>(IRestRequest request);
        Task<T> GetAsync<T>(Action<IRestRequest> requestBuilder);

        IRestResponse Post(IRestRequest request);
        IRestResponse Post(Action<IRestRequest> requestBuilder);
        T Post<T>(IRestRequest request);
        T Post<T>(Action<IRestRequest> requestBuilder);
        Task<IRestResponse> PostAsync(IRestRequest request);
        Task<IRestResponse> PostAsync(Action<IRestRequest> requestBuilder);
        Task<T> PostAsync<T>(IRestRequest request);
        Task<T> PostAsync<T>(Action<IRestRequest> requestBuilder);

        IRestResponse Put(IRestRequest request);
        IRestResponse Put(Action<IRestRequest> requestBuilder);
        Task<IRestResponse> PutAsync(IRestRequest request);
        Task<IRestResponse> PutAsync(Action<IRestRequest> requestBuilder);
    }
}