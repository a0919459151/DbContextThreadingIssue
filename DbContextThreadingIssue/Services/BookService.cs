using DbContextThreadingIssue.Data;
using DbContextThreadingIssue.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace DbContextThreadingIssue.Services;

public class BookService
{
    private readonly IDbContextFactory<BookContext> _contextFactory;

    public BookService(IDbContextFactory<BookContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<Book>> GetBooks()
    {
        // 使用 using 語句確保 DbContext 正確釋放
        using var context = _contextFactory.CreateDbContext();
        return await context.Books.ToListAsync();
    }

    public async Task<Book> GetBookById(int id)
    {
        // 使用 using 語句確保 DbContext 正確釋放
        using var context = _contextFactory.CreateDbContext();
        return await context.Books.FirstAsync(b => b.Id == id);
    }
}
