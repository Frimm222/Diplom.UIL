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
                _context.table_logging.Add(new Log
                {
                    item_id = item.id,
                    text = $"добавил {item.quantity}шт.",
                    user_id = CurrentId,
                    time = DateTime.UtcNow
                });
                _context.SaveChanges();
                _context.Entry(item).State = EntityState.Detached;
                return true;
            }
            else
            {
                _context.Entry(item).State = EntityState.Detached;
                return false;
            }
        }

        public bool Update(Item item)
        {
            var existingItem = _context.table_items.AsNoTracking().FirstOrDefault(p => p.id.Equals(item.id));
            _context.table_items.Update(item);
            if (_context.SaveChanges() > 0) {
                if (existingItem.quantity != item.quantity && existingItem.price != item.price)
                {
                    _context.table_logging.Add(new Log
                    {
                        item_id = item.id,
                        text = $"изменил количество с {existingItem.quantity} на {item.quantity} и изменил цену с {existingItem.price} на {item.price}",
                        user_id = CurrentId,
                        time = DateTime.UtcNow
                    });
                    _context.SaveChanges();
                }
                else
                if (existingItem.quantity != item.quantity)
                {
                    _context.table_logging.Add(new Log
                    {
                        item_id = item.id,
                        text = $"изменил количество с {existingItem.quantity} на {item.quantity}",
                        user_id = CurrentId,
                        time = DateTime.UtcNow
                    });
                    _context.SaveChanges();
                }
                else if (existingItem.price != item.price)
                {
                    _context.table_logging.Add(new Log
                    {
                        item_id = item.id,
                        text = $"изменил цену с {existingItem.price} на {item.price}",
                        user_id = CurrentId,
                        time = DateTime.UtcNow
                    });
                    _context.SaveChanges();
                }
                else
                {
                    _context.table_logging.Add(new Log
                    {
                        item_id = item.id,
                        text = "изменил",
                        user_id = CurrentId,
                        time = DateTime.UtcNow
                    });
                    _context.SaveChanges();
                }
                _context.Entry(item).State = EntityState.Detached; // Отсоединяем объект от контекста
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(Item item)
        {
            item.is_deleted = true;
            item.category_id = _context.table_category.FirstOrDefault(c => c.name == item.category_id)?.id.ToString() ?? string.Empty;
            _context.table_items.Update(item);
            int res = _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;
            if (res > 0)
            {
                _context.table_logging.Add(new Log
                {
                    item_id = item.id,
                    text = "удалил",
                    user_id = CurrentId,
                    time = DateTime.UtcNow
                });
                _context.SaveChanges();
            }
            return res > 0;
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
                                   barcode = i.barcode,
                                   is_deleted = i.is_deleted,
                                   image = i.image
                               }).ToListAsync();
            return items;
        }
        public async Task<List<Log>> GetLogsAsync()
        {
            var logs = await (from l in _context.table_logging
                              join u in _context.table_users on l.user_id equals u.id
                              join i in _context.table_items on l.item_id equals i.id
                              orderby l.time descending
                              select new Log
                              {
                                  item_id = l.item_id,
                                  text = $"{u.Name} {u.Patronymic} {l.text} {i.name}",
                                  user_id = l.user_id,
                                  time = l.time
                              }).Take(50).ToListAsync();
            return logs;
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
