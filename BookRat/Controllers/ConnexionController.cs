using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using BookRat.Models;

namespace BookRat.Controllers
{
    public class ConnexionController : Controller
    {
        private readonly string? _chaineConnexion;

        // Constructeur de la classe
        public ConnexionController(IConfiguration configuration)
        {
            _chaineConnexion = configuration.GetConnectionString("BdGplccConnectionString");
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
        public IActionResult Connexion(IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Connexion à la base de données pour vérifier les informations de connexion
                    using SqlConnection connexion = new SqlConnection(_chaineConnexion);
                    string requete = "SELECT * FROM connexions WHERE courriel=@courriel AND motdepasse=@motdepasse";
                    SqlCommand commande = new SqlCommand(requete, connexion);

                    commande.Parameters.AddWithValue("@courriel", (string?)collection["courriel"]);
                    commande.Parameters.AddWithValue("@motdepasse", (string?)collection["motdepasse"]);
                    connexion.Open();
                    SqlDataReader lecteur = commande.ExecuteReader();
                    if (lecteur.Read())
                    {
                        // Stocke l'ID du membre en session
                        HttpContext.Session.SetInt32("MembreId", (Int32)lecteur["membreid"]);

                        // Vérifie le statut du membre
                        if ((string)lecteur["statut"] != "A")
                        {
                            return Content("<h1>Contactez l'administrateur</h1>", "text/html");
                        }

                        // Vérifie le rôle du membre et redirige en conséquence
                        if ((string)lecteur["role"] == "A")
                        {
                            HttpContext.Session.SetString("Role", (string)lecteur["role"]);
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
