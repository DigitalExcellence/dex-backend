using System;
using System.Collections.Generic;
using System.Text;
using Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<User> User { get; set; }
		public DbSet<Project> Project { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			//modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new ProjectConfiguration());

		}
	}
}
