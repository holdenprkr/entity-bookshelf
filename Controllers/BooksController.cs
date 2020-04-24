using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShelf.Data;
using BookShelf.Models;
using BookShelf.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookShelf.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Books
        public async Task<ActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var books = await _context.Book
                .Where(b => b.ApplicationUserId == user.Id)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .ToListAsync();

            return View(books);
        }

        // GET: Books/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Books/Create
        public async Task<ActionResult> Create()
        {
            var allGenres = await _context.Genre
                .Select(g => new SelectListItem() { Text = g.Name, Value = g.Id.ToString() })
                .ToListAsync();

            var viewModel = new BookViewModel();

            viewModel.Genres = allGenres;

            return View(viewModel);
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BookViewModel bookViewModel)
        {
            try
            {
                var book = new Book
                {
                    Title = bookViewModel.Title,
                    Author = bookViewModel.Author
                };

                book.BookGenres = bookViewModel.GenreIds.Select(gi =>
                    new BookGenre
                    {
                        BookId = book.Id,
                        GenreId = gi
                    }
                ).ToList();

                var user = await GetCurrentUserAsync();
                book.ApplicationUserId = user.Id;

                _context.Book.Add(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Books/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var allGenres = await _context.Genre
                .Select(g => new SelectListItem() { Text = g.Name, Value = g.Id.ToString() })
                .ToListAsync();

            var book = await _context.Book
                .Include(b => b.BookGenres)
                .FirstOrDefaultAsync(b => b.Id == id);

            var viewModel = new BookViewModel()
            {
                Id = id,
                Title = book.Title,
                Author = book.Author,
                Genres = allGenres,
                GenreIds = book.BookGenres.Select(bg => bg.GenreId).ToList()
            };

            return View(viewModel);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, BookViewModel bookViewModel)
        {
            try
            {
                var user = await GetCurrentUserAsync();

                var book = await _context.Book
                    .Include(b => b.BookGenres)
                    .FirstOrDefaultAsync(b => b.Id == id);

                book.BookGenres.Clear();

                book.Title = bookViewModel.Title;
                book.Author = bookViewModel.Author;
                book.ApplicationUserId = user.Id;
                book.BookGenres = bookViewModel.GenreIds.Select(gi =>
                        new BookGenre
                        {
                            BookId = id,
                            GenreId = gi
                        }).ToList();

                _context.Book.Update(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Books/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var book = await _context.Book
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .FirstOrDefaultAsync(b => b.Id == id);

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Book book)
        {
            try
            {
                _context.Book.Remove(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}