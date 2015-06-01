using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.IO;

namespace ProiectPS.Models
{
    public class ReadBookModel
    {

        public Dictionary<UserProfile, List<BookModel>> listOfReadBooks = new Dictionary<UserProfile,List<BookModel>>();
    
         public void addReadBook(UserProfile user, BookModel book, int deserializationFlag){
             if (deserializationFlag == 0)
             {
                 deserialize();
             }
             if(listOfReadBooks.ContainsKey(user)){
                 List<BookModel> readBooks = listOfReadBooks[user];
                 readBooks.Add(book);
                 listOfReadBooks[user] = readBooks;
             }
             else
             {
                 List<BookModel> readBooks = new List<BookModel>();
                 readBooks.Add(book);
                 listOfReadBooks.Add(user, readBooks);
             }
             serialize();
         }

         public List<BookModel> readBooksForUser(UserProfile user)
         {
             deserialize();
             List<BookModel> books = new List<BookModel>();
             foreach (var u in listOfReadBooks)
             {
                 if (u.Key.UserId == user.UserId)
                 {
                     books.Add(u.Value.ElementAt(0));
                 }
             }
             return books;
         }

         public List<String> getGenresForUser(UserProfile user)
         {
             deserialize();
             List<String> genres = new List<String>();

             List<BookModel> books = readBooksForUser(user);

             foreach(var book in books){
                 if(!genres.Contains(book.genre)){
                     genres.Add(book.genre);
                 }
             }
             return genres;
         }

         public void deleteBook(BookModel book)
         {
             deserialize();
             foreach (var user in listOfReadBooks.Keys)
             {
                 if (listOfReadBooks[user].Contains(book))
                 {
                     if (listOfReadBooks[user].Count == 1)
                     {
                         listOfReadBooks.Remove(user);
                     }
                     else
                     {
                         List<BookModel> books = listOfReadBooks[user];
                         books.Remove(book);
                         listOfReadBooks[user] = books;
                     }
                 }
             }
             serialize();
         }

         public void serialize()
         {
             List<KeyValuePair<UserProfile,List<BookModel>>> list =  listOfReadBooks.ToList();

             string json = JsonConvert.SerializeObject(list, Formatting.Indented);
             System.IO.StreamWriter file = new System.IO.StreamWriter("D:\\PS Final cu HT\\test.txt");
             file.WriteLine(json);
             file.Close();
         }

        public void deserialize(){

            System.IO.StringReader file = new System.IO.StringReader("D:\\PS Final cu HT\\test.txt");
            List<KeyValuePair<UserProfile,List< BookModel>>> list = JsonConvert.DeserializeObject < List<KeyValuePair<UserProfile,List<BookModel>>>>(File.ReadAllText(@"D:\\PS Final cu HT\\test.txt"));

            foreach (var pair in list)
            {
                addReadBook(pair.Key, pair.Value.ElementAt(0),1);
                
            }
            file.Close();
        }
    }


}