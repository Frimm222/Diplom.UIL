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
        public bool Delete(Guid id);

        public Task<List<Item>> GetAll();
    }

}
