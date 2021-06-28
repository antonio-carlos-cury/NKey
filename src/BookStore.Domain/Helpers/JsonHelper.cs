using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Helpers
{
    public static class JsonHelper
    {
        public static string ToDefaultJSON(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

    }
}
