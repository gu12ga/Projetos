namespace StarWars;

using Microsoft.EntityFrameworkCore;
//using MySQL.EntityFrameworkCore.Extensions;
public class Context : DbContext
{
    public DbSet<Cliente> Cliente { get; set; }
    public DbSet<CreditCard> CreditCart { get; set; }
    public DbSet<Produto> Produto { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseMySQL("server=localhost;database=loja;user=gustavo;password=12345");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Cliente>(entity =>
      {
        entity.HasKey(e => e.client_id);
        entity.Property(e => e.client_name).IsRequired();
      });
    
      modelBuilder.Entity<Produto>(entity =>
      {
        entity.HasKey(e => e.produto_id);
        entity.Property(e => e.Title).IsRequired();
        entity.Property(e => e.Price).IsRequired();
        entity.Property(e => e.ZipCode).IsRequired();
        entity.Property(e => e.ThumbnailHd).IsRequired();
        entity.Property(e => e.Date).IsRequired();
      });

      modelBuilder.Entity<CreditCard>(entity =>
      {
        entity.HasKey(e => new {e.client_id, e.card_number});
        entity.Property(e => e.card_holder_name).IsRequired();
        entity.Property(e => e.value).IsRequired();
        entity.Property(e => e.cvv).IsRequired();
        entity.Property(e => e.exp_date).IsRequired();
        entity.HasOne(d => d.cliente)
          .WithMany(p => p.creditcards);
      });

     modelBuilder.Entity<Transaction>(entity =>
      {
        entity.HasKey(e => e.purchase_id);
        entity.Property(e => e.total_to_pay).IsRequired();
        entity.Property(e => e.client_id).IsRequired();
        entity.Property(e => e.card_number).IsRequired();
        entity.Property(e => e.total_to_pay).IsRequired();
        entity.Property(e => e.date).IsRequired();
        entity.HasOne(d => d.cliente)
          .WithMany(p => p.transactions);
        entity.HasOne(d => d.creditcard)
          .WithMany(p => p.transactions);
      });

    }

}
