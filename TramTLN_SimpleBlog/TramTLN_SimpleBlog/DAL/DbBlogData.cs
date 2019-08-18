using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class DbBlogData : DbContext
    {
        public DbBlogData() : base()
        {
            Database.SetInitializer(new DbInitializer());
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            MappingCategory(modelBuilder);
            MappingPost(modelBuilder);
            MappingTag(modelBuilder);
        }

        private void MappingCategory(DbModelBuilder modelBuilder)
        {
            var cateEntity = modelBuilder.Entity<Category>();
            cateEntity.Property(c => c.Name)
                      .HasMaxLength(50)
                      .IsRequired();

            cateEntity.Property(c => c.UrlSlug)
                      .HasMaxLength(200);

            cateEntity.HasMany(p => p.Posts)
                      .WithRequired(p => p.Category)
                      .HasForeignKey(p => p.CategoryId);
        }

        private void MappingTag(DbModelBuilder modelBuilder)
        {
            var tagEntity = modelBuilder.Entity<Tag>();
            tagEntity.Property(t => t.Name)
                     .HasMaxLength(50)
                     .IsRequired();
            tagEntity.HasMany(p => p.Posts)
                                  .WithMany(p => p.Tags)                 // Note the empty WithMany()
                                  .Map(x =>
                                  {
                                      x.MapLeftKey("TagId");
                                      x.MapRightKey("PostId");
                                      x.ToTable("TagPost");
                                  });
        }
        private void MappingPost(DbModelBuilder modelBuilder)
        {
            var postEntity = modelBuilder.Entity<Post>();
            postEntity.Property(p => p.Title)
                      .HasMaxLength(500)
                      .IsRequired();

            postEntity.Property(p => p.ShortDescription)
                      .HasMaxLength(5000)
                      .IsRequired();

            postEntity.Property(p => p.Description)
                      .HasMaxLength(5000)
                      .IsRequired();

            postEntity.Property(p => p.Meta)
                      .HasMaxLength(1000)
                      .IsRequired();

            postEntity.Property(p => p.Published)
                      .IsRequired();

            postEntity.Property(p => p.PostedOn)
                      .IsRequired();

            postEntity.HasMany(p => p.Tags)
                                  .WithMany(p => p.Posts)                 // Note the empty WithMany()
                                  .Map(x =>
                                  {
                                      x.MapLeftKey("PostId");
                                      x.MapRightKey("TagId");
                                      x.ToTable("TagPost");
                                  });

            postEntity.HasRequired(c => c.Category)
                      .WithMany(p => p.Posts)
                      .HasForeignKey<int>(p => p.CategoryId);
        }
    }
}
