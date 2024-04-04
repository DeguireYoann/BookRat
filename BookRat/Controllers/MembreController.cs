using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using BookRat.Models;
using BookRat.Helpers;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using System.Data;
using BookRat.Filters;
using Microsoft.AspNetCore.Authorization;

namespace BookRat.Controllers
{
    public class MembreController : Controller
    {
        private readonly string? _chaineConnexion;

        // Constructeur de la classe
        public MembreController(IConfiguration configuration)
        {
            _chaineConnexion = configuration.GetConnectionString("BdGplccConnectionString");
        }

        // Méthode pour afficher la liste des livres disponibles
        [ServiceFilter(typeof(MemberAuthorizationFilter))]
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
                using (SqlDataReader lecteur = commande.ExecuteReader())
                {
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
                SetSessionNbPret(connexion);
            }
            return View(listeLivre);
        }

        // Méthode pour afficher la page de confirmation
        [ServiceFilter(typeof(MemberAuthorizationFilter))]
        public ActionResult<string> Confirmation()
        {
            return View();
        }

        // Méthode pour afficher le formulaire d'ajout de membre
        public IActionResult AjouterMembre()
        {
            if (HttpContext.Session.GetInt32("MembreId") != null)
            {
                if (HttpContext.Session.GetString("Role") == "A")
                {
                    return RedirectToAction("Index", "Admin");
                }
                return RedirectToAction("Index", "Membre");
            }
            return View();
        }

        // Méthode pour traiter la soumission du formulaire d'ajout de membre
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult<string> AjouterMembre(Membre membre)
        {
            int dernierIdInsere = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(_chaineConnexion))
                    {
                        // Insertion des données du nouveau membre dans la base de données
                        string requete = "INSERT INTO membres (prenom, nom, courriel, sexe, dateNaissance) VALUES (@prenom, @nom, @courriel, @sexe, @dateNaissance); SELECT SCOPE_IDENTITY();";
                        SqlCommand commande = new SqlCommand(requete, sqlConnection);
                        // Définition des paramètres de la requête
                        commande.Parameters.AddWithValue("@prenom", membre.Prenom);
                        commande.Parameters.AddWithValue("@nom", membre.Nom);
                        commande.Parameters.AddWithValue("@courriel", membre.Courriel);
                        commande.Parameters.AddWithValue("@sexe", membre.Sexe);
                        commande.Parameters.AddWithValue("@dateNaissance", membre.DateNaissance);
                        sqlConnection.Open();
                        dernierIdInsere = Convert.ToInt32(commande.ExecuteScalar());

                        // Insertion des données de connexion du nouveau membre dans la base de données
                        requete = "INSERT INTO connexions (membreid, courriel, motdepasse, role, statut) VALUES (@membreId, @courriel, @motdepasse, @role, @statut)";
                        commande = new SqlCommand(requete, sqlConnection);
                        commande.Parameters.AddWithValue("@membreId", dernierIdInsere);
                        commande.Parameters.AddWithValue("@courriel", membre.Courriel);
                        commande.Parameters.AddWithValue("@motdepasse", membre.MotDePasse);
                        commande.Parameters.AddWithValue("@role", "M");
                        commande.Parameters.AddWithValue("@statut", "A");
                        commande.ExecuteNonQuery();
                    }
                    HttpContext.Session.SetInt32("MembreId", dernierIdInsere);
                    return RedirectToAction("Confirmation");
                }
                catch (Exception exception)
                {
                    return exception.Message;
                }
            }
            return View(membre);
        }

        // Méthode pour mettre à jour les données de session relatives aux prêts en cours
        [ServiceFilter(typeof(MemberAuthorizationFilter))]
        private void SetSessionNbPret(SqlConnection connexion)
        {
            int prets = 0;
            int retard = 0;
            int? membreId = HttpContext.Session.GetInt32("MembreId");

            if (membreId.HasValue)
            {
                // Requête pour récupérer les informations sur les prêts en cours du membre
                string requete = "SELECT * FROM prets WHERE membreId = @membreId";
                using (var commande = new SqlCommand(requete, connexion))
                {
                    commande.Parameters.AddWithValue("@membreId", membreId.Value);

                    using (var lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            if (lecteur.GetBoolean("retourner"))
                            {
                                continue;
                            }
                            else if (DateTime.Now > lecteur.GetDateTime("dateRetour"))
                            {
                                retard++;
                            }
                            else
                            {
                                prets++;
                            }
                        }
                    }
                }
            }

            // Stockage des données de session
            HttpContext.Session.SetInt32("Prets", prets);
            HttpContext.Session.SetInt32("Retards", retard);
        }

        // Méthode pour afficher la liste des prêts de l'utilisateur connecté
        [ServiceFilter(typeof(MemberAuthorizationFilter))]
        public IActionResult ListePret()
        {
            List<Pret> listePrets = new List<Pret>();
            using (SqlConnection connexion = new SqlConnection(_chaineConnexion))
            {
                int? membreId = HttpContext.Session.GetInt32("MembreId");
                string requete = "SELECT * FROM prets WHERE membreId = @membreId";
                var commande = new SqlCommand(requete, connexion);
                commande.Parameters.AddWithValue("@membreId", membreId);
                connexion.Open();
                SqlDataReader lecteur = commande.ExecuteReader();
                while (lecteur.Read())
                {
                    listePrets.Add(new Pret
                    {
                        Id = lecteur.GetInt32("id"),
                        LivreId = lecteur.GetInt32("livreId"),
                        TitreLivre = lecteur.GetString("titreLivre"),
                        DateLocation = lecteur.GetDateTime("dateLocation"),
                        DateRetour = lecteur.GetDateTime("dateRetour"),
                        Retourner = lecteur.GetBoolean("retourner"),
                    });
                }
            }
            return View(listePrets);
        }

        // Méthode pour afficher les détails d'un livre et vérifier s'il est déjà emprunté par l'utilisateur
        [ServiceFilter(typeof(MemberAuthorizationFilter))]
        public IActionResult Details(int id)
        {
            Livre? livre = LivreHelper.GetLivreById(_chaineConnexion ?? "", id);
            int? membreId = HttpContext.Session.GetInt32("MembreId");
            bool emprunter = false;
            int pretId = 0;
            try
            {
                using (SqlConnection connexion = new SqlConnection(_chaineConnexion))
                {

                    string requete = "SELECT retourner, id FROM prets WHERE membreId = @membreId AND livreId = @livreId";
                    SqlCommand commande = new SqlCommand(requete, connexion);

                    commande.Parameters.AddWithValue("@membreId", membreId);
                    commande.Parameters.AddWithValue("@livreId", id);
                    connexion.Open();
                    SqlDataReader lecteur = commande.ExecuteReader();

                    while (lecteur.Read())
                    {
                        if (!lecteur.GetBoolean("retourner"))
                        {
                            emprunter = true;
                            pretId = lecteur.GetInt32("id");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                return Content("ERREUR : " + exception.Message);
            }
            ViewBag.Emprunter = emprunter;
            ViewBag.PretId = pretId;
            ViewBag.NbPrets = HttpContext.Session.GetInt32("Prets");
            return View(livre);
        }

        // Méthode pour traiter la demande de prêt d'un livre par l'utilisateur connecté
        [HttpPost]
        [ServiceFilter(typeof(MemberAuthorizationFilter))]
        public IActionResult Pret(int id)
        {
            if (HttpContext.Session.GetInt32("Prets") >= 3)
            {
                return RedirectToAction("Details", "Membre", new { id = id, errorMessage = "Vous avez atteint la limite de prêts par membre, retournez vos prêts ou contactez l'administrateur." });
            }
            try
            {
                Livre? livre = LivreHelper.GetLivreById(_chaineConnexion ?? "", id);
                int? membreId = HttpContext.Session.GetInt32("MembreId");

                if (membreId == null || livre == null)
                {
                    return RedirectToAction("Details", "Membre", new { id = id, errorMessage = "Une erreur s'est produite lors du traitement de votre demande." });
                }

                using (SqlConnection connection = new SqlConnection(_chaineConnexion))
                {
                    // Insertion des informations sur le prêt dans la base de données
                    string requete = "INSERT INTO prets (livreId, membreId, titreLivre, retourner, dateLocation, dateRetour) VALUES (@livreId, @membreId, @titreLivre, @retourner, @dateLocation, @dateRetour)";
                    SqlCommand commande = new SqlCommand(requete, connection);
                    commande.Parameters.AddWithValue("@livreId", livre.Id);
                    commande.Parameters.AddWithValue("@membreId", membreId);
                    commande.Parameters.AddWithValue("@titreLivre", livre.Titre);
                    commande.Parameters.AddWithValue("@retourner", false);
                    commande.Parameters.AddWithValue("@dateLocation", DateTime.Now);
                    commande.Parameters.AddWithValue("@dateRetour", DateTime.Now.AddDays(7));
                    connection.Open();
                    commande.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                return RedirectToAction("Details", "Membre", new { id = id, errorMessage = "Une erreur s'est produite lors du traitement de votre demande." + exception.Message });
            }
        }

        // Méthode pour traiter le retour d'un livre par l'utilisateur connecté
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(MemberAuthorizationFilter))]
        public IActionResult Retourner(int id)
        {
            try
            {
                using (SqlConnection connexion = new SqlConnection(_chaineConnexion))
                {
                    // Mise à jour du statut du prêt dans la base de données pour indiquer qu'il a été retourné
                    string requete = "UPDATE prets SET retourner = @retourner WHERE id = @id";
                    SqlCommand commande = new SqlCommand(requete, connexion);
                    commande.Parameters.AddWithValue("@id", id);
                    commande.Parameters.AddWithValue("@retourner", true);
                    connexion.Open();
                    commande.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                return View();
            }
        }
    }
}
