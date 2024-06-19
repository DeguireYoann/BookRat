using System.ComponentModel.DataAnnotations;

namespace BookRat.Models
{
    public class LivreViewModel
    {
        [Required(ErrorMessage = "Le Titre est obligatoire.")]
        public required string Titre { get; set; }
        [Required(ErrorMessage = "L'auteur est obligatoire.")]
        public required string Auteur { get; set; }
        public int? NbPages { get; set; }
        [Required(ErrorMessage = "Veuillez selectionner une catégorie")]
        public int CategorieId { get; set; }
        public Categorie? Categorie { get; set; }
    }
}
