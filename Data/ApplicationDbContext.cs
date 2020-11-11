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

using System.Collections.Generic;
using Data.Configurations;
using Data.Helpers;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    /// <summary>
    /// ApplicationDatabaseContext
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>
        /// The project.
        /// </value>
        public DbSet<Project> Project { get; set; }
        /// <summary>
        /// Gets or sets the collaborators.
        /// </summary>
        /// <value>
        /// The collaborators.
        /// </value>
        public DbSet<Collaborator> Collaborators { get; set; }
        /// <summary>
        /// Gets or sets the highlight.
        /// </summary>
        /// <value>
        /// The highlight.
        /// </value>
        public DbSet<Highlight> Highlight { get; set; }
        /// <summary>
        /// Gets or sets the embedded project.
        /// </summary>
        /// <value>
        /// The embedded project.
        /// </value>
        public DbSet<EmbeddedProject> EmbeddedProject { get; set; }
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public DbSet<Role> Role { get; set; }
        /// <summary>
        /// Gets or sets the institution.
        /// </summary>
        /// <value>
        /// The institution.
        /// </value>
        public DbSet<Institution> Institution { get; set; }
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public DbSet<File> File { get; set; }

        public DbSet<UserProject> UserProject { get; set; }
        public DbSet<UserUser> UserUser { get; set; }

        /// <summary>
        /// Gets or sets the Call to Action
        /// </summary>
        /// /// <value>
        /// The call to action.
        /// </value>
        public DbSet<UserTask> UserTask { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        }
    }

}
