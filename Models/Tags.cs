using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVC_Challenge.Models
{
    public class Tags
    {
        public Int64 TagID { get; set; }
        public string FK_PostID {get; set;}
        public string Tag {get; set;}
       
    }
}