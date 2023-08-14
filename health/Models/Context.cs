namespace health;
using Microsoft.EntityFrameworkCore;

public class Context: DbContext
{
    public DbSet<Customer> Customer { get; set; }

    public DbSet<Loan> Loan { get; set; }

    public DbSet<LoanMatches> LoanMatches { get; set; }

     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseMySQL("server=localhost;database=library;user=gustavo;password=12345");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      
      modelBuilder.Entity<Customer>(entity =>
      {
        entity.HasKey(e => e.cpf);
        entity.Property(e => e.name).IsRequired();
        entity.Property(e => e.age).IsRequired();
        entity.Property(e => e.location).IsRequired();
        entity.Property(e => e.income).IsRequired();
      });
      
      modelBuilder.Entity<Loan>(entity =>
      {
        entity.HasKey(e => e.idLoan);
        entity.Property(e => e.type).IsRequired();
        entity.Property(e => e.rate).IsRequired();
      });

     modelBuilder.Entity<LoanMatches>(entity =>
      {
        entity.HasKey(e => new{e.cpf, e.idLoan});
        entity.HasOne(d => d.Customer)
          .WithMany(p => p.LoanMatches);
        entity.HasOne(d => d.Customer)
          .WithMany(p => p.LoanMatches);
      });
    }

}