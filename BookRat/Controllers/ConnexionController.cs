using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookRat.Models;

namespace BookRat.Controllers
{
    public class ConnexionController : Controller
    {
        private readonly BdBiblioContext _context;

        // Constructeur de la classe
        public ConnexionController(BdBiblioContext context)
        {
            _context = context;
        }

        // Méthode pour gérer la connexion des utilisateurs
        public IActionResult Connexion()
        {
            // Vérifie si l'utilisateur est déjà connecté
            if (HttpContext.Session.GetInt32("MembreId") != null)
            {
                // Redirige l'utilisateur en fonction de son rôle
                if (HttpContext.Session.GetString("Role") == "A")
                {
                    return RedirectToAction("Index", "Admin");
                }
                return RedirectToAction("Index", "Membre");
            }
            return View();
        }

        // Méthode pour gérer la soumission du formulaire de connexion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Connexion(ConnexionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Recherche de l'utilisateur dans la base de données
                    var connexion = _context.Connexions
                        .Where(c => c.Courriel == model.Courriel && c.MotDePasse == model.MotDePasse)
                        .FirstOrDefault();

                    if (connexion != null)
                    {
                        // Stocke l'ID du membre en session
                        HttpContext.Session.SetInt32("MembreId", connexion.MembreId);

                        // Vérifie le statut du membre
                        if (connexion.Statut != "A")
                        {
                            return Content("<h1>Contactez l'administrateur</h1>", "text/html");
                        }

                        // Vérifie le rôle du membre et redirige en conséquence
                        if (connexion.Role == "A")
                        {
                            HttpContext.Session.SetString("Role", connexion.Role);
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Membre");
                        }
                    }
                    else
                    {
                        return Content("<h1>Vérifiez vos données de connexion !</h1>", "text/html");
                    }
                }
                catch (Exception exception)
                {
                    return Content("ERREUR : " + exception.Message);
                }
            }
            else
            {
                return Content("<h1>Erreur de validation</h1>", "text/html");
            }
        }

        // Méthode pour gérer la déconnexion de l'utilisateur
        public IActionResult Deconnexion()
        {
            try
            {
                // Efface toutes les données de session
                HttpContext.Session.Clear();
            }
            catch (Exception exception)
            {
                return Content("<h2> Une erreur est survenue: " + exception.Message + " </h2>");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
