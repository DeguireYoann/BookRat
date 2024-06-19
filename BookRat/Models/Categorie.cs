using System.ComponentModel;

namespace BookRat.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        [DisplayName("Catégorie")]
        public string Titre { get; set; }
    }
}
