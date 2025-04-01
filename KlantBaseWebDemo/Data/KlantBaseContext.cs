using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using KlantBaseWebDemo.Models.KlantBase;

namespace KlantBaseWebDemo.Data
{
    public partial class KlantBaseContext : DbContext
    {
        public KlantBaseContext()
        {
        }

        public KlantBaseContext(DbContextOptions<KlantBaseContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.StblMaanden>().HasNoKey();

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.StblReplaceFieldsNew>().HasNoKey();

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblSetting>().HasNoKey();

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.VwAankomendeInspecty>().HasNoKey();

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.VwKiwainspecty>().HasNoKey();

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFactRegel>()
              .HasOne(i => i.TblProjectOnderdelen)
              .WithMany(i => i.TblFactRegels)
              .HasForeignKey(i => i.FldFdprojOndId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .HasOne(i => i.TblProjecten)
              .WithMany(i => i.TblProjectOnderdelens)
              .HasForeignKey(i => i.FldProjectId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Adre>()
              .Property(p => p.Klantnum)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Adre>()
              .Property(p => p.Cursistnr)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Adre>()
              .Property(p => p.Cursist)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Adre>()
              .Property(p => p.Bedrijfsadresid)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Adre>()
              .Property(p => p.Bedrijskoppeling)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Adre>()
              .Property(p => p.Leverancier)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Adre>()
              .Property(p => p.Esteco)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Adre>()
              .Property(p => p.Attentie)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Adre>()
              .Property(p => p.CursistId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Adre>()
              .Property(p => p.OldId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Correspondentie>()
              .Property(p => p.KlantId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Correspondentie>()
              .Property(p => p.FldCorProjNum)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Correspondentie>()
              .Property(p => p.FldCorOpdrachtNum)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Correspondentie>()
              .Property(p => p.FldCorConsultancyId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Correspondentie>()
              .Property(p => p.FldCorTrainingId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Correspondentie>()
              .Property(p => p.FldCorSoort)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Correspondentie>()
              .Property(p => p.FldCorCpersId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField>()
              .Property(p => p.Gebruikt)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.StblReplaceField>()
              .Property(p => p.Gebruikt)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.StblReplaceFieldsNew>()
              .Property(p => p.Gebruikt)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFactRegel>()
              .Property(p => p.FldFdprojOndId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFactRegel>()
              .Property(p => p.FldFdfactId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFactRegel>()
              .Property(p => p.FldFdprijsId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFactRegel>()
              .Property(p => p.FldFdpercentage)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFactRegel>()
              .Property(p => p.FldFdaantal)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFactRegel>()
              .Property(p => p.FldFdprijsStukEur)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFactRegel>()
              .Property(p => p.FldFdtotPrijsEur)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFactRegel>()
              .Property(p => p.FldFdbtwperc)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFactRegel>()
              .Property(p => p.FldFdopdrachtId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFactRegel>()
              .Property(p => p.FldFdafgerond)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFactRegel>()
              .Property(p => p.FldFdfacturerenAanId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFaktuur>()
              .Property(p => p.FacturerenAan)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFaktuur>()
              .Property(p => p.FactContPers)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFaktuur>()
              .Property(p => p.FaktuurEur)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFaktuur>()
              .Property(p => p.Project)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFaktuur>()
              .Property(p => p.Btweur)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFaktuur>()
              .Property(p => p.TotaalEur)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblGlobal>()
              .Property(p => p.FactuurHandtekening)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblGlobal>()
              .Property(p => p.DisplayMailVoorVerzenden)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblMemo>()
              .Property(p => p.WerknId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblMemo>()
              .Property(p => p.FldMklantId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblMemo>()
              .Property(p => p.FldMofferteId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblMemo>()
              .Property(p => p.FldMprojectId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblMemo>()
              .Property(p => p.FldOpdrachtId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblMemo>()
              .Property(p => p.FldMactieVoor)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblMemo>()
              .Property(p => p.FldMprioriteit)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjecten>()
              .Property(p => p.FldProjectNummer)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjecten>()
              .Property(p => p.FldOpdrachtgeverId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjecten>()
              .Property(p => p.FldArchiefMap)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjecten>()
              .Property(p => p.FldKiwaCert)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldOpdrachtId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldOpdrachtNr)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldProjectId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldSoort)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldPrijsId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldVolgnr)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldBedrag)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldStatus)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldFactuurRegelId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldOpdrachtgeverId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldAantalKms)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldKmvergoeding)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldFacturering)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldCertKeuring)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblSoortProject>()
              .Property(p => p.Facturering)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblSoortProject>()
              .Property(p => p.OpEenRegel)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Werknemer>()
              .Property(p => p.FldPensioenverz)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Werknemer>()
              .Property(p => p.AantalKinderen)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Werknemer>()
              .Property(p => p.FldAdministrator)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Werknemer>()
              .Property(p => p.FldFacturering)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Werknemer>()
              .Property(p => p.FldPlanningAlles)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Werknemer>()
              .Property(p => p.FldPlanningKiwa)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Werknemer>()
              .Property(p => p.FldPlanningSgg)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Werknemer>()
              .Property(p => p.FldPlanningSteekproeven)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Werknemer>()
              .Property(p => p.FldPlanningOverig)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Werknemer>()
              .Property(p => p.DashBoardId)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.StblReplaceFieldsNew>()
              .Property(p => p.Id)
              .ValueGeneratedOnAddOrUpdate().UseIdentityColumn()
              .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Adre>()
              .Property(p => p.GebDatum)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Adre>()
              .Property(p => p.DatumCursusdoc)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Adre>()
              .Property(p => p.DatumJaarMon1)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Adre>()
              .Property(p => p.DatumJaarMon2)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Adre>()
              .Property(p => p.DatumJaarMon3)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Correspondentie>()
              .Property(p => p.FldCorDatum)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Correspondentie>()
              .Property(p => p.FldCorDatum2)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFactRegel>()
              .Property(p => p.FldFddatum)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFaktuur>()
              .Property(p => p.Datum2)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFaktuur>()
              .Property(p => p.Betaald)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFaktuur>()
              .Property(p => p.Datum1eHerrin)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFaktuur>()
              .Property(p => p.Datum2eHerrin)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblFaktuur>()
              .Property(p => p.DatumLaanman)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblKeuring>()
              .Property(p => p.FldKeurPeriodeVan)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblKeuring>()
              .Property(p => p.FldKeurPeriodeTot)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblKeuring>()
              .Property(p => p.FldKeurPlandatum)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblKeuring>()
              .Property(p => p.FldKeurUitgevoerd)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblMemo>()
              .Property(p => p.FldMdatum)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblMemo>()
              .Property(p => p.FldMactieDatum)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblMemo>()
              .Property(p => p.FldMactieGereed)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjecten>()
              .Property(p => p.FldDatum)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldPlanDatum)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldGefactureerd)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldDatumGereed)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldGereedVoor)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.Datum1eInspectie1)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.EindDatumContract)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.BelDatum)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.Toegekend)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.AppointmentDateTime)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldPlanPeriodeVan)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>()
              .Property(p => p.FldPlanPeriodeTot)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Werknemer>()
              .Property(p => p.FldDatumIndienst)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Werknemer>()
              .Property(p => p.FldDatumUitDienst)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Werknemer>()
              .Property(p => p.FldGebDatum)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.Werknemer>()
              .Property(p => p.FldDatumBurgStaat)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.VwAankomendeInspecty>()
              .Property(p => p.DatumGereed)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.VwAankomendeInspecty>()
              .Property(p => p.Toegekend)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.VwAankomendeInspecty>()
              .Property(p => p.AppointmentDateTime)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.VwKiwainspecty>()
              .Property(p => p.FldPlanDatum)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.VwKiwainspecty>()
              .Property(p => p.PlanDatum)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.VwKiwainspecty>()
              .Property(p => p.FldGereedVoor)
              .HasColumnType("datetime");

            builder.Entity<KlantBaseWebDemo.Models.KlantBase.VwKiwainspecty>()
              .Property(p => p.FldDatumGereed)
              .HasColumnType("datetime");
            this.OnModelBuilding(builder);
        }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.Adre> Adres { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.Contactpersonen> Contactpersonens { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.Correspondentie> Correspondenties { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblMaanden> StblMaandens { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblSelectie> StblSelecties { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblActieSoort> StblActieSoorts { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblAfwerking> StblAfwerkings { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblBelStatus> StblBelStatuses { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblBook> StblBooks { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField> StblCorrespondentieFields { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort> StblDocumentSoorts { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblFabrikant> StblFabrikants { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblFactureermethode> StblFactureermethodes { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting> StblFactuurKortings { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblGlobal> StblGlobals { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblMeldsoort> StblMeldsoorts { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie> StblOpdrachtCategories { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblPriority> StblPriorities { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort> StblRelatieSoorts { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblReplaceField> StblReplaceFields { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblReplaceFieldsNew> StblReplaceFieldsNews { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblSelectieCode> StblSelectieCodes { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblStatus> StblStatuses { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblSysteem> StblSysteems { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort> StblTrainingsSoorts { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.Stucadoor> Stucadoors { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.TblFactRegel> TblFactRegels { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.TblFaktuur> TblFaktuurs { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.TblGlobal> TblGlobals { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.TblKeuring> TblKeurings { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.TblMemo> TblMemos { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.TblProjecten> TblProjectens { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar> TblProjectM2jaars { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.TblProjectNr> TblProjectNrs { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen> TblProjectOnderdelens { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen> TblRelatiePartijens { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.TblSetting> TblSettings { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.TblSoortProject> TblSoortProjects { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc> TblStandaardDocs { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.Werknemer> Werknemers { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.VwAankomendeInspecty> VwAankomendeInspecties { get; set; }

        public DbSet<KlantBaseWebDemo.Models.KlantBase.VwKiwainspecty> VwKiwainspecties { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}