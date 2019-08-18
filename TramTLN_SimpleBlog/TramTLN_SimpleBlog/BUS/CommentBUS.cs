using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Responsitory;

namespace BUS
{
    public class CommentBUS
    {
        private CommentDAL commentDal;
        public CommentBUS()
        {
            commentDal = new CommentDAL();
        }

        public Comment GetById(int id, List<string> error)
        {
            Comment comment = null;
            try
            {
                comment = commentDal.GetById(id);
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
            return comment;
        }

        public IEnumerable<Comment> GetAll(List<string> error)
        {
            IEnumerable<Comment> comments = new List<Comment>();
            try
            {
                comments = commentDal.GetAll().ToList();
                if (comments == null)
                {
                    error.Add("There are not any post");
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
            return comments;
        }
        

        public void Insert(Comment comment, List<string> error)
        {
            error = Validate(comment, error);
            try
            {
                if (error.Count == 0)
                {
                    commentDal.Insert(comment);
                    commentDal.Save();
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
        }

        public void Update(Comment comment, List<string> error)
        {
            error = Validate(comment, error);
            try
            {
                if (error.Count == 0)
                {
                    commentDal.Update(comment);
                    commentDal.Save();
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
        }

        public void Delete(int categoryId, List<string> error)
        {
            try
            {
                commentDal.Delete(categoryId);
                commentDal.Save();
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
        }


        public List<string> Validate(Comment comment, List<string> error)
        {
            if (comment == null)
            {
                error.Add("Comment have no data");
            }
            if (string.IsNullOrEmpty(comment.Content))
            {
                error.Add("Content is required");
            }
            if (string.IsNullOrEmpty(comment.Username))
            {
                error.Add("Username is required");
            }
            if (comment.PostId==0)
            {
                error.Add("Post is required");
            }
            return error;
        }
    }
}
