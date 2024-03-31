using Microsoft.EntityFrameworkCore;

namespace PSV.Models;

public class Repository : DbContext
{
    public Repository()
    {
        
    }

    public Repository(DbContextOptions<Repository> options) : base(options)
    {
        
    }
    
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Cut> Cuts { get; set; }
    public virtual DbSet<Milling> Millings { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Wrapping> Wrappings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Client_pk");
            entity.ToTable("Client");
            entity.Property(e => e.Name);
            entity.Property(e => e.Address);
            entity.Property(e => e.PhoneNumber);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Order_pk");
            entity.ToTable("Order");
            entity.Property(e => e.OrderNumber);
            entity.Property(e => e.CreatedAt);
            entity.Property(e => e.QrCode);
            entity.Property(e => e.BarCode);
            entity.Property(e => e.Format);
            entity.Property(e => e.Comments);
            entity.Property(e => e.Photos);

            entity
                .HasOne(e => e.Client)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.IdClient)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Client_Orders");
        });

        modelBuilder.Entity<Cut>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Cut_pk");
            entity.ToTable("Cut");
            entity.Property(e => e.From);
            entity.Property(e => e.To);
            entity.Property(e => e.IsPresent);

            entity
                .HasOne(e => e.Order)
                .WithOne(e => e.Cut)
                .HasForeignKey<Order>(e => e.IdCut)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Order_Cut");
        });

        modelBuilder.Entity<Milling>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Milling_pk");
            entity.ToTable("Milling");
            entity.Property(e => e.From);
            entity.Property(e => e.To);
            entity.Property(e => e.IsPresent);

            entity
                .HasOne(e => e.Order)
                .WithOne(e => e.Milling)
                .HasForeignKey<Order>(e => e.IdMilling)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Order_Milling");
        });

        modelBuilder.Entity<Wrapping>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Wrapping_pk");
            entity.ToTable("Wrapping");
            entity.Property(e => e.From);
            entity.Property(e => e.To);
            entity.Property(e => e.IsPresent);

            entity
                .HasOne(e => e.Order)
                .WithOne(e => e.Wrapping)
                .HasForeignKey<Order>(e => e.IdWrapping)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Order_Wrapping");
        });
    }
}