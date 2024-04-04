namespace BookRat.Models
{
    public class Livre
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Categorie { get; set; }
        public string? Auteur { get; set; }
        public int? NbPages { get; set; }
    }
}
