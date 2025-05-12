using Diplom.BLL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Diplom.DAL
{
    public class DataBase : ICrud
    {
        private readonly DataBaseContext _context;
        public DataBase()
        {
            _context = new DataBaseContext();
        }

        public bool Create(Item item)
        {
            _context.table_items.Add(item);
            return _context.SaveChanges() > 0;
        }

        public bool Update(Item item)
        {
            _context.table_items.Update(item);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(Guid id)
        {
            _context.table_items.Remove(_context.table_items.Find(id));
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Item> GetAll()
        {
            var items = (from i in _context.table_items
                         join c in _context.table_category on i.category_id equals c.id.ToString()
                         select new Item
                         {
                             id = i.id,
                             name = i.name,
                             description = i.description,
                             category_id = c.name,
                             price = i.price,
                             quantity = i.quantity,
                             barcode = i.barcode
                         });
            return items;
        }
        public List<Category> GetCategories()
        {
            var categories = (from c in _context.table_category
                              select new { c.id, c.name }).ToList();
            List<Category> categoriesList = new List<Category>();
            foreach (var category in categories)
            {
                categoriesList.Add(new Category
                {
                    id = category.id,
                    name = category.name
                });
            }
            return categoriesList;
        }
    }
}
