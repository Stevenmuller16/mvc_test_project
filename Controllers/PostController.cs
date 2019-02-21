using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Challenge.Models;


namespace MVC_Challenge.Controllers
{
    public class PostController : Controller
    {
        public int count = 3;
        // GET: Tournament
        [AllowAnonymous]
        public ActionResult Posts(int id = 0)
        {
            if(id == 0)
            { 
                if (Session["UserID"] != null)
                {
                    nologodbEntities entities = new nologodbEntities();
                    count = 3;
                    return View(from Post in entities.Posts.Take(count) select Post);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            else
            {
                count += id;
                if (Session["UserID"] != null)
                {

                    nologodbEntities entities = new nologodbEntities();



                    return View(from Post in entities.Posts.Take(count) select Post);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }

            }
        }

        public ActionResult SinglePost(int id)
        {
            if (ModelState.IsValid)
            {
                nologodbEntities Post = new nologodbEntities();
                List<Post> GetPostById = Post.Posts.Where(a => a.PostID == id).ToList();
                return View(GetPostById);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult CreatePost(Post post)
        {
            if (ModelState.IsValid)
            {
                    nologodbEntities entities = new nologodbEntities();
                    entities.Posts.Add(post);
                    entities.SaveChanges();
                    return View();  
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        
        public ActionResult EditPost(int id)
        {
            using (nologodbEntities Post = new nologodbEntities())
            {
                var obj = Post.Posts.Where(a => a.PostID == id).FirstOrDefault();
                return View(obj);
            }
        }
        public ActionResult EditedPost(Post post)
        {
            if (ModelState.IsValid)
            {
                if (Session["UserRole"].ToString() == "admin")
                {
                   
                        using (nologodbEntities Posts = new nologodbEntities())
                        {
                            var obj = Posts.Posts.Where(a => a.PostID == post.PostID).FirstOrDefault();
                            if (obj != null)
                            {
                                obj.PostTitle = post.PostTitle;
                                obj.PostImage = post.PostImage;
                                obj.PostBody = post.PostBody;
                                Posts.SaveChanges();

                            }
                        }
                    
                    return RedirectToAction("Posts", "Post");
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult DeletePost(int id)
        {
            if (ModelState.IsValid)
            {
                if (Session["UserRole"].ToString() == "admin")
                {
                    if (ModelState.IsValid)
                    {
                        using (nologodbEntities Posts = new nologodbEntities())
                        {
                            var obj = Posts.Posts.Where(a => a.PostID == id).FirstOrDefault();
                            if (obj != null)
                            {
                                Posts.Posts.Remove(obj);//unsure about validity of statement
                                Posts.SaveChanges();

                            }
                        }
                    }
                    return RedirectToAction("Posts", "Post");
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult PostDetails(int id)
        {
            if (ModelState.IsValid)
            {
                if (Session["UserRole"].ToString() != "")
                {
                    return RedirectToAction("Events", "Event", new{ @TournamentID = id});

                    
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
    }
}