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
    public virtual DbSet<Operator> Operators { get; set; }
    public virtual DbSet<Location> Locations { get; set; }
    public virtual DbSet<Comment> Comments {get; set;}
    public virtual DbSet<Releaser> Releasers {get; set;}
    public virtual DbSet<OrderStatus> OrderStatuses {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Client_pk");
            entity.ToTable("Client");
            entity.Property(e => e.Name);
            entity.Property(e => e.Address);
            entity.Property(e => e.PhoneNumber);
            entity.Property(e => e.NIP);
            entity.Property(e => e.Email);
        });

        modelBuilder.Entity<Operator>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Operator_pk");
            entity.ToTable("Operator");
            entity.Property(e => e.FirstName);
            entity.Property(e => e.LastName);
            entity.Property(e => e.PhoneNumber);

            entity
                .HasOne(e => e.Location)
                .WithMany(e => e.Operators)
                .HasForeignKey(e => e.IdLocation)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("Operator_Locations");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Location_pk");
            entity.ToTable("Location");
            entity.Property(e => e.Name);

            entity.HasData(
                new Location {Id = 1, Name = "Przasnysz"},
                new Location {Id = 2, Name = "Jednorożec"}
            );
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Order_pk");
            entity.ToTable("Order");
            entity.Property(e => e.OrderNumber);
            entity.Property(e => e.OrderName);
            entity.Property(e => e.CreatedAt);
            entity.Property(e => e.QrCode);
            entity.Property(e => e.BarCode);
            entity.Property(e => e.OrderFile);
            entity.Property(e => e.Photos);
            entity.Property(e => e.EdgeCodeProvided);
            entity.Property(e => e.EdgeCodeUsed);
            entity.Property(e => e.StagesCompleted);
            entity.Property(e => e.StagesTotal);
            entity.Property(e => e.ReleaseDate);

            entity
                .HasOne(e => e.Client)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.IdClient)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Client_Orders");

            entity
                .HasOne(e => e.Location)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.IdLocation)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("Order_Location");

            entity
                .HasOne(e => e.Releaser)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.IdReleaser)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("Order_Releaser");
            
            entity
                .HasOne(e => e.OrderStatus)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.IdOrderStatus)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("Order_OrderStatus");
        });

        modelBuilder.Entity<Releaser>(entity => {
            entity.HasKey(e => new { e.Id }).HasName("Releaser_pk");
            entity.ToTable("Releaser");
            entity.Property(e => e.FirstName);
            entity.Property(e => e.LastName);

            entity
                .HasOne(e => e.Location)
                .WithMany(e => e.Releasers)
                .HasForeignKey(e => e.IdLocation)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("Releaser_Location");
        });

        modelBuilder.Entity<OrderStatus>(entity => {
            entity.HasKey(e => new { e.Id }).HasName("OrderStatus_pk");
            entity.Property(e => e.Name);

            entity.HasData(
                new OrderStatus {Id = 1, Name = "W produkcji"},
                new OrderStatus {Id = 2, Name = "Gotowe do odbioru"},
                new OrderStatus {Id = 3, Name = "Odebrane"}
            );
        });

        modelBuilder.Entity<Cut>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Cut_pk");
            entity.ToTable("Cut");
            entity.Property(e => e.From);
            entity.Property(e => e.To);
            entity.Property(e => e.IsPresent);
            entity.Property(e => e.ClientNotified);

            entity
                .HasOne(e => e.Order)
                .WithOne(e => e.Cut)
                .HasForeignKey<Order>(e => e.IdCut)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Order_Cut");

            entity
                .HasOne(e => e.Operator)
                .WithMany(e => e.Cuts)
                .HasForeignKey(e => e.IdOperator)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Operator_Cut");
        });

        modelBuilder.Entity<Milling>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Milling_pk");
            entity.ToTable("Milling");
            entity.Property(e => e.From);
            entity.Property(e => e.To);
            entity.Property(e => e.IsPresent);
            entity.Property(e => e.ClientNotified);

            entity
                .HasOne(e => e.Order)
                .WithOne(e => e.Milling)
                .HasForeignKey<Order>(e => e.IdMilling)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Order_Milling");

            entity
                .HasOne(e => e.Operator)
                .WithMany(e => e.Millings)
                .HasForeignKey(e => e.IdOperator)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Operator_Milling");
        });

        modelBuilder.Entity<Wrapping>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Wrapping_pk");
            entity.ToTable("Wrapping");
            entity.Property(e => e.From);
            entity.Property(e => e.To);
            entity.Property(e => e.IsPresent);
            entity.Property(e => e.ClientNotified);

            entity
                .HasOne(e => e.Order)
                .WithOne(e => e.Wrapping)
                .HasForeignKey<Order>(e => e.IdWrapping)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Order_Wrapping");

            entity
                .HasOne(e => e.Operator)
                .WithMany(e => e.Wrappings)
                .HasForeignKey(e => e.IdOperator)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Operator_Wrapping");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("Comment_pk");
            entity.ToTable("Comment");
            entity.Property(e => e.Content);
            entity.Property(e => e.Time);
            entity.Property(e => e.Source);

            entity
                .HasOne(e => e.Operator)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.IdOperator)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Operator_Comment");
            
            entity
                .HasOne(e => e.Order)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.IdOrder)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Order_Comment");
        });
    }
}