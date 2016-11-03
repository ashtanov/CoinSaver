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
    public class CoinSaverContext : IdentityDbContext<CSUser>
    {
        public DbSet<CSPurchase> Purchases { get; set; }

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

        }

        public IEnumerable<Purchase> GetUserSpendings(CSUser user)
        {
            return Purchases.Where(x => x.UserID == user.Id).Select(x => x.ToPurchase());
        }
    }

    public class CSUser : IdentityUser
    {
        public string RealName { get; set; }
        public virtual ICollection<CSPurchase> Purchaces { get; set; }
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
                Price = Price,
                Date = Date,
                Category = Category,
                Reason = Reason
            };
        }
    }
}
