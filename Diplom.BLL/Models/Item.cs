using System;

namespace Diplom.BLL.Models
{
    public class Item
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public string? category_id { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public string? barcode { get; set; }
        public bool is_deleted { get; set; }
        public string? image {  get; set; }
        public Item()
        {
            id = Guid.NewGuid();
            name = string.Empty;
            description = string.Empty;
            category_id = string.Empty;
            price = 0;
            quantity = 0;
            barcode = string.Empty;
            is_deleted = false;
            image = string.Empty;
        }
    }
}
