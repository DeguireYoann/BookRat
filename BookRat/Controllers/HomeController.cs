using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BookRat.Models;
using System.Data.SqlClient;

namespace BookRat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string? _chaineConnexion;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _chaineConnexion = configuration.GetConnectionString("BdGplccConnectionString");
            _logger = logger;
        }

        public IActionResult Index()
        {
            int? membreId = HttpContext.Session.GetInt32("MembreId");
            if (membreId == null) {  return View(); }
            try
            {
                using SqlConnection connexion = new SqlConnection(_chaineConnexion);
                string requete = "SELECT role, statut FROM membres WHERE id=@membreId";
                SqlCommand commande = new SqlCommand(requete, connexion);
                commande.Parameters.AddWithValue("@membreId", membreId.Value);

                connexion.Open();
                SqlDataReader lecteur = commande.ExecuteReader();
                if (lecteur.Read())
                {

                    if ((string)lecteur["statut"] != "A")
                    {
                        return Content("<h1>Contactez l'administrateur</h1>", "text/html");
                    }
                    if ((string)lecteur["role"] == "M")
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
