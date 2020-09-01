using BookStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Data
{
    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> context) : base(context)
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
