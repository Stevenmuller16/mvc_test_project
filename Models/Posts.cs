using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVC_Challenge.Models
{
    public class Posts
    {
        public Int64 PostID { get; set; }
        public Int64 FK_UserID { get; set; }
        public String PostTitle { get; set; }
        public DateTime PostDate { get; set; }
        public String PostAuthor { get; set; }
        public String PostBody { get; set; }
        public String Image { get; set; }
        
    }
}