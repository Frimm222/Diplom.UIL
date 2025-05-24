using Diplom.BLL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diplom.DAL
{
    public class DataBase : ICrud
    {
        private readonly DataBaseContext _context;
        private Guid CurrentId { get; set; }
        public DataBase()
        {
            _context = new DataBaseContext();
        }

        public bool Create(Item item)
        {
            _context.table_items.Add(item);
            if (_context.SaveChanges() > 0)
            {
                _context.Database.ExecuteSqlRaw($"INSERT INTO table_logging (item_id, text, user_id) VALUES ('{item.id}', 'добавил', '{CurrentId}');");
                return true;
            }
            else
            {
                return false;
            }
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

        public async Task<List<Item>> GetAll()
        {
            var items = await (from i in _context.table_items
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
                               }).ToListAsync();
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
        public User? GetUser(string login, string password)
        {
            var user = (from u in _context.table_users
                        where u.Login == login && u.Password == password
                        select new User
                        {
                            id = u.id,
                            Name = u.Name,
                            Surname = u.Surname,
                            Patronymic = u.Patronymic,
                            Login = u.Login,
                            Password = u.Password,
                            Role = u.Role
                        }).FirstOrDefault();
            CurrentId = user?.id ?? Guid.Empty;
            return user;
        }
    }
}
