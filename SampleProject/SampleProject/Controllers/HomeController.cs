using Newtonsoft.Json;
using SampleProject.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SampleProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GeneratePDF()
        {
            var client = new HttpClient();

            string url = $"https://localhost:44368/Home/SamplePdf";

            object parameters = new
            {
                testParam=true
            };

            string data = JsonConvert.SerializeObject(parameters);

            var response = await client.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json"));
            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PdfGeneraitonResult>(responseContent);


            return File(result.FileBytes, "application/pdf", "SamplePdf.pdf");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}