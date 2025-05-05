using System;

namespace Diplom.BLL.Models
{
    public class Item
    {
        public Guid id { get; set; }
        public required string name { get; set; }
        public string? description { get; set; }
        public string? category_id { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public string? barcode { get; set; }
    }
}
