
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

using SampleMVC.Data.Entities;

namespace SampleMVC.Data
{
    public partial class MyDbContext : DbContext
    {
		public MyDbContext() : base("EntityConnection") 
		{ 
		     Configuration.LazyLoadingEnabled = false;
		}

		public MyDbContext(string connectionName) : base(connectionName) 
		{ 
		     Configuration.LazyLoadingEnabled = false;
		}
		public virtual DbSet<FileInformation> FileInformationRecords { get; set; }
		public virtual DbSet<Person> People { get; set; }
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
			CustomOnModelCreating(modelBuilder);
			
			#region Base Tables
			modelBuilder
                .Entity<FileInformation>()
                .ToTable("FileInformation", "dbo")
                .HasKey(fileinformation => fileinformation.Id)

                .Property(fileinformation => fileinformation.Id)
                .HasColumnName("FileInformationId")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

			modelBuilder
                .Entity<Person>()
                .ToTable("Person", "dbo")
                .HasKey(person => person.Id)

                .Property(person => person.Id)
                .HasColumnName("PersonId")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

			#endregion Base Tables

			#region Associations
			#endregion Associations
					
			OnModelCreatingWithIdentities(modelBuilder);
			base.OnModelCreating(modelBuilder);
		}

		protected virtual void OnModelCreatingWithIdentities(DbModelBuilder modelBuilder)
		{
				}
		partial void CustomOnModelCreating(DbModelBuilder modelBuilder);
	}

}

