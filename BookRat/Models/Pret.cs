namespace BookRat.Models
{
    public class Pret
    {
        public int Id { get; set; }
        public int MembreId { get; set; }
        public int LivreId { get; set; }
        public string TitreLivre { get; set; }
        public bool Retourner { get; set; }
        public DateTime DateLocation { get; set; }
        public DateTime DateRetour { get; set; }
    }
}
