using DbContextThreadingIssue.DbEntities;
using DbContextThreadingIssue.Services;
using Microsoft.AspNetCore.Mvc;

namespace DbContextThreadingIssue.Controllers;

[ApiController]
[Route("api/book")]
public class BookController : Controller
{
    private readonly BookSerivce _bookSerivce;

    public BookController(BookSerivce bookSerivce)
    {
        _bookSerivce = bookSerivce;
    }

    // Get books
    [HttpGet("getBooks")]
    public async Task<IActionResult> GetBooks()
    {
        var books = await _bookSerivce.GetBooks();
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
            var books = await Task.WhenAll(tasks.ToArray());

            return Ok(books);

        }
        catch (Exception)
        {
            // 這裡會發生
            // System.InvalidOperationException: A second operation was started on this context instance before a previous operation completed.
            // This is usually caused by different threads concurrently using the same instance of DbContext.
            // For more information on how to avoid threading issues with DbContext, see https://go.microsoft.com/fwlink/?linkid=2097913.
            throw;
        }

        List<Task<Book>> GetBookTasks()
        {
            var  tasks = new List<Task<Book>>();
            for (int i = 1; i <= 100; i++)
            {
                tasks.Add(_bookSerivce.GetBookById(i));
            }
            return tasks;
        }
    }
}
