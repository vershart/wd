using System;
using System.Collections.Generic;

namespace Sludinajumi.Api.Models
{
    public class CategoryProperty
    {
        public long CategoryId { get; set; }
        public long PropertyId { get; set; }
        public bool IsRequired { get; set; }

        public virtual Category Category { get; set; }
        public virtual Property Property { get; set; }
    }
}