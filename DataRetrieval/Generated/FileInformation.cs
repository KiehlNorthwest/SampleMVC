using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SampleMVC.Data.Entities
{
    /// <summary>
    /// Represents a FileInformation.
    /// NOTE1: This class is generated from a T4 template - you should not modify it manually.
	/// NOTE2: This table was sourced from the includeTables.ttinclude file at the solution root

    /// </summary>
    public partial class FileInformation : BaseEntity<int>, IBaseEntity<int> 
    { 
		//Simple Properties
	
		[Required]
        public override int  Id 
		{
			get { return base.Id;}
			set { base.Id = value;}
		}
		
		[MaxLength(255)]
        public string FileName { get; set; }
		
		public int? PercentSaved { get; set; }
		
		//Navigation Properties 
		
		//Constructor
		public FileInformation() : base()
		{
        			
		}
	}
}
