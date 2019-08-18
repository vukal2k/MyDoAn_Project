using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUS;
using DTO;

namespace SimpleBlogPresentation.Controllers
{
    public class BlogController : Controller
    {
        private PostBUS postBUS;
        public BlogController()
        {
            postBUS = new PostBUS();
        }
        public ActionResult GetPost()
        {
            List<string> error = new List<string>();
            var posts = postBUS.GetAllPostPublishedByPage(error).OrderByDescending(p=>p.PostId).ToList();
            ViewBag.IsHome = true;
            ViewBag.Errors = error;
            return View(posts);
        }

        public ActionResult DetailPost(int id)
        {
            List<string> error = new List<string>();
            var post = postBUS.GetById(id, error);
            ViewBag.Errors = error;
            if (post == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(post);
        }
        
        public ActionResult LatestPost()
        {
            List<string> error = new List<string>();
            var lastestPost = postBUS.LatestPost(error);
            ViewBag.Errors = error;
            return View(lastestPost);
        }
        
        public ActionResult GetPostByCate(int id)
        {
            List<string> error = new List<string>();
            var postsByCate = postBUS.ListPostByCategory(id,error).OrderByDescending(p => p.PostId);
            CategoryBUS categoryBus = new CategoryBUS();
            ViewBag.CateName = categoryBus.GetById(id,error).Name;
            ViewBag.Errors = error;
            return View(postsByCate);
        }
        
        public ActionResult GetPostByTag(int id)
        {
            List<string> error = new List<string>();
            var postsByTag = postBUS.ListPostByTag(id, error).OrderByDescending(p => p.PostId);
            TagBUS tagBus = new TagBUS();
            ViewBag.TagName = tagBus.GetById(id, error).Name;
            ViewBag.Errors = error;
            return View(postsByTag);
        }
        
        public ActionResult Search(string search)
        {
            List<string> error = new List<string>();
            var postsSearch = postBUS.SearchPost(search, error).OrderByDescending(p => p.PostId);
            ViewBag.Errors = error;
            ViewBag.Search = search;
            return View(postsSearch);
        }
    }
}