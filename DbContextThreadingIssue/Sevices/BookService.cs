﻿using DbContextThreadingIssue.Data;
using DbContextThreadingIssue.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace DbContextThreadingIssue.Services;

public class BookService
{
    private readonly BookContext _context;

    public BookService(BookContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetBooks()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<Book> GetBookById(int id)
    {
        return await _context.Books.FirstAsync(b => b.Id == id);
    }
}
