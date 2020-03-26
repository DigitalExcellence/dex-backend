using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;
using Newtonsoft.Json;

namespace Data.Configurations
{
    internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            ValueConverter<string[], string> converter = new ValueConverter<string[], string>(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<string[]>(v));

            ValueComparer<string[]> comparer = new ValueComparer<string[]>(
               (l, r) => JsonConvert.SerializeObject(l) == JsonConvert.SerializeObject(r),
               v => v == null ? 0 : JsonConvert.SerializeObject(v).GetHashCode(),
               v => JsonConvert.DeserializeObject<string[]>(JsonConvert.SerializeObject(v)));

            builder.Property(p => p.Contributors).HasConversion(converter);
            builder.Property(p => p.Contributors).Metadata.SetValueConverter(converter);
            builder.Property(p => p.Contributors).Metadata.SetValueComparer(comparer);

        }
    }
}