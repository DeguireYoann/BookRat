namespace BookRat.Models
{
    public class Connexion
    {

        public int Id { get; set; }
        public int MembreId { get; set; }
        public required string Courriel { get; set; }
        public required string MotDePasse { get; set; }
        public required string Statut { get; set; }
        public required string Role { get; set; }
        public required Membre Membre { get; set; }
    }
}
