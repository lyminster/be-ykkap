Scaffold-DbContext "Server=192.168.1.143;Database=TMS;User ID=etpuser;Password=etpuser;Trusted_Connection=False;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context "BusinessModelContext" -UseDatabaseNames


public virtual DbSet<VValidateErrorMessage> VValidateErrorMessages { get; set; }
public virtual DbSet<VDODetail> VDODetails { get; set; }
public virtual DbSet<UpdateAmountModel> UpdateAmountModels { get; set; }


 modelBuilder.Entity<VValidateErrorMessage>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("V_ValidateErrorMessage");
                entity.Property(e => e.ErrorMessage)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);
            }); 

 modelBuilder.Entity<UpdateAmountModel>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("UpdateAmountModel");
            });

 modelBuilder.Entity<VDODetail>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VDODetail");
            });


 