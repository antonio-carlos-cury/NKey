using System.Collections.Generic;

namespace BookStore.Domain.Interfaces
{
    public interface IDefaultApiResponse
    {
        bool success { get; set; }
        object data { get; set; }
        IEnumerable<string> errors { get; set; }
    }
}
