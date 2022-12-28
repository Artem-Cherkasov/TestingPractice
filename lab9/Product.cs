using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab9.Entities
{
    public class Product
    {
        public int? id { get; set; }
        public int? category_id { get; set; }
        public string? title { get; set; }
        public string? alias { get; set; }
        public string? content { get; set; }
        public int? price { get; set; }
        public int? old_price { get; set; }
        public int? status { get; set; }
        public string? keywords { get; set; }
        public string? description { get; set; }
        public int? hit { get; set; }
    }
}
