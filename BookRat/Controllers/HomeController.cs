using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BookRat.Models;
using Microsoft.EntityFrameworkCore;

namespace BookRat.Controllers
{
    public class HomeController : Controller
    {
        private readonly BdBiblioContext _context;

        public HomeController( BdBiblioContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int? membreId = HttpContext.Session.GetInt32("MembreId");
            if (membreId == null) { return View(); }
            try
            {
                var connexion = _context.Connexions
                    .Include(c => c.Membre)
                    .Include(c => c.Membre)
                    .FirstOrDefault(c => c.MembreId == membreId);

                if (connexion != null)
                {
                    if (connexion.Statut != "A")
                    {
                        return Content("<h1>Contactez l'administrateur</h1>", "text/html");
                    }
                    if (connexion.Role == "M")
                    {
                        return RedirectToAction("Index", "Membre");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }
            }
            catch (Exception exception)
            {
                return Content("ERREUR : " + exception.Message);
            }
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
    }
}
