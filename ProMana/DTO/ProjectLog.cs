namespace DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectLog")]
    public partial class ProjectLog
    {
        public int Id { get; set; }

        public int Content { get; set; }

        public int? ProjectId { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Project Project { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
