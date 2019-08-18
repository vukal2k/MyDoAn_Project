using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class DbInitializer: DropCreateDatabaseIfModelChanges<DbBlogData>
    {
        protected override void Seed(DbBlogData context)
        {
            IEnumerable<Category> cateList = new List<Category>();
            for (int i = 0; i < 3; i++)
            {
                context.Categories.Add(new Category
                {
                    Name = "Category " + (i + 1),
                    Description = "Des cate " + (i + 1),
                    UrlSlug = "" + (i + 1),
                });
            }

            for (int i = 0; i < 3; i++)
            {
                context.Tags.Add(new Tag
                {
                    Name = "Tag " + (i + 1),
                    Description = "Des tag " + (i + 1),
                    UrlSlug = "" + (i + 1),
                });
            }
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
