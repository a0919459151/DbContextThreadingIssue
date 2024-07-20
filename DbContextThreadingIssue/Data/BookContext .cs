using DbContextThreadingIssue.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace DbContextThreadingIssue.Data;

public class BookContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    public BookContext(DbContextOptions<BookContext> options) 
        : base(options)
    {
    }

}
