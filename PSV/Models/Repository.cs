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
            entity.Property(e => e.QrCode);
            entity.Property(e => e.Format);
            entity.Property(e => e.Comments);
            entity.Property(e => e.Photos);

            entity
                .HasOne(e => e.Client)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.IdClient)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Client_Orders");

            entity
                .HasOne(e => e.Cut)
                .WithOne(e => e.Order)
                .HasForeignKey<Cut>(e => e.IdOrder)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Order_Cut");

            entity
                .HasOne(e => e.Milling)
                .WithOne(e => e.Order)
                .HasForeignKey<Milling>(e => e.IdOrder)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Order_Milling");

            entity
                .HasOne(e => e.Wrapping)
                .WithOne(e => e.Order)
                .HasForeignKey<Wrapping>(e => e.IdOrder)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Order_Wrapping");
        });

        modelBuilder.Entity<Cut>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Cut_pk");
            entity.ToTable("Cut");
            entity.Property(e => e.From);
            entity.Property(e => e.To);
            entity.Property(e => e.IsPresent);
        });

        modelBuilder.Entity<Milling>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Milling_pk");
            entity.ToTable("Milling");
            entity.Property(e => e.From);
            entity.Property(e => e.To);
            entity.Property(e => e.IsPresent);
        });

        modelBuilder.Entity<Wrapping>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Wrapping_pk");
            entity.ToTable("Wrapping");
            entity.Property(e => e.From);
            entity.Property(e => e.To);
            entity.Property(e => e.IsPresent);
        });
    }
}