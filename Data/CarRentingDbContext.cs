namespace CarRentingSystem.Data;

using CarRentingSystem.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class CarRentingDbContext : IdentityDbContext<IdentityUser>
{
    public CarRentingDbContext(DbContextOptions<CarRentingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Car> Cars { get; init; }

    public DbSet<Category> Category { get; init; }

    public DbSet<Dealer> Dealers { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Car>(x =>
        {
            x.HasOne(x => x.Category)
            .WithMany(x => x.Cars)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Car>(x =>
        {
            x.HasOne(x => x.Dealer)
            .WithMany(x => x.Cars)
            .HasForeignKey(x => x.DealerId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Dealer>(x =>
        {
            x.HasOne<IdentityUser>()
            .WithOne()
            .HasForeignKey<Dealer>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        base.OnModelCreating(builder);
    }
}