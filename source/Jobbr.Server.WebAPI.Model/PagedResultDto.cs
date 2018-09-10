using System.Collections.Generic;

namespace Jobbr.Server.WebAPI.Model
{
    public class PagedResultDto<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
    }
}
