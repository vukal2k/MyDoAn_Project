using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data.Entity;

namespace Responsitory
{
    public class PostDAL:AllResponsitory<Post>
    {
        public void Insert(Post post, ICollection<Tag> tags)
        {
            post.Tags = new List<Tag>();


            Tag tag;
            foreach (var item in tags)
            {
                tag = new Tag
                {
                    TagId = item.TagId,
                    Description = item.Description,
                    Name = item.Name,
                    UrlSlug = item.UrlSlug
                };
                Context.Tags.Attach(tag);
                post.Tags.Add(tag);
            }

            Context.Posts.Add(post);
        }

        public void Update(Post post, ICollection<Tag> tags)
        {
            //var postFromDb = GetById(post.PostId);
            //postFromDb.Tags=tags;
            //Save();
            //Context.Posts.Attach(post);
            //post.Tags= new List<Tag>();

            //Tag tag;
            //foreach (var item in tags)
            //{
            //    //tag = new Tag
            //    //{
            //    //    TagId = item.TagId,
            //    //    Description = item.Description,
            //    //    Name = item.Name,
            //    //    UrlSlug = item.UrlSlug
            //    //};
            //    Context.Tags.Attach(item);
            //    post.Tags.Add(item);
            //}

            //Update(post);

            //Delete(post.PostId);
            /* 1- Get existing data from database */
            Update(post);
            Context.SaveChanges();

            Context = new DbBlogData();
            var existingPost = Context.Posts.Find(post.PostId);

            /* 2- Find deleted courses from student's course collection by 
            students' existing courses (existing data from database) minus students' 
            current course list (came from client in disconnected scenario) */
            var deletedTags = existingPost.Tags.Where(t=>tags.Contains(t)==false).ToList();

            /* 3- Find Added courses in student's course collection by students' 
            current course list (came from client in disconnected scenario) minus 
            students' existing courses (existing data from database)  */
            var addedTags = tags.Where(t => existingPost.Tags.Contains(t) == false).ToList();

            /* 4- Remove deleted courses from students' existing course collection 
            (existing data from database)*/
            deletedTags.ForEach(c => existingPost.Tags.Remove(c));
            Context.SaveChanges();

            Context = new DbBlogData();
            existingPost = Context.Posts.Find(post.PostId);
            //5- Add new courses
            Tag tag;
            foreach (Tag item in addedTags)
            {
                tag = new Tag
                {
                    TagId = item.TagId,
                    Description = item.Description,
                    Name = item.Name,
                    UrlSlug = item.UrlSlug
                };
                /*6- Attach courses because it came from client 
                as detached state in disconnected scenario*/

                Context.Tags.Attach(tag);

                //7- Add course in existing student's course collection
                existingPost.Tags.Add(tag);
            }
        }
    }
}
