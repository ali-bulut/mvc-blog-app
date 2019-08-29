using MvcBlogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBlogApp.Controllers
{
    public class HomeController : Controller
    {
        private BlogContext context = new BlogContext();
        // GET: Home
        public ActionResult Index()
        {
            var blogs = context.Blogs
                .Select(i=> new BlogModel(){

                    Id=i.Id,
                    Title = i.Title.Length > 100 ? i.Title.Substring(0,100) + "..." : i.Title,
                    Explanation=i.Explanation,
                    UploadDate=i.UploadDate,
                    IsHomepage=i.IsHomepage,
                    Confirmation=i.Confirmation,
                    Image=i.Image
                })
                .Where(i => i.IsHomepage == true && i.Confirmation == true);
        
            return View(blogs.ToList());
        }
    }
}