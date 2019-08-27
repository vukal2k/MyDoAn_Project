namespace DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Request")]
    public partial class Request
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Title { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Content { get; set; }

        public int? ProjectId { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [StringLength(50)]
        public string ApprovalBy { get; set; }

        public DateTime? ApprovalDateTime { get; set; }

        public int StatusId { get; set; }

        public int IsActive { get; set; }

        public virtual LookupStatus LookupStatu { get; set; }

        public virtual Project Project { get; set; }

        public virtual UserInfo UserInfo { get; set; }

        public virtual UserInfo UserInfo1 { get; set; }
    }
}
