using Microsoft.EntityFrameworkCore;

namespace ActielijstApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Memo> Memos { get; set; }
        public DbSet<Werknemer> Werknemers { get; set; }
        public DbSet<ActieSoort> ActieSoorten { get; set; }
        public DbSet<StblPriority> StblPriorities { get; set; }
        public DbSet<Inspectie> Inspecties { get; set; }
        public DbSet<AankomendeInspectie> AankomendeInspecties { get; set; }
        public DbSet<Correspondentie> Correspondentie { get; set; }
        public DbSet<StblCorrespondentieField> StblCorrespondentieFields { get; set; }
        public DbSet<ProjectOnderdeel> ProjectOnderdelen { get; set; }
        public DbSet<Adres> Adresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Memo>().ToTable("tblMemo", "dbo").HasKey(m => m.FldMid);
            modelBuilder.Entity<Werknemer>().ToTable("Werknemers", "dbo").HasKey(w => w.WerknId);
            modelBuilder.Entity<ActieSoort>().ToTable("stblActieSoort", "dbo").HasKey(a => a.Id);
            modelBuilder.Entity<StblPriority>().ToTable("stblPriority", "dbo").HasKey(p => p.Id);
            modelBuilder.Entity<Inspectie>().ToView("vw_KIWAInspecties").HasKey(i => i.OpdrachtId);
            modelBuilder.Entity<AankomendeInspectie>().ToView("vw_AankomendeInspecties").HasNoKey();
            modelBuilder.Entity<Correspondentie>().ToTable("correspondentie")
                .Property(e => e.SSMA_TimeStamp).IsRowVersion();
            modelBuilder.Entity<StblCorrespondentieField>().ToTable("stblCorrespondentieFields").HasKey(f => f.Id);
            modelBuilder.Entity<ProjectOnderdeel>().ToTable("tblProjectOnderdelen").HasKey(p => p.fldProjectId);
            modelBuilder.Entity<Adres>().ToTable("adres")
                .Property(a => a.SsmaTimeStamp).IsRowVersion();
            modelBuilder.Entity<Adres>()
                .Property(a => a.Id).HasColumnName("ID");
            modelBuilder.Entity<Adres>()
                .Property(a => a.Bedrijf).HasColumnName("BEDRIJF");
            modelBuilder.Entity<Adres>()
                .Property(a => a.EmailAdr).HasColumnName("E-MAIL_ADR");
        }
    }
}