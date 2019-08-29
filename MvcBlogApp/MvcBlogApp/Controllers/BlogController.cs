using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcBlogApp.Models;

namespace MvcBlogApp.Controllers
{
    public class BlogController : Controller
    {
        private BlogContext db = new BlogContext();

        public ActionResult List(int? id ,string q)
        {
            var blogs = db.Blogs
                .Where(i=>i.Confirmation==true)
                .Select(i => new BlogModel()
                {
                    Id = i.Id,
                    Title = i.Title.Length > 100 ? i.Title.Substring(0, 100) + "..." : i.Title,
                    Explanation = i.Explanation,
                    UploadDate = i.UploadDate,
                    IsHomepage = i.IsHomepage,
                    Confirmation = i.Confirmation,
                    Image = i.Image,
                    CategoryId = i.CategoryId
                })
                .AsQueryable();
            //AsQueryable'ın mantığı istediğimiz zaman blogs'a where sorgusu yazabiliriz.

            if (id != null)
            {
                blogs = blogs.Where(i => i.CategoryId == id);
            }

            if (string.IsNullOrEmpty("q") == false)
            {
                blogs = blogs.Where(i => i.Title.Contains(q) || i.Explanation.Contains(q));
            }

            

            return View(blogs.ToList());
        }

        // GET: Blog
        public ActionResult Index()
        {
            //her bloğun kategori bilgisi blogs listesine dahil edilip gelir
            var blogs = db.Blogs.Include(b => b.Category).OrderByDescending(i=>i.UploadDate);
            return View(blogs.ToList());
        }

        // GET: Blog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: Blog/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName");
            return View();
        }

        // POST: Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Explanation,Image,Content,CategoryId")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                blog.UploadDate = DateTime.Now;

                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", blog.CategoryId);
            return View(blog);
        }

        // GET: Blog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", blog.CategoryId);
            return View(blog);
        }

        // POST: Blog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Explanation,Image,Content,Confirmation,IsHomepage,CategoryId")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                var entity = db.Blogs.Find(blog.Id);
                if (entity!=null)
                {
                    entity.Title = blog.Title;
                    entity.Explanation = blog.Explanation;
                    entity.Image = blog.Image;
                    entity.Content = blog.Content;
                    entity.Confirmation = blog.Confirmation;
                    entity.IsHomepage = blog.IsHomepage;
                    entity.CategoryId = blog.CategoryId;

                    db.SaveChanges();

                    //viewbag yerine tempdata kullanıyoruz çünkü redirectoaction dendiğinde viewbag içindeki bilgiler
                    //sıfırlanırken tempdatadakiler sıfırlanmaz
                    TempData["Blog"] = entity;

                    return RedirectToAction("Index");
                }
                
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", blog.CategoryId);
            return View(blog);
        }

        // GET: Blog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Blogs.Find(id);
            db.Blogs.Remove(blog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
