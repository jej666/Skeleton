﻿using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Skeleton.Web.Client
{
    public sealed class JsonObjectContent : StringContent 
    {
        public JsonObjectContent(object dto)
            : base(Serialize(dto), Encoding.UTF8, Constants.JsonMediaType)
        {
        }

        private static string Serialize(object dto) 
        {
            return JsonConvert.SerializeObject(dto);
        }
    }
}
