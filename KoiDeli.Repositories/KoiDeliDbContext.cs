using KoiDeli.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace KoiDeli.Repositories
{
    public class KoiDeliDbContext : DbContext
    {
        public KoiDeliDbContext(DbContextOptions<KoiDeliDbContext> options) : base(options)
        {
        }

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Box> Boxes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<BoxOption> BoxOptions { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet <Distance> Distances { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<KoiFish> KoiFishs { get;set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderTimeline> OrderTimeline { get; set; }
        public DbSet<PartnerShipment> PartnerShipment { get; set; }
        public DbSet<TimelineDelivery> TimelineDelivery { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<TransactionDetail> TransactionDetail { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            //Config N-N realationship

            //config Boxoption
            modelBuilder.Entity<BoxOption>()
           .HasKey(ps => new { ps.BoxId, ps.FishId });

            modelBuilder.Entity<BoxOption>()
            .HasOne(s => s.Box)
            .WithMany(ps => ps.BoxOptions)
            .HasForeignKey(s => s.BoxId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BoxOption>()
            .HasOne(p => p.Fish)
            .WithMany(ps => ps.BoxOptions)
            .HasForeignKey(s => s.FishId)
            .OnDelete(DeleteBehavior.Restrict);

            //config TimelineDelivery
            modelBuilder.Entity<TimelineDelivery>()
           .HasKey(ps => new { ps.BranchId, ps.VehicleId });

            modelBuilder.Entity<TimelineDelivery>()
            .HasOne(s => s.Branch)
            .WithMany(ps => ps.TimelineDeliveries)
            .HasForeignKey(s => s.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TimelineDelivery>()
            .HasOne(p => p.Vehicle)
            .WithMany(ps => ps.TimelineDeliveries)
            .HasForeignKey(s => s.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

            //config OrderTimeline

            modelBuilder.Entity<OrderTimeline>()
            .HasOne(s => s.OrderDetail)
            .WithMany(ps => ps.OrderTimelines)
            .HasForeignKey(s => s.OrderDetailId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderTimeline>()
            .HasOne(p => p.TimelineDelivery)
            .WithMany(ps => ps.OrderTimelines)
            .HasForeignKey(s => s.TimelineDeliveryId)
            .OnDelete(DeleteBehavior.Restrict);

            //config OrderDetail
            modelBuilder.Entity<OrderDetail>() // BoxOption vs OrD
           .HasOne(s => s.BoxOption)
           .WithMany(ps => ps.OrderDetails)
           .HasForeignKey(s => s.BoxId)
           .OnDelete(DeleteBehavior.Restrict);


        }


        }
}
