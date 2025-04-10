using ActielijstApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ActielijstApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<AssignmentListDefinition> AssignmentListDefinitions { get; set; }
        public DbSet<AssignmentFieldConfig> AssignmentFieldConfigs { get; set; }
        public DbSet<Actie> Acties { get; set; }
        public DbSet<Werknemer> Werknemers { get; set; }
        public DbSet<ActieSoort> ActieSoorten { get; set; }
        public DbSet<StblPriority> Priorities { get; set; }
        public DbSet<Inspectie> Inspecties { get; set; }
        public DbSet<AankomendeInspectie> AankomendeInspecties { get; set; }
        public DbSet<Correspondentie> Correspondentie { get; set; }
        public DbSet<Adres> Adressen { get; set; }
        public DbSet<CorrespondentieFields> CorrespondentieVelden { get; set; }
        public DbSet<StandaardDoc> StandaardDocs { get; set; }
        public DbSet<Globals> Globals { get; set; }
        public DbSet<Project> Projecten { get; set; }
        public DbSet<ProjectAssignment> ProjectAssignments { get; set; }
        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<AssignmentCategory> AssignmentCategories { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Werknemer
            modelBuilder.Entity<Werknemer>()
                .ToTable("Werknemers", "dbo")
                .HasKey(w => w.WerknId);

            // ActieSoort
            modelBuilder.Entity<ActieSoort>()
                .ToTable("stblActieSoort", "dbo")
                .HasKey(a => a.Id);

            // StblPriority
            modelBuilder.Entity<StblPriority>()
                .ToTable("stblPriority", "dbo")
                .HasKey(p => p.Id);

            // Inspectie
            modelBuilder.Entity<Inspectie>()
                .ToView("vw_KIWAInspecties")
                .HasKey(i => i.OpdrachtId);

            // AankomendeInspectie
            modelBuilder.Entity<AankomendeInspectie>()
                .ToView("vw_AankomendeInspecties")
                .HasNoKey();

            // CorrespondentieFields
            modelBuilder.Entity<CorrespondentieFields>()
                .ToView("vw_CorrespondentieFields")
                .HasNoKey();

            // StandaardDoc - ValueConverter voor ProjectMap
            var projectMapConverter = new ValueConverter<bool, string>(
                v => v ? "1" : "0",
                v => string.IsNullOrWhiteSpace(v) ? false : (v == "1" || v == "-1" || v.ToLower() == "true")
            );
            modelBuilder.Entity<StandaardDoc>()
                .Property(sd => sd.ProjectMap)
                .HasConversion(projectMapConverter);

            // Status
            modelBuilder.Entity<Status>()
                .ToTable("stblStatus")
                .HasKey(s => s.Id);

            modelBuilder.Entity<Status>()
                .Property(s => s.Id).HasColumnName("id");
            modelBuilder.Entity<Status>()
                .Property(s => s.StatusName).HasColumnName("status");
        }
    }
}