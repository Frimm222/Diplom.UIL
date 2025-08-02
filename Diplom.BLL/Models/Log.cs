using System;

namespace Diplom.BLL.Models
{
    public class Log
    {
        public Guid item_id { get; set; }
        public string text { get; set; }
        public Guid user_id { get; set; }
        public DateTime time { get; set; }
    }
}
