using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Responsitory;

namespace BUS
{
    public class CategoryBUS
    {
        private CategoryDAL categoryDal;
        public CategoryBUS()
        {
            categoryDal = new CategoryDAL();
        }

        public Category GetById(int id, List<string> error)
        {
            Category category = null;
            try
            {
                category = categoryDal.GetById(id);
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
            return category;
        }

        public IEnumerable<Category> GetAll(List<string> error)
        {
            IEnumerable<Category> categories = new List<Category>();
            try
            {
                categories = categoryDal.GetAll("Posts").ToList();
                if (categories == null)
                {
                    error.Add("There are not any category");
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
            return categories;
        }

        public void Insert(Category category, List<string> error)
        {
            error = Validate(category,error);
            try
            {
                if (error.Count == 0)
                {
                    categoryDal.Insert(category);
                    categoryDal.Save();
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
        }

        public void Update(Category category, List<string> error)
        {
            error = Validate(category,error);
            try
            {
                if (error.Count == 0)
                {
                    categoryDal.Update(category);
                    categoryDal.Save();
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
                categoryDal.Delete(categoryId);
                categoryDal.Save();
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
        }


        public List<string> Validate(Category category, List<string> error)
        {
            if (category == null)
            {
                error.Add("Category have no data");
            }
            if (string.IsNullOrEmpty(category.Name))
            {
                error.Add("Category name is required");
            }
            if (string.IsNullOrEmpty(category.Description))
            {
                error.Add("Category description is required");
            }
            return error;
        }
    }
}
