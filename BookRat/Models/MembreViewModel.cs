namespace BookRat.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public class MembreViewModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(20, ErrorMessage = "Le nom ne doit pas dépasser 20 caractères.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        [StringLength(20, ErrorMessage = "Le prénom ne doit pas dépasser 30 caractères.")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "L'adresse courriel est obligatoire.")]
        [EmailAddress(ErrorMessage = "Le format de l'adresse courriel est invalide.")]
        public string Courriel { get; set; }
        public string? Sexe { get; set; }

        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "Le mot de passe doit contenir entre 8 et 12 caractères.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[-_#$%!])[A-Za-z\d-_#$%!]{8,12}$", ErrorMessage = "Le mot de passe doit contenir au moins une majuscule, une minuscule, un chiffre et un caractère spécial (-, _, #, $, %, !).")]
        public string MotDePasse { get; set; }

        [Compare("MotDePasse", ErrorMessage = "La confirmation ne correspond pas au mot de passe.")]
        public string? ConfirmationMotDePasse { get; set; }

        public IFormFile? Photo { get; set; }
    }
}
