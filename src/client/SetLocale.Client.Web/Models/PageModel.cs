using System.Collections.Generic;

namespace SetLocale.Client.Web.Models
{
    public class PageModel<T> where T : BaseModel
    {
        public int Number { get; set; }
        public long TotalCount { get; set; }
        public int TotalPageCount { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }

        public List<T> Items { get; set; }
    }
}