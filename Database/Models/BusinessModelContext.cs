using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Database.Models
{
    public partial class BusinessModelContext : DbContext
    {
        public BusinessModelContext()
        {
        }

        public BusinessModelContext(DbContextOptions<BusinessModelContext> options)
            : base(options)
        {

        }

        public virtual DbSet<FileLog> FileLogs { get; set; }
        public virtual DbSet<FileLogDetail> FileLogDetails { get; set; }
        public virtual DbSet<HelperTable> HelperTables { get; set; }
        public virtual DbSet<JobsLog> JobsLogs { get; set; }


        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyProfile> CompanyProfile { get; set; }
        public virtual DbSet<SocialMedia> SocialMedia { get; set; }
        public virtual DbSet<Showroom> Showroom { get; set; }
        public virtual DbSet<ProjectReferences> ProjectReferences { get; set; }
        public virtual DbSet<CatalogType> CatalogType { get; set; }
        public virtual DbSet<CatalogDetail> CatalogDetail { get; set; }
        public virtual DbSet<ProjectType> ProjectType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=192.168.1.143;user=etpuser;password=etpuser;database=ISSHOST");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeNIK");

                entity.Property(e => e.CreatedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Idclient)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDClient");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeNIK");

                entity.Property(e => e.LastModifiedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.TimeStatus)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });


            modelBuilder.Entity<CompanyProfile>(entity =>
            {
                entity.ToTable("CompanyProfile");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.about).IsUnicode(false);

                entity.Property(e => e.visionMission).IsUnicode(false);

                entity.Property(e => e.imgUrl).IsUnicode(false);

                entity.Property(e => e.youtubeId).IsUnicode(false);

                entity.Property(e => e.CreatedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeNIK");

                entity.Property(e => e.CreatedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Idclient)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDClient");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeNIK");

                entity.Property(e => e.LastModifiedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.TimeStatus)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<ProjectType>(entity =>
            {
                entity.ToTable("ProjectType");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.name).IsUnicode(false);

                entity.Property(e => e.CreatedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeNIK");

                entity.Property(e => e.CreatedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Idclient)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDClient");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeNIK");

                entity.Property(e => e.LastModifiedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.TimeStatus)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<CatalogType>(entity =>
            {
                entity.ToTable("CatalogType");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.name).IsUnicode(false);

                entity.Property(e => e.description).IsUnicode(false);

                entity.Property(e => e.imgUrl).IsUnicode(false);

                entity.Property(e => e.CreatedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeNIK");

                entity.Property(e => e.CreatedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Idclient)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDClient");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeNIK");

                entity.Property(e => e.LastModifiedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.TimeStatus)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<CatalogDetail>(entity =>
            {
                entity.ToTable("CatalogDetail");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.name).IsUnicode(false);
                entity.Property(e => e.CatalogType).IsUnicode(false);
                entity.Property(e => e.description).IsUnicode(false);
                entity.Property(e => e.imgUrl).IsUnicode(false);
                entity.Property(e => e.enPdfUrl).IsUnicode(false);
                entity.Property(e => e.idPdfUrl).IsUnicode(false);

                entity.Property(e => e.CreatedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeNIK");

                entity.Property(e => e.CreatedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Idclient)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDClient");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeNIK");

                entity.Property(e => e.LastModifiedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.TimeStatus)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<ProjectReferences>(entity =>
            {
                entity.ToTable("ProjectReferences");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.name).IsUnicode(false);
                entity.Property(e => e.detail).IsUnicode(false);
                entity.Property(e => e.building).IsUnicode(false);
                entity.Property(e => e.type).IsUnicode(false);
                entity.Property(e => e.location).IsUnicode(false);
                entity.Property(e => e.urlYoutube).IsUnicode(false);
                entity.Property(e => e.listProductUsed).IsUnicode(false);
                entity.Property(e => e.startTimeStamp).HasColumnType("datetime");
                entity.Property(e => e.finishTimestamp).HasColumnType("datetime");

                entity.Property(e => e.CreatedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeNIK");

                entity.Property(e => e.CreatedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Idclient)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDClient");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeNIK");

                entity.Property(e => e.LastModifiedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.TimeStatus)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<SocialMedia>(entity =>
            {
                entity.ToTable("SocialMedia");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.urlFb).IsUnicode(false);

                entity.Property(e => e.urlIg).IsUnicode(false);

                entity.Property(e => e.urlYt).IsUnicode(false);

                entity.Property(e => e.urlWeb).IsUnicode(false);

                entity.Property(e => e.CreatedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeNIK");

                entity.Property(e => e.CreatedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Idclient)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDClient");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeNIK");

                entity.Property(e => e.LastModifiedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.TimeStatus)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Showroom>(entity =>
            {
                entity.ToTable("Showroom");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.name).IsUnicode(false);

                entity.Property(e => e.urlImage).IsUnicode(false);

                entity.Property(e => e.workingHour).IsUnicode(false);
                entity.Property(e => e.address).IsUnicode(false);
                entity.Property(e => e.telephone).IsUnicode(false);

                entity.Property(e => e.CreatedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeNIK");

                entity.Property(e => e.CreatedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Idclient)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDClient");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeNIK");

                entity.Property(e => e.LastModifiedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.TimeStatus)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });




            modelBuilder.Entity<JobsLog>(entity =>
            {
                entity.ToTable("JobsLog");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                    entity.Property(e => e.TableKey)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                    entity.Property(e => e.JobRunning).HasColumnType("datetime");

                    entity.Property(e => e.Description)
                    .IsUnicode(false);


                entity.Property(e => e.CreatedBy)
                                .IsRequired()
                                .IsUnicode(false);

                entity.Property(e => e.CreatedByEmployeeID)
                                .HasMaxLength(50)
                                .IsUnicode(false)
                                .HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedByEmployeeNIK)
                                .HasMaxLength(50)
                                .IsUnicode(false)
                                .HasColumnName("CreatedByEmployeeNIK");

                entity.Property(e => e.CreatedByName)
                                .HasMaxLength(50)
                                .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Idclient)
                                .IsRequired()
                                .HasMaxLength(50)
                                .IsUnicode(false)
                                .HasColumnName("IDClient");

                entity.Property(e => e.LastModifiedBy)
                                .IsRequired()
                                .IsUnicode(false);

                entity.Property(e => e.LastModifiedByEmployeeID)
                                .HasMaxLength(50)
                                .IsUnicode(false)
                                .HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedByEmployeeNIK)
                                .HasMaxLength(50)
                                .IsUnicode(false)
                                .HasColumnName("LastModifiedByEmployeeNIK");

                entity.Property(e => e.LastModifiedByName)
                                .HasMaxLength(50)
                                .IsUnicode(false);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.TimeStatus)
                                .IsRequired()
                                .IsRowVersion()
                                .IsConcurrencyToken();
            });




            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.UserType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeNIK");

                entity.Property(e => e.CreatedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Idclient)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDClient");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeNIK");

                entity.Property(e => e.LastModifiedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.Email).IsUnicode(false);
                entity.Property(e => e.Password).IsUnicode(false);
                entity.Property(e => e.UserType).IsUnicode(false);
                entity.Property(e => e.IsAD).IsUnicode(false);
                entity.Property(e => e.Jabatan).IsUnicode(false);
                entity.Property(e => e.IDVendor).IsUnicode(false);




                entity.Property(e => e.TimeStatus)
                            .IsRequired()
                            .IsRowVersion()
                            .IsConcurrencyToken();
            });




            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("UserType");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

              

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByEmployeeNIK");

                entity.Property(e => e.CreatedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Idclient)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDClient");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedByEmployeeID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedByEmployeeNIK)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LastModifiedByEmployeeNIK");

                entity.Property(e => e.LastModifiedByName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.Code).IsUnicode(false).HasMaxLength(50);
                entity.Property(e => e.Name).IsUnicode(false);
                entity.Property(e => e.RolesCode).IsUnicode(false);
     



                entity.Property(e => e.TimeStatus)
                            .IsRequired()
                            .IsRowVersion()
                            .IsConcurrencyToken();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
