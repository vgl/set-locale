using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace set.locale.Data.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public string UrlName { get; set; }
    }
}