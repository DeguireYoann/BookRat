using Microsoft.AspNetCore.Mvc;
using BookRat.Models;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace BookRat.Controllers
{
    public class MembreController : Controller
    {
        private readonly BdBiblioContext _context;

        // Constructeur de la classe
        public MembreController(BdBiblioContext context)
        {
            _context = context;
        }

        // Méthode pour afficher la liste des livres disponibles
        public async Task<IActionResult> Index()
        {
            try
            {
                var listeLivre = await _context.Livres
                    .Include(l => l.Categorie)
                    .Select(l => new Livre
                    {
                        Id = l.Id,
                        Titre = l.Titre,
                        Categorie = l.Categorie,
                        Auteur = l.Auteur,
                        NbPages = l.NbPages
                    })
                    .ToListAsync();

                await SetSessionNbPretAsync();

                return View(listeLivre);
            }
            catch (Exception exception)
            {
                return Content("ERREUR : " + exception.Message);
            }
        }

        // Méthode pour afficher la page de confirmation
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
        public ActionResult<string> AjouterMembre(MembreViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Membre membre = new Membre
                    {
                        Nom = model.Nom,
                        Prenom = model.Prenom,
                        Courriel = model.Courriel,
                        Sexe = model.Sexe,
                    };
                    // Ajout du nouveau membre dans la base de données
                    _context.Membres.Add(membre);
                    _context.SaveChanges();

                    // Création d'une connexion pour le nouveau membre
                    Connexion connexion = new Connexion
                    {
                        MembreId = membre.Id,
                        Courriel = membre.Courriel,
                        MotDePasse = model.MotDePasse,
                        Role = "M",
                        Statut = "A",
                        Membre = membre
                    };

                    // Ajout de la connexion dans la base de données
                    _context.Connexions.Add(connexion);
                    _context.SaveChanges();

                    HttpContext.Session.SetInt32("MembreId", membre.Id);
                    return RedirectToAction("Confirmation");
                }
                catch (Exception exception)
                {
                    return Content("ERREUR : " + exception.Message);
                }
            }
            return View(model);
        }

        // Méthode pour mettre à jour les données de session relatives aux prêts en cours
        private async Task SetSessionNbPretAsync()
        {
            int prets = 0;
            int retard = 0;
            int? membreId = HttpContext.Session.GetInt32("MembreId");

            if (membreId.HasValue)
            {
                var pretsMembre = await _context.Prets
                    .Where(p => p.MembreId == membreId)
                    .ToListAsync();

                foreach (var pret in pretsMembre)
                {
                    if (!pret.Retourner)
                    {
                        if (DateTime.Now > pret.DateRetour)
                        {
                            retard++;
                        }
                        prets++;
                    }
                }
            }

            // Stockage des données de session
            HttpContext.Session.SetInt32("Prets", prets);
            HttpContext.Session.SetInt32("Retards", retard);
        }

        // Méthode pour afficher la liste des prêts de l'utilisateur connecté
        public IActionResult ListePret()
        {
            int? membreId = HttpContext.Session.GetInt32("MembreId");

            if (membreId == null)
            {
                return RedirectToAction("Index");
            }

            List<Pret> listePrets = _context.Prets
                .Include(p => p.Livre)
                .Where(p => p.MembreId == membreId)
                .ToList();

            return View(listePrets);
        }

        // Méthode pour afficher les détails d'un livre et vérifier s'il est déjà emprunté par l'utilisateur
        public IActionResult Details(int id)
        {
            Livre livre = _context.Livres
                .Include(l => l.Categorie)
                .FirstOrDefault(l => l.Id == id);

            int? membreId = HttpContext.Session.GetInt32("MembreId");

            if (membreId == null)
            {
                return RedirectToAction("Index");
            }

            Pret pret = _context.Prets
                .FirstOrDefault(p => p.MembreId == membreId && p.LivreId == id && !p.Retourner);

            ViewBag.Emprunter = pret != null;
            ViewBag.PretId = pret?.Id;
            ViewBag.NbPrets = HttpContext.Session.GetInt32("Prets");

            return View(livre);
        }

        // Méthode pour traiter la demande de prêt d'un livre par l'utilisateur connecté
        [HttpPost]
        public IActionResult Pret(int id)
        {
            int? membreId = HttpContext.Session.GetInt32("MembreId");

            if (membreId == null)
            {
                return RedirectToAction("Details", new { id = id });
            }

            if (_context.Prets.Count(p => p.MembreId == membreId && !p.Retourner) >= 3)
            {
                return RedirectToAction("Details", new { id = id, errorMessage = "Vous avez atteint la limite de prêts par membre, retournez vos prêts ou contactez l'administrateur." });
            }

            Livre livre = _context.Livres.FirstOrDefault(l => l.Id == id);
            Membre membre = _context.Membres.FirstOrDefault(m => m.Id == membreId);

            if (livre == null || membre == null)
            {
                return RedirectToAction("Details", new { id = id, errorMessage = "Une erreur s'est produite lors du traitement de votre demande." });
            }

            Pret pret = new Pret
            {
                LivreId = livre.Id,
                MembreId = membre.Id,
                Retourner = false,
                DateLocation = DateTime.Now,
                DateRetour = DateTime.Now.AddDays(7),
                Membre = membre,
                Livre = livre
            };

            _context.Prets.Add(pret);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Méthode pour traiter le retour d'un livre par l'utilisateur connecté
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Retourner(int id)
        {
            Pret pret = _context.Prets.FirstOrDefault(p => p.Id == id);

            if (pret == null)
            {
                return RedirectToAction("Index");
            }

            pret.Retourner = true;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
