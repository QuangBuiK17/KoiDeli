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

        }


        }
}
