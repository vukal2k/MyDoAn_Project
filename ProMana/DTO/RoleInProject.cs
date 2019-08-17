namespace DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoleInProject")]
    public partial class RoleInProject
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        public bool IsActive { get; set; }

        public virtual JobRole JobRole { get; set; }

        public virtual Module Module { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
