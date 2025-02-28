// ActielijstGS/Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;

namespace ActielijstApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Memo> Memos { get; set; }
        public DbSet<Werknemer> Werknemers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Memo>().ToTable("tblMemo", "dbo");
            modelBuilder.Entity<Memo>().HasKey(m => m.FldMid);

            modelBuilder.Entity<Werknemer>().ToTable("Werknemers", "dbo");
            modelBuilder.Entity<Werknemer>().HasKey(w => w.WerknId);

            // Default waarden
            modelBuilder.Entity<Memo>().Property(m => m.WerknId).HasDefaultValue(0);
            modelBuilder.Entity<Memo>().Property(m => m.FldMKlantId).HasDefaultValue(0);
            modelBuilder.Entity<Memo>().Property(m => m.FldMOfferteId).HasDefaultValue(0);
            modelBuilder.Entity<Memo>().Property(m => m.FldMProjectId).HasDefaultValue(0);
            modelBuilder.Entity<Memo>().Property(m => m.FldOpdrachtId).HasDefaultValue(0);
            modelBuilder.Entity<Memo>().Property(m => m.FldMActieVoor).HasDefaultValue(0);
            modelBuilder.Entity<Memo>().Property(m => m.FldMPrioriteit).HasDefaultValue(0);

            // Configureer SSMA_TimeStamp als timestamp
            modelBuilder.Entity<Memo>()
              .Property(m => m.SSMA_TimeStamp)
              .HasColumnType("timestamp")
              .IsRowVersion()
              .IsConcurrencyToken();
        }
    }
}