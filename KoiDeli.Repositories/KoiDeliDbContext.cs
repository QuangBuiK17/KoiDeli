using KoiDeli.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
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
        public DbSet<TimelineDelivery> TimelineDelivery { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<FishInBox> FishInBoxes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Loại bỏ vòng lặp này vì nó ảnh hưởng đến tất cả các quan hệ trước khi ta có thể cấu hình chi tiết
            // foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            // {
            //     relationship.DeleteBehavior = DeleteBehavior.Restrict;
            // }

            // Config N-N relationship

            // Config BoxOption
            modelBuilder.Entity<BoxOption>()
               .HasKey(ps => ps.Id);

            modelBuilder.Entity<BoxOption>()
               .HasOne(s => s.Box)
               .WithMany(ps => ps.BoxOptions)
               .HasForeignKey(s => s.BoxId)
               .OnDelete(DeleteBehavior.Restrict); // Không xóa Box khi xóa BoxOption

            // Config FishInBox
            modelBuilder.Entity<FishInBox>()
               .HasOne(fib => fib.BoxOption)        // Mỗi FishInBox thuộc về một BoxOption
               .WithMany(bo => bo.FishInBoxes)      // Một BoxOption chứa nhiều FishInBox
               .HasForeignKey(fib => fib.BoxOptionId) // Khóa ngoại là BoxOptionId
               .OnDelete(DeleteBehavior.Cascade);   // Khi xóa BoxOption, xóa luôn FishInBox

            modelBuilder.Entity<FishInBox>()
               .HasOne(fib => fib.KoiFish)          // Mỗi FishInBox chứa một loại KoiFish
               .WithMany(kf => kf.FishInBoxes)      // Một KoiFish có thể có nhiều FishInBox
               .HasForeignKey(fib => fib.FishId)    // Khóa ngoại là FishId
               .OnDelete(DeleteBehavior.Restrict);  // Không xóa KoiFish khi xóa FishInBox

            // Config OrderDetail
            modelBuilder.Entity<OrderDetail>()
               .HasKey(ps => ps.Id);

            modelBuilder.Entity<OrderDetail>()
               .HasOne(s => s.BoxOption)            // Mỗi OrderDetail chứa một BoxOption
               .WithOne(ps => ps.OrderDetail)       // Một BoxOption thuộc về một OrderDetail
               .HasForeignKey<OrderDetail>(s => s.BoxOptionId)
               .OnDelete(DeleteBehavior.Cascade);   // Khi xóa OrderDetail, xóa luôn BoxOption

            modelBuilder.Entity<OrderDetail>()
               .HasOne(s => s.Distance)
               .WithMany(ps => ps.OrderDetails)
               .HasForeignKey(s => s.DistanceId)
               .OnDelete(DeleteBehavior.Restrict);  // Không xóa Distance khi xóa OrderDetail

            // Config TimelineDelivery
            modelBuilder.Entity<TimelineDelivery>()
               .HasKey(ps => ps.Id);

            modelBuilder.Entity<TimelineDelivery>()
               .HasOne(s => s.Branch)
               .WithMany(ps => ps.TimelineDeliveries)
               .HasForeignKey(s => s.BranchId)
               .OnDelete(DeleteBehavior.Restrict);  // Không xóa Branch khi xóa TimelineDelivery

            modelBuilder.Entity<TimelineDelivery>()
               .HasOne(p => p.Vehicle)
               .WithMany(ps => ps.TimelineDeliveries)
               .HasForeignKey(s => s.VehicleId)
               .OnDelete(DeleteBehavior.Restrict);  // Không xóa Vehicle khi xóa TimelineDelivery

            // Config OrderTimeline
            modelBuilder.Entity<OrderTimeline>()
               .HasKey(ps => ps.Id);

            modelBuilder.Entity<OrderTimeline>()
               .HasOne(s => s.OrderDetail)
               .WithMany(ps => ps.OrderTimelines)
               .HasForeignKey(s => s.OrderDetailId)
               .OnDelete(DeleteBehavior.Restrict);  // Không xóa OrderDetail khi xóa OrderTimeline

            modelBuilder.Entity<OrderTimeline>()
               .HasOne(p => p.TimelineDelivery)
               .WithMany(ps => ps.OrderTimelines)
               .HasForeignKey(s => s.TimelineDeliveryId)
               .OnDelete(DeleteBehavior.Restrict);  // Không xóa TimelineDelivery khi xóa OrderTimeline

            modelBuilder.Entity<Order>()
                .HasOne(p => p.User)
                .WithMany(p =>p.Orders)
                .HasForeignKey( s=> s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }



    }
}
