using IronPdf;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SampleProject.PdfGeneration.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }        

        public ActionResult SamplePdf()
        {
            var document = this.GeneratePdfDocument(this.Sample());

            return Json(new { fileBytes = document });            
        }


        public ActionResult Sample()
        {

            return View("Sample");
        }

        private byte[] GeneratePdfDocument(ActionResult actionToPdf)
        {
            var documentRendered = this.RenderActionResultToString(actionToPdf);

            IronPdf.Logging.Logger.EnableDebugging = true;
            IronPdf.Logging.Logger.LogFilePath = @"C:\logs\sample\ironPDFLog.log"; //May be set to a directory name or full file
            IronPdf.Logging.Logger.LoggingMode = IronPdf.Logging.Logger.LoggingModes.All;

            var renderer = new ChromePdfRenderer();            

            renderer.RenderingOptions.MarginTop = 10;
            renderer.RenderingOptions.MarginBottom = 20;
            renderer.RenderingOptions.MarginLeft = 10;
            renderer.RenderingOptions.MarginRight = 10;
            renderer.RenderingOptions.PaperSize = IronPdf.Rendering.PdfPaperSize.A4;

            renderer.RenderingOptions.FitToPaper = true;
            
            var pdfDocument = renderer.RenderHtmlAsPdf(documentRendered);

            byte[] documentBytes = pdfDocument.BinaryData;
            //if (sign)
            //{
            //    var signedDocument = this.SignPdfDocument(documentBytes, reasonForSigning, visibleSignature);

            //    return signedDocument;
            //}

            return documentBytes;
        }

        private string RenderActionResultToString(ActionResult result)
        {
            var sb = new StringBuilder();
            var memoryWriter = new StringWriter(sb);

            var fakeResponse = new HttpResponse(memoryWriter);
            var fakeContext = new HttpContext(System.Web.HttpContext.Current.Request, fakeResponse);
            var fakeControllerContext = new ControllerContext(
                new HttpContextWrapper(fakeContext),
                this.ControllerContext.RouteData,
                this.ControllerContext.Controller);

            var oldContext = System.Web.HttpContext.Current;
            System.Web.HttpContext.Current = fakeContext;

            result.ExecuteResult(fakeControllerContext);

            System.Web.HttpContext.Current = oldContext;

            memoryWriter.Flush();
            return sb.ToString();
        }
    }
}