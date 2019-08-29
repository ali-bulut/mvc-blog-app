using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBlogApp.Models
{
    public class BlogModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Explanation { get; set; }
        public string Image { get; set; }
        public DateTime UploadDate { get; set; }
        public bool Confirmation { get; set; }
        public bool IsHomepage { get; set; }
    }
}