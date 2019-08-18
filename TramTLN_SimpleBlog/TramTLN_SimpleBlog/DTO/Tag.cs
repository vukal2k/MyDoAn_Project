using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class Tag
    {
        public int TagId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string UrlSlug { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

        public Tag()
        {
            this.Posts = new HashSet<Post>();
        }
    }
}
