namespace DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Task")]
    public partial class Tassk
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tassk()
        {
            Solutions = new HashSet<Solutionn>();
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

        public int TaskTypeId { get; set; }

        public int? ModuleId { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public int Level { get; set; }

        public int StatusId { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public virtual LookupStatus LookupStatu { get; set; }

        public virtual Module Module { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Solutionn> Solutions { get; set; }

        public virtual TaskType TaskType { get; set; }

        public virtual UserInfo UserInfo { get; set; }

        public virtual UserInfo UserInfo1 { get; set; }
    }
}
