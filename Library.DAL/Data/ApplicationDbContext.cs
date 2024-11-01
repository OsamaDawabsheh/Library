using Library.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
       .Property(b => b.CreatedAt).HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ApplicationUser>()
    .HasIndex(u => u.Email)
    .IsUnique();

            modelBuilder.Entity<Book>()
        .HasMany(b => b.categories)
        .WithMany(c => c.books)
        .UsingEntity<BookCategory>(
                j => j.HasOne(bc => bc.category).WithMany(c => c.BookCategories).HasForeignKey(bc => bc.CategoryId),
               j => j.HasOne(bc => bc.book).WithMany(b => b.BookCategories).HasForeignKey(bc => bc.BookId),
               j => j.HasKey(bc => new {bc.BookId,bc.CategoryId})
               );

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<BookCategory> BookCategories { get; set; }
    }
}
