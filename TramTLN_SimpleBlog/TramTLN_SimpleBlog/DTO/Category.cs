using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string UrlSlug { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

        public Category()
        {
            this.Posts = new HashSet<Post>();
        }
    }
}
