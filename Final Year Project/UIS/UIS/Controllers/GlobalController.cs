using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UIS.Models;
using System.Data.Entity;

namespace UIS.Controllers
{
    public class GlobalController : Controller
    {
        //
        // GET: /Global/
        public ActionResult SemFeeInfoCheck(string faculty, string semester)
        {
            //ViewBag.studentId = id;
            //ViewBag.semester = semester;
            //ViewBag.amount = amount;

            ViewBag.queryList = findAll("2013-14","CSE","8");
            return View();
        }
        public IEnumerable<TempBankModel> findAll(string session,string faculty,string semester)
        {
            try
            {
                string baseUrl = "http://localhost:8024/api/";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(

                    new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("Test?session="+session+"&faculty="+faculty+"&semester="+semester).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<IEnumerable<TempBankModel>>().Result;

                }

                else
                {
                    return null;
                }


            }
            catch
            {
                return null;
            }
        }
	}
}