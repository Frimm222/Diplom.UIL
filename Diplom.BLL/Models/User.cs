
using System;

namespace Diplom.BLL.Models
{
    public class User
    {
        public Guid id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Patronymic { get; set; }
        public string? Role { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
}
