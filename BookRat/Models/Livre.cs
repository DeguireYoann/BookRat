namespace BookRat.Models
{
    public class Livre
    {
        public int Id { get; set; }
        public required string Titre { get; set; }
        public string? Auteur { get; set; }
        public int? NbPages { get; set; }
        public int CategorieId { get; set; }
        public Categorie? Categorie { get; set; }
    }
}
