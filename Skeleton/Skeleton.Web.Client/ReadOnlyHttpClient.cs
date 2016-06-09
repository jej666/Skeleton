//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Formatting;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;

//namespace Skeleton.Web.Client
//{
//    public class ReadOnlyHttpClient
//    {
//        public class GenericRestfulCrudHttpClient<T, TResourceIdentifier> : IDisposable where T : class
//        {
//            private bool disposed = false;
//            private HttpClient httpClient;
//            protected readonly string serviceBaseAddress;
//            private readonly string addressSuffix;
//            private readonly string jsonMediaType = "application/json";

//            public GenericRestfulCrudHttpClient(string serviceBaseAddress, string addressSuffix)
//            {
//                this.serviceBaseAddress = serviceBaseAddress;
//                this.addressSuffix = addressSuffix;
//                httpClient = MakeHttpClient(serviceBaseAddress);
//            }

//            protected virtual HttpClient MakeHttpClient(string serviceBaseAddress)
//            {
//                httpClient = new HttpClient();
//                httpClient.BaseAddress = new Uri(serviceBaseAddress);
//                httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(jsonMediaType));
//                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
//                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("defalte"));
//                httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("Skeleton_HttpClient", "1.0")));
//                return httpClient;
//            }

//            public async Task<IEnumerable<T>> GetManyAsync()
//            {
//                var responseMessage = await httpClient.GetAsync(addressSuffix);
//                responseMessage.EnsureSuccessStatusCode();
//                return await responseMessage.Content.ReadAsAsync<IEnumerable<T>>();
//            }

//            public async Task<T> GetAsync(TResourceIdentifier identifier)
//            {
//                var responseMessage = await httpClient.GetAsync(addressSuffix + identifier.ToString());
//                responseMessage.EnsureSuccessStatusCode();
//                return await responseMessage.Content.ReadAsAsync<T>();
//            }

//            public async Task<T> PostAsync(T model)
//            {
//                var requestMessage = new HttpRequestMessage();
//                var objectContent = CreateJsonObjectContent(model);
//                var responseMessage = await httpClient.PostAsync(addressSuffix, objectContent);
//                return await responseMessage.Content.ReadAsAsync<T>();
//            }

//            public async Task PutAsync(TResourceIdentifier identifier, T model)
//            {
//                var requestMessage = new HttpRequestMessage();
//                var objectContent = CreateJsonObjectContent(model);
//                var responseMessage = await httpClient.PutAsync(addressSuffix + identifier.ToString(), objectContent);
//            }

//            public async Task DeleteAsync(TResourceIdentifier identifier)
//            {
//                var r = await httpClient.DeleteAsync(addressSuffix + identifier.ToString());
//            }

//            private ObjectContent CreateJsonObjectContent(T model)
//            {
//                var requestMessage = new HttpRequestMessage();
//                return requestMessage.CreateContent<T>(
//                    model,
//                    MediaTypeHeaderValue.Parse(jsonMediaType),
//                    new MediaTypeFormatter[] { new JsonMediaTypeFormatter() },
//                    new FormatterSelector());
//            }

//            public void Dispose()
//            {
//                Dispose(true);
//                GC.SuppressFinalize(this);
//            }

//            private void Dispose(bool disposing)
//            {
//                if (!disposed && disposing)
//                {
//                    if (httpClient != null)
//                    {
//                        var hc = httpClient;
//                        httpClient = null;
//                        hc.Dispose();
//                    }
//                    disposed = true;
//                }
//            }
//        }
//    }
//}
