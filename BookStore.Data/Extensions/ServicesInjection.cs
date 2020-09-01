using BookStore.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Data.Extensions
{
    public static class ServicesInjection
    {
        public static void AddProjectModules(this IServiceCollection services)
        {
            services.AddDbContext<BookStoreDbContext>(context => { context.UseInMemoryDatabase("BookStoreDb"); });
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
