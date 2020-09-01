using BookStore.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private BookStoreDbContext _context;
        public BookRepository(BookStoreDbContext context)
        {
            _context = context;
            if (!_context.Books.Any())
            {
                _context.Books.Add(new Book()
                {
                    BookName = "Tom And Jerry",
                    Price = 123.5,
                    Author = "Unknown",
                    Publisher = "Kim dong"
                });
                _context.Books.Add(new Book()
                {
                    BookName = "Steve Job",
                    Price = 200,
                    Author = "Unknown",
                    Publisher = "BBC"
                });
                _context.Books.Add(new Book()
                {
                    BookName = "Golden Axe",
                    Price = 199,
                    Author = "David Copy and Pase",
                    Publisher = "Sega"
                });
                _context.SaveChanges();
            }
        }
        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books.ToList();
        }

        public Book GetBookDetail(int bookId)
        {
            return _context.Books.FirstOrDefault(b => b.Id == bookId);
        }
    }

    public interface IBookRepository
    {
        void AddBook(Book book);
        IEnumerable<Book> GetAllBooks();
        Book GetBookDetail(int bookId);
    }
}
