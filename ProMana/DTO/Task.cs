namespace DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Task")]
    public partial class Task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task()
        {
            Solutions = new HashSet<Solution>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Required]
        [StringLength(50)]
        public string AssignedTo { get; set; }

        public string TaskType { get; set; }

        public int? ModuleId { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string Severity { get; set; }
        public string Priority { get; set; }
        public bool IsTask { get; set; }

        public int StatusId { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public virtual LookupStatus LookupStatus { get; set; }

        public virtual Module Module { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Solution> Solutions { get; set; }

        public virtual UserInfo UserInfo { get; set; }

        public virtual UserInfo UserInfo1 { get; set; }
        public virtual LookupSeverity LookupSeverity { get; set; }
        public virtual LookupPriority LookupPriority { get; set; }
    }
}
