using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProiectPS.Models;

namespace ProiectPS.Controllers
{
    [Authorize]
    public class ReadBookController : Controller
    {
        private ReadBookModel readBookModel = new ReadBookModel();
        private BookDBContext booksDB = new BookDBContext();
        private UsersContext usersDB = new UsersContext();
        //
        // GET: /ReadBook/

        public ActionResult Index()
        {
            UserProfile currentUser = usersDB.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name); 

            List<BookModel> readBooks = readBookModel.readBooksForUser(currentUser);
            return View(readBooks);
        }

        public List<BookModel> getSuggestions(UserProfile currentUser)
        {
            

            List<BookModel> suggestedBooksForUser = new List<BookModel>();

            List<BookModel> readBooksByUser = readBookModel.readBooksForUser(currentUser);

            List<String> genres = readBookModel.getGenresForUser(currentUser);

            foreach (var book in booksDB.Books)
            {
                if (readBooksByUser.Count == 0 || (!readBooksByUser.Contains(book) && genres.Contains(book.genre )) )
                {
                    suggestedBooksForUser.Add(book);
                }
            }
            return suggestedBooksForUser;
        }

        public ActionResult AddReadBook(int bookId)
        {
            UserProfile currentUser = usersDB.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name); 
            BookModel book = booksDB.Books.Find(bookId);
            readBookModel.addReadBook(currentUser, book,0);
            return RedirectToAction("Index");
        }
        public void deleteBook(int bookId)
        {
            BookModel book = booksDB.Books.Find(bookId);
            readBookModel.deleteBook(book);
        }

    }
}
