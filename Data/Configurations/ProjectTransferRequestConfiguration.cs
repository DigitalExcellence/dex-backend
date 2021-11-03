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

    internal class ProjectTransferRequestConfiguration : IEntityTypeConfiguration<ProjectTransferRequest>
    {

        public void Configure(EntityTypeBuilder<ProjectTransferRequest> builder)
        {
            builder.HasKey(ptr => ptr.Id);

            builder.Property(ptr => ptr.Status).IsRequired();
            builder.Property(ptr => ptr.CurrentOwnerAcceptedRequest).IsRequired();
            builder.Property(ptr => ptr.PotentialNewOwnerAcceptedRequest).IsRequired();

            builder.HasIndex(ptr => ptr.TransferGuid).IsUnique();
        }
    }
}
