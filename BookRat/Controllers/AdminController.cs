using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using BookRat.Models;
using BookRat.Helpers;
using BookRat.Filters;

namespace BookRat.Controllers
{
    public class AdminController : Controller
    {
        private readonly string? _chaineConnexion;

        // Constructeur de la classe
        public AdminController(IConfiguration configuration)
        {
            _chaineConnexion = configuration.GetConnectionString("BdGplccConnectionString");
        }

        // Méthode pour afficher la liste des livres (requiert une autorisation de rôle)
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        public IActionResult Index()
        {
            List<Livre> listeLivre = new List<Livre>();
            using (SqlConnection connexion = new SqlConnection(_chaineConnexion))
            {
                string requete = "SELECT l.id, l.titre, l.categorie, l.auteur, l.nbpages, c.titre AS categorie_titre " +
                                 "FROM livres l " +
                                 "JOIN categories c ON l.categorie = c.id";
                SqlCommand commande = new SqlCommand(requete, connexion);
                connexion.Open();
                SqlDataReader lecteur = commande.ExecuteReader();
                while (lecteur.Read())
                {
                    listeLivre.Add(new Livre
                    {
                        Id = lecteur.GetInt32("id"),
                        Titre = lecteur.GetString("titre"),
                        Categorie = lecteur.GetString("categorie_titre"),
                        Auteur = lecteur.GetString("auteur"),
                        NbPages = lecteur.GetInt32("nbpages"),
                    });
                }
            }
            return View(listeLivre);
        }

        // Méthode pour afficher les détails d'un livre (requiert une autorisation de rôle)
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        public IActionResult Details(int id)
        {
            Livre? livre = LivreHelper.GetLivreById(_chaineConnexion ?? "", id);
            if (livre == null)
            {
                return NotFound();
            }

            return View(livre);
        }

        // Méthode pour afficher le formulaire de création d'un livre (requiert une autorisation de rôle)
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        public IActionResult Create()
        {
            List<Categorie> categories = LivreHelper.GetAllCategories(_chaineConnexion ?? "");
            ViewBag.Categories = categories;

            return View();
        }

        // Méthode pour créer un livre (requiert une autorisation de rôle)
        [HttpPost]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        public IActionResult Create(Livre livre)
        {
            try
            {
                using (SqlConnection connexion = new SqlConnection(_chaineConnexion))
                {
                    string requete = "INSERT INTO livres (titre, categorie, auteur, nbpages) VALUES (@titre, @categorie, @auteur, @nbpages)";
                    SqlCommand commande = new SqlCommand(requete, connexion);
                    commande.Parameters.AddWithValue("@titre", livre.Titre);
                    commande.Parameters.AddWithValue("@categorie", livre.Categorie);
                    commande.Parameters.AddWithValue("@auteur", livre.Auteur ?? "");
                    commande.Parameters.AddWithValue("@nbpages", livre.NbPages ?? 0);

                    connexion.Open();
                    commande.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // Méthode pour afficher le formulaire de modification d'un livre (requiert une autorisation de rôle)
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        public IActionResult Edit(int id)
        {
            List<Categorie> categories = LivreHelper.GetAllCategories(_chaineConnexion ?? "");
            Livre? livre = LivreHelper.GetLivreById(_chaineConnexion ?? "", id);
            if (livre == null)
            {
                return NotFound();
            }
            ViewBag.Categories = categories;
            return View(livre); ;
        }

        // Méthode pour modifier un livre (requiert une autorisation de rôle)
        [HttpPost]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        public IActionResult Edit(int id, Livre livre)
        {
            try
            {
                using (SqlConnection connexion = new SqlConnection(_chaineConnexion))
                {
                    string requete = "UPDATE livres SET titre = @titre, categorie = @categorie, auteur = @auteur, nbpages = @nbpages WHERE id = @id";
                    SqlCommand commande = new SqlCommand(requete, connexion);
                    commande.Parameters.AddWithValue("@id", id);
                    commande.Parameters.AddWithValue("@titre", livre.Titre);
                    commande.Parameters.AddWithValue("@categorie", livre.Categorie);
                    commande.Parameters.AddWithValue("@auteur", livre.Auteur);
                    commande.Parameters.AddWithValue("@nbpages", livre.NbPages);

                    connexion.Open();
                    commande.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // Méthode pour afficher le formulaire de suppression d'un livre (requiert une autorisation de rôle)
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        public IActionResult Delete(int id)
        {
            Livre? livre = LivreHelper.GetLivreById(_chaineConnexion ?? "", id);

            return View(livre);
        }

        // Méthode pour gérer la suppression d'un livre (requiert une autorisation de rôle)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (SqlConnection connexion = new SqlConnection(_chaineConnexion))
                {
                    string requete = "DELETE FROM livres WHERE id = @id";
                    SqlCommand commande = new SqlCommand(requete, connexion);
                    commande.Parameters.AddWithValue("@id", id);

                    connexion.Open();
                    commande.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
