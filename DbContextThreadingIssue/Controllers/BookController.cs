using DbContextThreadingIssue.DbEntities;
using DbContextThreadingIssue.Services;
using Microsoft.AspNetCore.Mvc;

namespace DbContextThreadingIssue.Controllers;

[ApiController]
[Route("api/book")]
public class BookController : Controller
{
    private readonly BookService _bookService;
    private readonly IServiceProvider _provider;

    public BookController(
        BookService bookService,
        IServiceProvider provider)
    {
        _bookService = bookService;
        _provider = provider;
    }

    // Get books
    [HttpGet("getBooks")]
    public async Task<IActionResult> GetBooks()
    {
        var books = await _bookService.GetBooks();
        return Ok(books);
    }

    // Get books when all
    [HttpGet("getBooksWhenAll")]
    public async Task<IActionResult> GetBooksWhenAll()
    {
        try
        {
            // Get 100 books task
            var tasks = GetBookTasks();

            // when all
            var books = await Task.WhenAll(tasks);

            return Ok(books);
        }
        catch (Exception)
        {
            // 如果 _bookService 直接注入 dbcontext 這裡會發生異常
            // System.InvalidOperationException: A second operation was started on this context instance before a previous operation completed.
            // This is usually caused by different threads concurrently using the same instance of DbContext.
            // For more information on how to avoid threading issues with DbContext, see https://go.microsoft.com/fwlink/?linkid=2097913.
            // -------------------- or --------------------
            // System.InvalidOperationException: An attempt was made to use the context instance while it is being configured.
            // A DbContext instance cannot be used inside 'OnConfiguring' since it is still being configured at this point.
            // This can happen if a second operation is started on this context instance before a previous operation completed.
            // Any instance members are not guaranteed to be thread safe.
            throw;
        }

        List<Task<Book>> GetBookTasks()
        {
            var tasks = new List<Task<Book>>();
            for (int i = 1; i <= 100; i++)
            {
                tasks.Add(GetTask(i));
            }
            return tasks;
        }
        Task<Book> GetTask(int bookId)
        {
            return Task.Run(async () =>
            {
                using var scope = _provider.CreateScope();
                var bookService = scope.ServiceProvider.GetRequiredService<BookService>();

                Console.WriteLine($"Task {bookId} is running on thread {Thread.CurrentThread.ManagedThreadId}");
                return await bookService.GetBookById(bookId);
            });
        }
    }
}
