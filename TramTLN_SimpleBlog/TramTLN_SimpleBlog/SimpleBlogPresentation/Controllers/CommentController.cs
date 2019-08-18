using BUS;
using DTO;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SimpleBlogPresentation.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private const string commentJsonForm = "{'Content':'{0}','PostId':{1},'Username':'{2}'}";
        private CommentBUS commentBus;
        public CommentController()
        {
            commentBus = new CommentBUS();
        }
        // GET: Comment
        [HttpPost]
        public ActionResult PostComment(FormCollection formCollection)
        {
            List<string> error = new List<string>();
            Comment comment = new Comment
            {
                Content = formCollection["content"],
                PostId = int.Parse(formCollection["postId"]),
                Username = User.Identity.GetUserName()
            };
            commentBus.Insert(comment, error);
            return Content(JsonConvert.SerializeObject(comment).ToString());
        }

        [AllowAnonymous]
        [HttpPost]
        public ContentResult Delete(int commentId)
        {
            //int commentId = int.Parse(formCollection["commentId"]);
            List <string> error = new List<string>();
            
            commentBus.Delete(commentId, error);
            if (error.Count == 0)
            {
                return Content("successss");
            }
            else
            {
                return Content("fail");
            }
        }
    }
}