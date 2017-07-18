using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Skeleton.Web.Client
{
    public sealed class JsonObjectContent : StringContent 
    {
        public JsonObjectContent(object value)
            : base(Serialize(value), Encoding.UTF8, Constants.JsonMediaType)
        {
        }

        private static string Serialize(object value) 
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}