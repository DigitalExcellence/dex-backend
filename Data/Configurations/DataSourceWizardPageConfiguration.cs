using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;

namespace Data.Configurations
{

    public class DataSourceWizardPageConfiguration : IEntityTypeConfiguration<DataSourceWizardPage>
    {

        public void Configure(EntityTypeBuilder<DataSourceWizardPage> builder)
        {
            builder.HasKey(dw => new
                                 {
                                     dw.DataSourceId,
                                     dw.WizardPageId,
                                     dw.AuthFlow
                                 });

            builder.HasOne(dw => dw.DataSource)
                   .WithMany(d => d.DataSourceWizardPages)
                   .HasForeignKey(dw => dw.DataSourceId);

            builder.HasOne(dw => dw.WizardPage)
                   .WithMany(w => w.DataSourceWizardPages)
                   .HasForeignKey(dw => dw.WizardPageId);
        }

    }

}
