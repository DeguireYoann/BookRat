using System.ComponentModel.DataAnnotations;

namespace BookRat.Models
{
    public class ConnexionViewModel
    {
        [Required(ErrorMessage = "L'adresse courriel est obligatoire.")]
        [EmailAddress(ErrorMessage = "Le format de l'adresse courriel est invalide.")]
        public required string Courriel { get; set; }

        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "Le mot de passe doit contenir entre 8 et 12 caractères.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[-_#$%!])[A-Za-z\d-_#$%!]{8,12}$", ErrorMessage = "Le mot de passe doit contenir au moins une majuscule, une minuscule, un chiffre et un caractère spécial (-, _, #, $, %, !).")]
        public required string MotDePasse { get; set; }
    }
}
