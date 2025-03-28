﻿using ActielijstApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ActielijstApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Actie> Acties { get; set; }
        public DbSet<Werknemer> Werknemers { get; set; }
        public DbSet<ActieSoort> ActieSoorten { get; set; }
        public DbSet<StblPriority> Priorities { get; set; }
        public DbSet<Inspectie> Inspecties { get; set; }
        public DbSet<AankomendeInspectie> AankomendeInspecties { get; set; }
        public DbSet<Correspondentie> Correspondentie { get; set; }
        //public DbSet<StblCorrespondentieField> CorrespondentieFields { get; set; }
        public DbSet<ProjectOnderdeel> ProjectOnderdelen { get; set; }
        public DbSet<Adres> Adressen { get; set; }
        public DbSet<CorrespondentieFields> CorrespondentieVelden { get; set; }
        public DbSet<StandaardDoc> StandaardDocs { get; set; }
        public DbSet<Globals> Globals { get; set; }
        public DbSet<Project> Projecten { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Actie
            modelBuilder.Entity<Actie>()
                .ToTable("tblMemo", "dbo")
                .HasKey(m => m.FldMid);

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

            // Correspondentie
            modelBuilder.Entity<Correspondentie>()
                .ToTable("correspondentie")
                .HasKey(c => c.Id); // Alleen de sleutel hier

            modelBuilder.Entity<Actie>()
                .Property(a => a.SSMA_TimeStamp)
                .IsRowVersion() // Markeer als rowversion
                .IsConcurrencyToken(); // Gebruik voor concurrency-controle

            // StblCorrespondentieField
            modelBuilder.Entity<StblCorrespondentieField>()
                .ToTable("stblCorrespondentieFields")
                .HasKey(f => f.Id);

            // ProjectOnderdeel
            modelBuilder.Entity<ProjectOnderdeel>()
                .ToTable("tblProjectOnderdelen")
                .HasKey(p => p.fldProjectId);

            // Adres
            modelBuilder.Entity<Adres>()
                .ToTable("adres")
                .HasKey(a => a.Id);

            modelBuilder.Entity<Adres>()
                .Property(a => a.Id).HasColumnName("ID");
            modelBuilder.Entity<Adres>()
                .Property(a => a.Bedrijf).HasColumnName("BEDRIJF");
            modelBuilder.Entity<Adres>()
                .Property(a => a.EmailAdr).HasColumnName("E-MAIL_ADR");
            modelBuilder.Entity<Adres>()
                .Property(a => a.SsmaTimeStamp).IsRowVersion();

            // CorrespondentieFields
            modelBuilder.Entity<CorrespondentieFields>()
                .ToView("vw_CorrespondentieFields")
                .HasNoKey();

            // StandaardDoc
            modelBuilder.Entity<StandaardDoc>()
                .ToTable("tblStandaardDoc")
                .HasKey(sd => sd.DocId);

            modelBuilder.Entity<StandaardDoc>()
                .Property(sd => sd.DocId).HasColumnName("doc_id");
            modelBuilder.Entity<StandaardDoc>()
                .Property(sd => sd.NaamDoc).HasColumnName("fldNaamDoc");
            modelBuilder.Entity<StandaardDoc>()
                .Property(sd => sd.PathDoc).HasColumnName("fldPathDoc");
            modelBuilder.Entity<StandaardDoc>()
                .Property(sd => sd.DocOmschrijving).HasColumnName("fldDocOmschrijving");
            modelBuilder.Entity<StandaardDoc>()
                .Property(sd => sd.DocNum).HasColumnName("fldDocNum");
            modelBuilder.Entity<StandaardDoc>()
                .Property(sd => sd.DocSavePath).HasColumnName("fldDocSavePath");

            var projectMapConverter = new ValueConverter<bool, string>(
                v => v ? "1" : "0",
                v => string.IsNullOrWhiteSpace(v) ? false : (v == "1" || v == "-1" || v.ToLower() == "true")
            );
            modelBuilder.Entity<StandaardDoc>()
                .Property(sd => sd.ProjectMap)
                .HasColumnName("fldProjectMap")
                .HasConversion(projectMapConverter);

            modelBuilder.Entity<StandaardDoc>()
                .Property(sd => sd.DocPrefix).HasColumnName("fldDocPrefix");
            modelBuilder.Entity<StandaardDoc>()
                .Property(sd => sd.Soort).HasColumnName("fldSoort");
            modelBuilder.Entity<StandaardDoc>()
                .Property(sd => sd.PrijsId).HasColumnName("fldPrijsId");
            modelBuilder.Entity<StandaardDoc>()
                .Property(sd => sd.EmailSjabloon).HasColumnName("fldEmailSjabloon");
            modelBuilder.Entity<StandaardDoc>()
                .Property(sd => sd.EmailAan).HasColumnName("fldEmailAan");
            modelBuilder.Entity<StandaardDoc>()
                .Property(sd => sd.EmailSubject).HasColumnName("fldEmailSubject");

            // Correspondentie velden
            modelBuilder.Entity<Correspondentie>()
                .Property(c => c.KlantID).HasColumnName("KlantID");
            modelBuilder.Entity<Correspondentie>()
                .Property(c => c.fldCorProjNum).HasColumnName("fldCorProjNum");
            modelBuilder.Entity<Correspondentie>()
                .Property(c => c.fldCorOpdrachtNum).HasColumnName("fldCorOpdrachtNum");
            modelBuilder.Entity<Correspondentie>()
                .Property(c => c.fldCorAuteur).HasColumnName("fldCorAuteur");
            modelBuilder.Entity<Correspondentie>()
                .Property(c => c.fldCorDatum).HasColumnName("fldCorDatum");
            modelBuilder.Entity<Correspondentie>()
                .Property(c => c.fldCorOmschrijving).HasColumnName("fldCorOmschrijving");
            modelBuilder.Entity<Correspondentie>()
                .Property(c => c.fldCorCPersId).HasColumnName("fldCorCPersId");
            modelBuilder.Entity<Correspondentie>()
                .Property(c => c.fldCorBestand).HasColumnName("fldCorBestand");

            // Globals
            modelBuilder.Entity<Globals>()
                .ToTable("tblGlobals")
                .HasKey(g => g.Id);

            modelBuilder.Entity<Globals>()
                .Property(g => g.Id).HasColumnName("Id");
            modelBuilder.Entity<Globals>()
                .Property(g => g.SavePath).HasColumnName("savepath");
            modelBuilder.Entity<Globals>()
                .Property(g => g.DesignPath).HasColumnName("designpath");
            modelBuilder.Entity<Globals>()
                .Property(g => g.SjabloonPath).HasColumnName("sjabloonpath");
            modelBuilder.Entity<Globals>()
                .Property(g => g.Version).HasColumnName("version");
            modelBuilder.Entity<Globals>()
                .Property(g => g.ArchiefPath).HasColumnName("archiefpath");
            modelBuilder.Entity<Globals>()
                .Property(g => g.PdfPath).HasColumnName("pdfPath");
            modelBuilder.Entity<Globals>()
                .Property(g => g.ScanPath).HasColumnName("ScanPath");
            modelBuilder.Entity<Globals>()
                .Property(g => g.KiwaPath).HasColumnName("KiwaPath");
            modelBuilder.Entity<Globals>()
                .Property(g => g.ProjectPath).HasColumnName("ProjectPath");
            modelBuilder.Entity<Globals>()
                .Property(g => g.FotoPath).HasColumnName("FotoPath");
            modelBuilder.Entity<Globals>()
                .Property(g => g.FactuurText).HasColumnName("FactuurText");
            modelBuilder.Entity<Globals>()
                .Property(g => g.FactuurAccount).HasColumnName("FactuurAccount");
            modelBuilder.Entity<Globals>()
                .Property(g => g.FactuurHandtekening).HasColumnName("FactuurHandtekening");
            modelBuilder.Entity<Globals>()
                .Property(g => g.DisplayMailVoorVerzenden).HasColumnName("DisplayMailVoorVerzenden");

            // Project
            modelBuilder.Entity<Project>()
                .ToTable("tblprojecten")
                .HasKey(p => p.Id);

            modelBuilder.Entity<Project>()
                .Property(p => p.Id).HasColumnName("id");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldProjectNummer).HasColumnName("fldProjectNummer");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldExternNummer).HasColumnName("fldExternNummer");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldExternNummer2).HasColumnName("fldExternNummer2");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldProjectNaam).HasColumnName("fldProjectNaam");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldAfdeling).HasColumnName("fldAfdeling");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldJaar).HasColumnName("fldjaar");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldDatum).HasColumnName("fldDatum");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldAdres).HasColumnName("fldAdres");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldPC).HasColumnName("fldPC");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldPlaats).HasColumnName("fldPlaats");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldSoort).HasColumnName("fldSoort");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldActie).HasColumnName("fldActie");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldIntracNr).HasColumnName("fldIntracNr");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldSGG).HasColumnName("fldSGG");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldEPA).HasColumnName("fldEPA");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldOpdrachtgeverId).HasColumnName("fldOpdrachtgeverId");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldOpdrachtgever).HasColumnName("fldOpdrachtgever");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldStatus).HasColumnName("fldStatus");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldFolder).HasColumnName("fldFolder");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldArchiefMap).HasColumnName("fldArchiefMap");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldVerwerkendBedrijf).HasColumnName("fldVerwerkendBedrijf");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldFabrikant).HasColumnName("fldFabrikant");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldSysteem).HasColumnName("fldSysteem");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldAantalM2).HasColumnName("fldAantalM2");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldKiWa).HasColumnName("fldKiWa");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldKiwaCert).HasColumnName("fldKiwaCert");
            modelBuilder.Entity<Project>()
                .Property(p => p.SSMA_TimeStamp).HasColumnName("SSMA_TimeStamp").IsRowVersion();
            modelBuilder.Entity<Project>()
                .Property(p => p.FldAfwerking).HasColumnName("fldAfwerking");
            modelBuilder.Entity<Project>()
                .Property(p => p.FldPrevProjectId).HasColumnName("fldPrevProjectId");
        }
    }
}