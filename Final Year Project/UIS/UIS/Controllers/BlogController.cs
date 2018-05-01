using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UIS.Models;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.IO;


using System.Net.Mail;
using PagedList;
using System.Dynamic;


namespace UIS.Controllers
{
    public class BlogController : Controller
    {
        private UisContext db = new UisContext();
        private const int PostPerPage = 4;
        // GET: /Blog/

    

        ////ষ্টুডেন্ট লগিন ... গেট
        //public ActionResult LoginStudent()
        //{
        //    return View();
        //}

        ////ষ্টুডেন্ট লগিন ... পোষ্ট
        //[HttpPost]
        //public ActionResult LoginStudent(int userName,string password)
        //{
        //    string strUserName = Convert.ToString(userName);
            
        //    var usr = db.Students.Single(u => u.studentId == strUserName && u.Password == password);
        //    if(usr!=null)
        //    {
        //        Session["userId"] = usr.studentId.ToString();
        //        Session["userNick"] = usr.nickName.ToString();
        //        Session["userName"] = usr.fullName.ToString();
        //        Session["photo"] = usr.Photo.ToString();
        //        Session["userFaculty"] = usr.Faculty.ToString();
        //        return RedirectToAction("Index");
        //    }

        //    else
        //    {
        //        ModelState.AddModelError("", "UserName Or Pass Is Wrong");
                
               
        //    }

        //    return View();
        //}

        //লগিন না করে কেউ পোষ্ট ক্রিয়েট করতে চাইলে
        public ActionResult CreatePostTemp()
        {
            return View();
        }

        //পোষ্ট ক্রিয়েট করা ... গেট
        public ActionResult CreatePost()
        {
           
            if(Session["userNick"]==null)
            {

                return RedirectToAction("CreatePostTemp");

            }
            else
            {
                return View();
            }
            
        }
        
        // পোষ্ট ক্রিয়েট করা ... পোষ্ট
        [HttpPost]
        public ActionResult CreatePost (string dummy)
        {

            return RedirectToAction("Index");
        }

        // ইনডেক্স পেজ ... গেট
        public ActionResult Index(int? id)
        {
            

            //List <Post> post = db.Posts.ToList();
            //PagedList<Post> pagedPostModel = new PagedList<Post>(post, page, pageSize);
            

            //dynamic myModel = new ExpandoObject();
            //myModel.allPost = post;
            //myModel.pagedPost = pagedPostModel;

            //return View(myModel);

            int pageNumber = id ?? 0;
            IEnumerable<Post> posts = (from post in db.Posts orderby post.ID descending select post).Skip(pageNumber * PostPerPage).Take(PostPerPage + 1);
            ViewBag.IsPreviousLinkVisible = pageNumber > 0;
            ViewBag.IsNextLinkVisible = posts.Count() > PostPerPage;
            ViewBag.pageNumber = pageNumber;

            IEnumerable<Comment> comments = from comment in db.Comments orderby comment.ID descending select comment;

            dynamic myModel = new ExpandoObject();
            myModel.allPost = db.Posts.ToList();
            myModel.pagedPost = posts.Take(PostPerPage);
            myModel.comment = comments;

            return View(myModel);
            //return View(posts.Take(PostPerPage));
            //return View(db.Posts.ToList());
           
        }

        //ইনডেক্স পেজ ... পোষ্ট
        [HttpPost, ValidateInput(false)]
        public ActionResult Index(string blogTitle, string editor1, HttpPostedFileBase file)
        {

            var path = "";
            var photoPath = "";

            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    if (Path.GetExtension(file.FileName).ToLower() == ".jpg"
                        || Path.GetExtension(file.FileName).ToLower() == ".png"
                              || Path.GetExtension(file.FileName).ToLower() == ".gif"
                                    || Path.GetExtension(file.FileName).ToLower() == ".jpeg")
                    {
                        path = Path.Combine(Server.MapPath("Images"), file.FileName);
                        file.SaveAs(path);
                         //photoPath = "~/Images/" + file.FileName; 
                        photoPath = file.FileName; 
                    }
                }
            }

            editor1 = editor1.Replace("&rdquo;", " ");
            editor1 = editor1.Replace("&ldquo;", " ");
            editor1 = editor1.Replace("&rdquo;", " ");

            editor1 = editor1.Replace("<br>", " ");
            editor1 = editor1.Replace("<br />", " ");
            editor1 = editor1.Replace("&nbsp;", " ");

            editor1 = editor1.Replace("&hellip", " ");
            editor1 = editor1.Replace("&ndash;", " ");
            editor1 = editor1.Replace("&nbsp;", " ");
            editor1 = editor1.Replace("&zwnj;", " ");
            editor1 = editor1.Replace("&rsquo;", " ");
            editor1 = editor1.Replace("&quot;", " ");
            editor1 = editor1.Replace("<p>", " ");
            editor1 = editor1.Replace("</p>", " "); 
       
            Post post = new Post();
            post.Title = blogTitle;
            post.UserName = Session["userNick"].ToString();
            post.DateTime = System.DateTime.Now.ToString();
            post.Body = editor1;
            post.Image = photoPath;

            string stringID = Session["userId"].ToString();
            int intId = Convert.ToInt32(stringID);

            post.UserId = stringID;

            db.Posts.Add(post);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

      
        //কমেন্ট ষ্টোর হওয়ার জন্য পোষ্ট মেথড
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Comment(int id, string editor1)
        {
           
            Comment cmt = new Comment();
            cmt.PostID = id;
            cmt.UserName = Session["userNick"].ToString();
            cmt.DateTime = System.DateTime.Now.ToString();
            cmt.Body = editor1;

            db.Comments.Add(cmt);
            db.SaveChanges();

            return RedirectToAction("Details/"+id);
        }

        //ডিটেইল দেখার জন্য গেট মেথড
        public ActionResult Details (int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Post post = db.Posts.Find(id);
            if(post==null)
            {
                return HttpNotFound();
            }

            return View(post);

        }

    


        //এডমিনের পোষ্টে ডিলিট কিংবা এডিটের মত অপশন পাওয়ার জন্য
        public ActionResult AdminPostDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }

    // ব্লগারের প্রোফাইল দেখার জন্য মানে , প্রোফাইলে পোষ্ট দেখার জন্য    
        public ActionResult BlogProfile()
        {

            var userId = Session["userId"].ToString();
            int intUserId = Convert.ToInt32(userId);
            var profilePost = from s in db.Posts select s;

            if (userId != null)
            {
                profilePost = profilePost.Where(c => c.UserId == userId);
            }

            return View(profilePost);
        }

        //পোষ্ট রিমুভ করার জন্য

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();

            return View();
           // return RedirectToAction("Index");
        }

      
        public ActionResult CommentRemove(int id)
        {
           

            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult logOut()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");

        }

    
        public ActionResult sentMail()
        {
            Random random = new Random();
            int password = random.Next(1000, 100000000);
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("mdrickyislam@gmail.com");
            msg.To.Add("rickygrason@gmail.com");
            msg.Subject = "PSTU Information System- Create Account";
            msg.Body = "Dear User !!! Your Security Code is "+password+". Please Enter This Code For Create Your Account.";
            SmtpClient sc = new SmtpClient("smtp.gmail.com");
            sc.Port = 25;
            sc.Credentials = new NetworkCredential("mdrickyislam@gmail.com", "12357890");
            sc.EnableSsl = true;
            sc.Send(msg);
            Response.Write("Mail send");

            return View();
        }

        public ActionResult MailAuth()
        {

            return View();
            
        }


        public ActionResult CodeAuth()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CodeAuth(string code)
        {
            string email = Session["authMail"].ToString();
            var auth = db.MailAuths.Single(u => u.code == code && u.mail == email);

            if(auth !=null)
            {
                return RedirectToAction("Register");
            }

            else
            {
                return View();
            }
            
            
            
            
            
        }

        [HttpPost]
        public ActionResult MailAuth(string email)
        {
            string myMail = email;
            Session["authMail"] = email;
            MailAuth ma = new MailAuth();

            try
            {
                Random random = new Random();
                int password = random.Next(1000, 100000000);
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("mdrickyislam@gmail.com");
                msg.To.Add(myMail);
                msg.Subject = "PSTU Information System- Create Account";
                msg.Body = "Dear User !!! Thanks For Your Interest. Your Security Code is " + password + ". Please Enter This Code For Creating Your Account.";
                SmtpClient sc = new SmtpClient("smtp.gmail.com");
                sc.Port = 25;
                sc.Credentials = new NetworkCredential("mdrickyislam@gmail.com", "12357890");
                sc.EnableSsl = true;
                sc.Send(msg);

                // TO strore in Database 

                string confirmationCode = Convert.ToString(password);
                ma.mail = email;
                ma.code = confirmationCode;

                db.MailAuths.Add(ma);
                db.SaveChanges();


            }
            catch (Exception ex)
            {
                return View("Error: " + ex);
            }

            return RedirectToAction("CodeAuth");

        }

	}
}