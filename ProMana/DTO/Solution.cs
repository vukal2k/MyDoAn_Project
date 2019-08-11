namespace DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Solution")]
    public partial class Solution
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public int? ResolveType { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Reason { get; set; }

        [Column("Solution", TypeName = "ntext")]
        [Required]
        public string SolutionDescription { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public virtual ResolveType ResolveType1 { get; set; }

        public virtual Task Task { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
