using Fiestivo.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
namespace Fiestivo.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Event> Events { get; set; }
    public DbSet<_User> _Users { get; set; }
    public DbSet<Attend> Attends { get; set; }
    public DbSet<Post_On> Post_Ons { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // إعدادات Attend
        modelBuilder.Entity<Attend>()
    .HasKey(a => new { a._User_ID, a.Event_ID });

        modelBuilder.Entity<Attend>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a._User_ID);

        modelBuilder.Entity<Attend>()
            .HasOne(a => a.Event)
            .WithMany(e => e.Attends)
            .HasForeignKey(a => a.Event_ID);

        // إعدادات Post_On
        modelBuilder.Entity<Post_On>()
            .HasKey(a => new { a._User_ID, a.Event_ID });
        modelBuilder.Entity<Post_On>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a._User_ID);
        modelBuilder.Entity<Post_On>()
            .HasOne(a => a.Event)
            .WithMany()
            .HasForeignKey(a => a.Event_ID);

        // إعدادات Review
        modelBuilder.Entity<Review>()
            .HasKey(a => new { a._User_ID, a.Event_ID });
        modelBuilder.Entity<Review>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a._User_ID);
        modelBuilder.Entity<Review>()
            .HasOne(a => a.Event)
            .WithMany()
            .HasForeignKey(a => a.Event_ID);

        // إعدادات Category
        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.Event_ID)
                  .ValueGeneratedOnAdd(); // Tells EF Core this is an auto-incrementing column
        });
        modelBuilder.Entity<Event>()
        .HasOne(e => e.User)
        .WithMany()
        .HasForeignKey(e => e.UserID)
        .OnDelete(DeleteBehavior.Restrict);
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<_User>()
         .HasMany(u => u.Events)
         .WithOne(e => e.User)
         .HasForeignKey(e => e.UserID);
    }





}
