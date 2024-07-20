using DbContextThreadingIssue.Data;
using DbContextThreadingIssue.DbEntities;
using DbContextThreadingIssue.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

/* InMemory db 無法重現 exception 
builder.Services.AddDbContext<BookContext>(options =>
    options.UseInMemoryDatabase("BookList"));
*/

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BookContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<BookSerivce>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

// Init DB
InitDB(app);

app.Run();

void InitDB(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BookContext>();

    // if book no data
    var isBookNoData = !context.Books.Any();

    if (isBookNoData)
    {
        // insert 100 books
        Insert100Books(context);
    }

    context.SaveChanges();

    void Insert100Books(BookContext context)
    {
        for (int i = 0; i < 100; i++)
        {
            context.Books.Add(new Book
            {
                Title = $"Book {i}",
                Content = $"Content {i}"
            });
        }
    }
}
