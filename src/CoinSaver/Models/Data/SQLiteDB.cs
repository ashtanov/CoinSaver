using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models
{
    public class CoinSaverContext : IdentityDbContext<CSUser, CSRole, string>
    {
        public DbSet<CSPurchase> Purchases { get; set; }
        public DbSet<CSSupply> Supplies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./coinsaver.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CSPurchase>()
                    .HasOne(x => x.User)
                    .WithMany(x => x.Purchaces)
                    .HasForeignKey(x => x.UserID);
            builder.Entity<CSSupply>()
                    .HasOne(x => x.User)
                    .WithMany(x => x.Supplies)
                    .HasForeignKey(x => x.UserID);

        }

        public IQueryable<Purchase> GetUserPurchases(CSUser user)
        {
            return Purchases.Where(x => x.UserID == user.Id).Select(x => x.ToPurchase());
        }

        public IQueryable<Supply> GetUserSupplies(CSUser user)
        {
            return Supplies.Where(x => x.UserID == user.Id).Select(x => x.ToSupply());
        }
    }

    public class CSUser : IdentityUser
    {
        public string RealName { get; set; }
        public virtual ICollection<CSPurchase> Purchaces { get; set; }
        public virtual ICollection<CSSupply> Supplies { get; set; }
    }

    [Table("Purchases")]
    public class CSPurchase
    {
        [Key]
        public int PurchaseId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public PurchaseCategory Category { get; set; }
        public PurchaseReason Reason { get; set; }
        public DateTime Date { get; set; }
        public virtual string UserID { get; set; }
        public virtual CSUser User { get; set; }

        public Purchase ToPurchase()
        {
            return new Purchase
            {
                PurchaseName = Name,
                Value = Price,
                Date = Date,
                Category = Category,
                Reason = Reason
            };
        }
    }

    [Table("Supplies")]
    public class CSSupply
    {
        [Key]
        public int SupplyId { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public DateTime Date { get; set; }
        public virtual string UserID { get; set; }
        public virtual CSUser User { get; set; }

        public Supply ToSupply()
        {
            return new Supply
            {
                SupplyName = Name,
                Value = Value,
                Date = Date
            };
        }
    }
}
