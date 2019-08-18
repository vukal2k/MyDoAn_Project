using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BUS;
using DTO;
using System.Web.Mvc;
using System.IO;

namespace SimpleBlogPresentation.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PostManagerController : Controller
    {
        private PostBUS postBus;
        public PostManagerController()
        {
            postBus = new PostBUS();
        }
        // GET: Admin/PostManager
        public ActionResult Index()
        {
            List<string> error = new List<string>();
            var posts = postBus.GetAllPostPublishedByPage(error).ToList();
            ViewBag.Errors = error;
            return View(posts);
        }

        public ActionResult Details(int id)
        {
            List<string> error = new List<string>();
            var post = postBus.GetById(id, error);
            ViewBag.Errors = error;
            return View(post);
        }

        public ActionResult Create()
        {
            TagBUS tagBus = new TagBUS();
            CategoryBUS categoryBus = new CategoryBUS();
            List<string> error = new List<string>();
            ViewBag.Tags = tagBus.GetAll(error).ToList();
            ViewBag.Categories = new SelectList(categoryBus.GetAll(error), "CategoryId", "Name");
            ViewBag.Errors = error;
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(FormCollection formCollection, Post post,HttpPostedFileBase UrlSlug)
        {
            string destinationPathPic = "";
            if (UrlSlug != null)
            {
                string sourceNamePic = Path.GetFileName(UrlSlug.FileName);
                destinationPathPic = Path.Combine(Server.MapPath(@"~/Asset/img"), sourceNamePic);
                UrlSlug.SaveAs(destinationPathPic);
                destinationPathPic = $@"/Asset/img/{sourceNamePic}";
            }
            post.UrlSlug = destinationPathPic;
            List<string> error = new List<string>();
            post.Published = formCollection["Published"] == null ? false : true;
            //Post post = new Post
            //{
            //    Title = formCollection["Title"] == null ? "" : formCollection["Title"],
            //    CategoryId = int.Parse(formCollection["CategoryId"] == null ? "0" : formCollection["CategoryId"]),
            //    Description = formCollection["Description"] == null ? "" : formCollection["Description"],
            //    ShortDescription = formCollection["ShortDescription"] == null ? "" : formCollection["ShortDescription"],
            //    Meta = formCollection["Meta"] == null ? "" : formCollection["Meta"],
            //    PostedOn = DateTime.Now,
            //    Published = formCollection["Published"] == null ? false : true,
            //    UrlSlug = "#1",
            //};

            TagBUS tagBus = new TagBUS();
            var tagsId = formCollection["tagId"];
            var tags = tagBus.GetAll(error).ToList();
            var tagsOfPost = tagBus.GetAll(error).Where(t => (tagsId.Contains($"tag{t.TagId}"))).ToList();

            postBus.Insert(post, tagsOfPost, error);
            if (error.Count == 0)
            {
                return RedirectToAction("Details", "PostManager", new { @id = post.PostId });
            }
            else
            {
                CategoryBUS categoryBus = new CategoryBUS();
                ViewBag.Categories = new SelectList(categoryBus.GetAll(error), "CategoryId", "Name");
                ViewBag.Tags = tags;
                ViewBag.Errors = error;
            }
            return View(post);
        }

        public ActionResult Edit(int id)
        {
            TagBUS tagBus = new TagBUS();
            CategoryBUS categoryBus = new CategoryBUS();
            List<string> error = new List<string>();
            Post post = postBus.GetById(id, error);
            ViewBag.Tags = tagBus.GetAll(error).ToList();
            ViewBag.Categories = new SelectList(categoryBus.GetAll(new List<string>()), "CategoryId", "Name");
            ViewBag.Errors = error;
            return View(post);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(FormCollection formCollection, Post post,HttpPostedFileBase UrlSlug)
        {
            List<string> error = new List<string>();
            string destinationPathPic = "";
            if (UrlSlug != null)
            {
                string sourceNamePic = Path.GetFileName(UrlSlug.FileName);
                destinationPathPic = Path.Combine(Server.MapPath(@"~/Asset/img"), sourceNamePic);
                UrlSlug.SaveAs(destinationPathPic);
                destinationPathPic = $@"/Asset/img/{sourceNamePic}";
            }
            else
            {
                destinationPathPic = postBus.GetById(post.PostId, error).UrlSlug;
            }
            post.UrlSlug = destinationPathPic;
            post.Published = formCollection["Published"] == null ? false : true;
            //Post post = new Post
            //{
            //    PostId = id,
            //    Title = formCollection["Title"] == null ? "" : formCollection["Title"],
            //    CategoryId = int.Parse(formCollection["CategoryId"] == null ? "0" : formCollection["CategoryId"]),
            //    Description = formCollection["Description"] == null ? "" : formCollection["Description"],
            //    ShortDescription = formCollection["ShortDescription"] == null ? "" : formCollection["ShortDescription"],
            //    Meta = formCollection["Meta"] == null ? "" : formCollection["Meta"],
            //    PostedOn = DateTime.Parse(formCollection["PostedOn"]),
            //    Modified = DateTime.Now,
            //    Published = formCollection["Published"] == null ? false : true,
            //    UrlSlug = "#1",
            //};

            TagBUS tagBus = new TagBUS();
            var tagsId = formCollection["tagId"];
            var tags = tagBus.GetAll(error).ToList();
            var tagsOfPost = tagBus.GetAll(error).Where(t => (tagsId.Contains($"tag{t.TagId}"))).ToList();

            postBus.Update(post, tagsOfPost, error);
            if (error.Count == 0)
            {
                return RedirectToAction("Details", "PostManager", new { @id = post.PostId });
            }
            else
            {
                CategoryBUS categoryBus = new CategoryBUS();
                ViewBag.Categories = new SelectList(categoryBus.GetAll(error), "CategoryId", "Name");
                ViewBag.Tags = tags;
                ViewBag.Errors = error;
            }
            return View(post);
        }

        public ActionResult Delete(int id)
        {
            List<string> error = new List<string>();
            postBus.Delete(id,error);
            return RedirectToAction("Index");
        }

    }
}
