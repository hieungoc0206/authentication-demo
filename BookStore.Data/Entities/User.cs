using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string FavoriteColor { get; set; }
        public string Role { get; set; }
        public string GoogleId { get; set; }
    }
}
