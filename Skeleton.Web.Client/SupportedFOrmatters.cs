using System.Linq;
using System.Net.Http.Formatting;

namespace Skeleton.Web.Client
{
    public sealed class SupportedFormatters : MediaTypeFormatterCollection
    {
        public SupportedFormatters()
        {
            if (!this.Any())
            {
                return;
            }

            var jsonFormatter = (JsonMediaTypeFormatter)this[0];
            jsonFormatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            jsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
        }
    }
}