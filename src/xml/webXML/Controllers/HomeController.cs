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
        public IActionResult Index(Model m, IFormFile outagesFile, IFormFile criticalBranchesFile)
        {
            string path = Path.GetTempFileName();
            if (outagesFile != null)
            {
                using (Stream s = System.IO.File.Create(path))
                {
                    outagesFile.CopyTo(s);
                    s.Close();
                }
                m.OutagesCSV = path;
            }

            if (criticalBranchesFile != null)
            {
                path = Path.GetTempFileName();
                using (Stream s = System.IO.File.Create(path))
                {
                    criticalBranchesFile.CopyTo(s);
                    s.Close();
                }
                m.CriticalBranchesCSV = path;
            }

            if (ModelState.IsValid)
            {
                m.Generate();
            }
            return View(m);
        }
    }
}
