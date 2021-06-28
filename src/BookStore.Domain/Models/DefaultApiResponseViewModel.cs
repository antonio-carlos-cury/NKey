using BookStore.Domain.Interfaces;
using System.Collections.Generic;

namespace BookStore.Domain.Models
{
    public class DefaultApiResponseViewModel : IDefaultApiResponse
    {
        public bool success { get; set; }
        public object data { get; set; }
        public IEnumerable<string> errors { get; set; }
    }
}
