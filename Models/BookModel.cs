using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProiectPS.Models
{
    [Table("BookModel")]
    public class BookModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int bookId { get; set; }
        public String title { get; set; }
        public String author { get; set; }
        public String genre { get; set; }
        public float rating { get; set; }
    }

    public class BookDBContext : DbContext
    {
        public DbSet<BookModel> Books { get; set; }
    }
}