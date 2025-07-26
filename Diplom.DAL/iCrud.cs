using Diplom.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.DAL
{
    public interface ICrud
    {
        public bool Create(Item item);
        public bool Update(Item item);
        public bool Delete(Item item);

        public Task<List<Item>> GetAll();
    }

}
