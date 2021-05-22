using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated(); // if it didn't added, it will return SqlException error ( The login failed. Login failed for user).
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Men In The Sun",
                    Description = "Novel by Palestinian writer"
                },
                new Book
                {
                    Id = 2,
                    Title = "The butterfly's burden",
                    Description = "Combines the complete text of Darwish's two most recent full-length volumes,"
                });

            modelBuilder.Entity<Book_Author>()
                .HasOne(b => b.Book)
                .WithMany(ba => ba.Book_Authors)
                .HasForeignKey(bi => bi.BookId);

            modelBuilder.Entity<Book_Author>()
                .HasOne(a => a.Author)
                .WithMany(ba => ba.Book_Authors)
                .HasForeignKey(ai => ai.AutorId);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Book> Book { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book_Author> Book_Authors { get; set; }
    }
}
