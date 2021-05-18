using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Models
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options)
        : base(options)
        {
            Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
        public DbSet<Book> BookStore { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Men In The Sun",
                    Author = "Ghassan Kanafani",
                    Description = "Novel by Palestinian writer"
                },
                new Book
                {
                    Id = 2,
                    Title = "The butterfly's burden",
                    Author = "Mahmoud Darwish",
                    Description = "Combines the complete text of Darwish's two most recent full-length volumes,"
                });


            // modelBuilder.Entity<Book>()
            //.Property(b => b.title)
            //.IsRequired();
        }
    }
}
