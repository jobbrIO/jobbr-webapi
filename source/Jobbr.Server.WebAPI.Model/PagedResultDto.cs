using System.Collections.Generic;

namespace Jobbr.Server.WebAPI.Model
{
    /// <summary>
    /// Paged result data transfer object.
    /// </summary>
    /// <typeparam name="T">Containing object type.</typeparam>
    public class PagedResultDto<T>
    {
        /// <summary>
        /// Page number.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Contained items.
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// The total amount of items.
        /// </summary>
        public int TotalItems { get; set; }
    }
}
