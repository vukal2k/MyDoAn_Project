using System;
using System.Collections.Generic;
using System.Linq;
using Responsitory;
using DTO;

namespace BUS
{
    public class PostBUS
    {
        private PostDAL postDal;

        public PostBUS()
        {
            postDal = new PostDAL();
        }

        public Post GetById(int id, List<string>error)
        {
            Post post = null;
            try
            {
                post = postDal.GetById(id);
                if (post == null)
                {
                    error.Add("There are not any post");
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
            return post;
        }
        public IEnumerable<Post> GetAllPostPublishedByPage(List<string> error)
        {
            IEnumerable<Post> post = new List<Post>();
            try
            {
                post = postDal.GetAll().Where(p => p.Published == true).ToList();
                if (post == null)
                {
                    error.Add("There are not any post");
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
            return post;
        }
        public IEnumerable<Post> LatestPost(List<string>error)
        {
            IEnumerable<Post> latestPost = new List<Post>();
            try
            {
                latestPost = postDal.GetAll().Where(p => p.Published == true).OrderByDescending(p => p.PostedOn).ToList().Take(6);
                if (latestPost == null)
                {
                    error.Add("There are not any post");
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
            return latestPost;
        }

        public IEnumerable<Post> ListPostByCategory(int categoryId, List<string> error)
        {
            IEnumerable<Post> listPost = new List<Post>();
            try
            {
                listPost = postDal.GetAll().Where(p => p.Published == true).Where(p => p.CategoryId == categoryId).ToList();
                if (listPost.Count() == 0)
                {
                    error.Add($"There are not any post with this category");
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
            return listPost.ToList();
        }
        

        public IEnumerable<Post> ListPostByTag(int tagId, List<string> error)
        {
            IEnumerable<Post> listPost = new List<Post>();
            try
            {
                //TagDAL tagDal = new TagDAL();
                //Tag tag = tagDal.GetById(tagId);
                listPost = postDal.GetAll().Where(p => p.Published == true).ToList().Where(p => p.Tags.Where(t => t.TagId==tagId).FirstOrDefault()!=null).ToList();
                if (listPost.Count() == 0)
                {
                    error.Add($"There are not any post with this tag");
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
            return listPost.ToList();
        }

        public IEnumerable<Post> SearchPost( string search, List<string> error)
        {
            IEnumerable<Post> listPost = new List<Post>();
            try
            {
                search = search.ToLower();
                listPost = postDal.GetAll().Where(p => p.Published==true).Where(p => p.Title.ToLower().Contains(search)).ToList();
                if (listPost.Count() == 0)
                {
                    error.Add($"There are not any post with this category");
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
            return listPost.ToList();
        }

        public Post DetailPost(int postId,List<string> error)
        {
            Post post = null;
            try
            {
                post = postDal.GetAll().Where(p => p.PostId==postId).FirstOrDefault();
                if (post == null)
                {
                    error.Add("There are not any post");
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
            return post;
        }

        public void Insert(Post post, ICollection<Tag> tags, List<string> error)
        {
            try
            {
                error = Validate(post,error);
                if (error.Count == 0)
                {
                    post.PostedOn = DateTime.Now;
                    postDal.Insert(post,tags);
                    postDal.Save();
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }

        }

        public void Update(Post post, ICollection<Tag> tags, List<string> error)
        {
            try
            {
                //postDal.BeginTransaction();
                error = Validate(post,error);
                if (error.Count == 0)
                {
                    post.Modified = DateTime.Now;
                    postDal = new PostDAL();
                    postDal.Update(post,tags);
                    postDal.Save();

                    //postDal.CommitTransaction();
                }
                else
                {
                    //postDal.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
                //postDal.RollBackTransaction();
            }
        }
        
        public void Delete (int postId, List<string> error)
        {
            try
            {
                if (error.Count == 0)
                {
                    postDal.Delete(postId);
                    postDal.Save();
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
        }
        public List<string> Validate(Post post, List<string>error)
        {
            if (string.IsNullOrEmpty(post.Title))
            {
                error.Add("Title is required");
            }
            if (string.IsNullOrEmpty(post.Description))
            {
                error.Add("Description is required");
            }
            if (string.IsNullOrEmpty(post.ShortDescription))
            {
                error.Add("Short Description is required");
            }
            if (string.IsNullOrEmpty(post.Meta))
            {
                error.Add("Meta Description is required");
            }
            return error;
        }
        public IEnumerable<Post> FilterByPage(int page, ICollection<Post>posts)
        {
            int pageSize = 10;
            var postByPage = posts.OrderBy(p => p.PostId).Skip(pageSize * (page - 1)).Take(6).ToList();
            return postByPage;
        }
    }
}
