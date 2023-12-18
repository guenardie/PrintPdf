using Microsoft.AspNetCore.Mvc;
using PrintPdf.Models;
using Spire.Pdf;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace PrintPdf.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Print()
        {
            List<PrinterModel> printerModels = new List<PrinterModel>
            {
                new PrinterModel { PrinterID = 1, PrinterName = "EPSON M100 MIS" },
                new PrinterModel { PrinterID = 2, PrinterName = "EPSON M200 DEV" },
            };

            var pr = (from c in printerModels select c).ToList();
            pr.Insert(0, new PrinterModel { PrinterID = 0, PrinterName = "--Select Pr Name--" });
            ViewBag.printerName = pr;

            return View();
        }

        [HttpPost]
        public IActionResult Print(IFormFile postedFile, string printerName)
        {
            string path = @"C:\mydir\";

            if (postedFile == null || postedFile.Length == 0)

                return BadRequest("No file selected for upload...");

            string fileName = Path.GetFileName(postedFile.FileName);

            string contentType = postedFile.ContentType;

            PdfDocument doc = new PdfDocument();
            //Load a PDF file
            doc.LoadFromFile(path + fileName);

            //Specify printer name
            doc.PrintSettings.PrinterName = printerName;

            //Print document
            doc.Print();

            return View();
        }
    }
}
