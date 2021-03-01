/*
* Digital Excellence Copyright (C) 2020 Brend Smits
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as published
* by the Free Software Foundation version 3 of the License.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU Lesser General Public License for more details.
*
* You can find a copy of the GNU Lesser General Public License
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

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
