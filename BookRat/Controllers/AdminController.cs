using Microsoft.AspNetCore.Mvc;
using System.Data;
using BookRat.Models;
using BookRat.Filters;
using Microsoft.EntityFrameworkCore;

namespace BookRat.Controllers
{
    public class AdminController : Controller
    {
        private readonly BdBiblioContext _context;

        public AdminController(BdBiblioContext context)
        {
            _context = context;
        }

        // Méthode pour afficher la liste des livres (requiert une autorisation de rôle)
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        public async Task<IActionResult> Index()
        {
            var listeLivre = await _context.Livres
                .Select(l => new Livre
                {
                    Id = l.Id,
                    Titre = l.Titre,
                    Categorie = l.Categorie,
                    Auteur = l.Auteur,
                    NbPages = l.NbPages
                })
                .ToListAsync();

            return View(listeLivre);
        }

        // Méthode pour afficher les détails d'un livre (requiert une autorisation de rôle)
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livre = await _context.Livres
                .Include(l => l.Categorie)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // Méthode pour créer un livre (requiert une autorisation de rôle)
        [HttpPost]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        public async Task<IActionResult> Create(LivreViewModel livre)
        {
            if (ModelState.IsValid)
            {
                _context.Livres.Add(new Livre {
                    Titre = livre.Titre,
                    Auteur = livre.Auteur,
                    NbPages = livre.NbPages,
                    CategorieId = livre.CategorieId
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // Méthode pour afficher le formulaire de modification d'un livre (requiert une autorisation de rôle)
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livre = await _context.Livres.FindAsync(id);
            if (livre == null)
            {
                return NotFound();
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(livre);
        }

        // Méthode pour modifier un livre (requiert une autorisation de rôle)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        public async Task<IActionResult> Edit(int id, Livre livre)
        {
            if (id != livre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Livres.Update(livre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivreExists(livre.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(livre);
        }

        // Méthode pour afficher le formulaire de suppression d'un livre (requiert une autorisation de rôle)
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livre = await _context.Livres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livre == null)
            {
                return NotFound();
            }

            return View(livre);
        }

        // Méthode pour gérer la suppression d'un livre (requiert une autorisation de rôle)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livre = await _context.Livres.FindAsync(id);
            _context.Livres.Remove(livre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LivreExists(int id)
        {
            return _context.Livres.Any(e => e.Id == id);
        }
    }
}

