namespace BookRat.Models
{
    public class Membre
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Courriel { get; set; }
        public string Sexe { get; set; }
        public DateTime DateNaissance { get; set; }
        public string MotDePasse { get; set; }
    }
}
