using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using webXML.Models;

namespace webXML.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Model model = new Model();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(Model m)
        {

            if (ModelState.IsValid)
            {
                m.Generate();
            }
            return View(m);
        }
        [HttpPost("upload")]
        public IActionResult Upload(Model m, IFormFile file, int fileType)
        {
            string path = Path.GetTempFileName();
            if (file != null)
            {
                using (Stream s = System.IO.File.Create(path))
                {
                    file.CopyTo(s);
                    s.Close();
                }
                if (fileType == 0)
                {
                    m.OutagesCSV = path;
                } else if (fileType == 1)
                {
                    m.CriticalBranchesCSV = path;
                }
            }
            return Content(path);
        }
        [HttpGet("download")]
        public IActionResult Download(string filename)
        {
            byte[] content = System.IO.File.ReadAllBytes(filename);
            System.IO.File.Delete(filename);
            return File(content, "application/force-download", "output.zip");
        }
    }
}
