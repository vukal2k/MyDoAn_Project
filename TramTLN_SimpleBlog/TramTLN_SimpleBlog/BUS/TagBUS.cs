using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Responsitory;
using DTO;

namespace BUS
{

    public class TagBUS
    {
        private TagDAL tagDal;

        public TagBUS()
        {
            tagDal = new TagDAL();
        }
        

        public IEnumerable<Tag> GetAll(List<string> error)
        {
            IEnumerable<Tag> tags = new List<Tag>();
            try
            {
                tags = tagDal.GetAll().ToList();
                if (tags == null)
                {
                    error.Add("There are not any tag");
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
            return tags;
        }

        public Tag GetById(int id, List<string> error)
        {
            Tag tag = null;
            try
            {
                tag = tagDal.GetById(id);
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
            return tag;
        }

        public void Insert(Tag tag, List<string> error)
        {
            error = Validate(tag,error);
            try
            {
                if (error.Count == 0)
                {
                    tagDal.Insert(tag);
                    tagDal.Save();
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
        }

        public void Update(Tag tag, List<string> error)
        {
            error = Validate(tag,error);
            try
            {
                if (error.Count == 0)
                {
                    tagDal.Update(tag);
                    tagDal.Save();
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
        }

        public void Delete(int tagId, List<string> error)
        {
            try
            {
                tagDal.Delete(tagId);
                tagDal.Save();
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
        }

        
        public List<string> Validate(Tag tag, List<string> error)
        {
            if (tag == null)
            {
                error.Add("Tag have no data");
            }
            if (string.IsNullOrEmpty(tag.Name))
            {
                error.Add("Tag name is required");
            }
            if (string.IsNullOrEmpty(tag.Description))
            {
                error.Add("Tag description is required");
            }
            return error;
        }
    }
}
