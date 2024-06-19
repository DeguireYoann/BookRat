namespace BookRat.Models
{
    public class Membre
    {
        public int Id { get; set; }
        public required string Nom { get; set; }
        public required string Prenom { get; set; }
        public required string Courriel { get; set; }
        public string? Sexe { get; set; }
    }
}
