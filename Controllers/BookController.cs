using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProiectPS.Models;

namespace ProiectPS.Controllers
{
    public class BookController : Controller
    {
        private BookDBContext db = new BookDBContext();
        private ReadBookController readBookController = new ReadBookController();
        private UsersContext usersDB = new UsersContext();
        //
        // GET: /Book/

        public ActionResult Index()
        {
            return View(db.Books.ToList());
        }

        public ActionResult viewSuggestedBooks()
        {
            UserProfile currentUser = usersDB.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name); 
            List<BookModel> suggestionsForUser = readBookController.getSuggestions(currentUser);
            return View(suggestionsForUser);
        }

        public ActionResult rateAsGood(int bookId)
        {
            BookModel book = db.Books.Find(bookId);
            float newRate = book.rating + 1.0f;
            book.rating = newRate;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult rateAsBad(int bookId)
        {
            BookModel book = db.Books.Find(bookId);
            float newRate = book.rating - 1.0f;
            book.rating = newRate;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id = 0)
        {
            BookModel bookmodel = db.Books.Find(id);
            if (bookmodel == null)
            {
                return HttpNotFound();
            }
            return View(bookmodel);
        }

        //
        // GET: /Book/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Book/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookModel bookmodel)
        {
            if (ModelState.IsValid)
            {
                bookmodel.rating = 0.0f;
                db.Books.Add(bookmodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bookmodel);
        }

        //
        // GET: /Book/Edit/5
        [Authorize(Users = "Admin")]
        public ActionResult Edit(int id = 0)
        {
            BookModel bookmodel = db.Books.Find(id);
            if (bookmodel == null)
            {
                return HttpNotFound();
            }
            return View(bookmodel);
        }

        //
        // POST: /Book/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "Admin")]
        public ActionResult Edit(BookModel bookmodel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookmodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookmodel);
        }

        //
        // GET: /Book/Delete/5
        [Authorize(Users = "Admin")]
        public ActionResult Delete(int id = 0)
        {
            BookModel bookmodel = db.Books.Find(id);
            if (bookmodel == null)
            {
                return HttpNotFound();
            }
            return View(bookmodel);
        }

        //
        // POST: /Book/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            BookModel bookmodel = db.Books.Find(id);
            db.Books.Remove(bookmodel);
            db.SaveChanges();
            readBookController.deleteBook(id);
            return RedirectToAction("Index");
        }

        
    }
}