namespace health;
using Microsoft.EntityFrameworkCore;

public class Context: DbContext
{
      public DbSet<Customer> Customers { get; set; }
      public DbSet<Loan> Loans { get; set; }
      public DbSet<LoanMatches> LoanMatches { get; set; }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
          optionsBuilder.UseMySQL("server=******;database=******;user=********;password=*****");
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
          base.OnModelCreating(modelBuilder);

          ConfigureCustomerEntity(modelBuilder);
          ConfigureLoanEntity(modelBuilder);
          ConfigureLoanMatchesEntity(modelBuilder);
      }

      private void ConfigureCustomerEntity(ModelBuilder modelBuilder)
      {
          modelBuilder.Entity<Customer>(entity =>
          {
              entity.HasKey(e => e.Cpf);
              entity.Property(e => e.Name).IsRequired();
              entity.Property(e => e.Age).IsRequired();
              entity.Property(e => e.Location).IsRequired();
              entity.Property(e => e.Income).IsRequired();
          });
      }

      private void ConfigureLoanEntity(ModelBuilder modelBuilder)
      {
          modelBuilder.Entity<Loan>(entity =>
          {
              entity.HasKey(e => e.IdLoan);
              entity.Property(e => e.Type).IsRequired();
              entity.Property(e => e.Rate).IsRequired();
          });
      }

      private void ConfigureLoanMatchesEntity(ModelBuilder modelBuilder)
      {
          modelBuilder.Entity<LoanMatches>(entity =>
          {
              entity.HasKey(e => new { e.Cpf, e.IdLoan });
              entity.HasOne(d => d.Customer)
                  .WithMany(p => p.LoanMatches);
              entity.HasOne(d => d.Loan)
                  .WithMany(p => p.LoanMatches);
          });
      }
}

