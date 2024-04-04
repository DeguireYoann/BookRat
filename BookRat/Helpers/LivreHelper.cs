using BookRat.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System;

namespace BookRat.Helpers
{
    public static class LivreHelper
    {


        public static Livre? GetLivreById(string chaineConnexion, int id)
        {
            Livre? livre = null;
            using (SqlConnection connexion = new SqlConnection(chaineConnexion))
            {
                string requete = "SELECT l.id, l.titre, l.categorie, l.auteur, l.nbpages, c.titre AS categorie_titre " +
                                 "FROM livres l " +
                                 "JOIN categories c ON l.categorie = c.id " +
                                 "WHERE l.id = @Id";
                var commande = new SqlCommand(requete, connexion);
                commande.Parameters.AddWithValue("@Id", id);

                connexion.Open();

                using (var lecteur = commande.ExecuteReader())
                {
                    if (lecteur.Read())
                    {
                        livre = new Livre
                        {
                            Id = lecteur.GetInt32("id"),
                            Titre = lecteur.GetString("titre"),
                            Categorie = lecteur.GetString("categorie_titre"),
                            Auteur = lecteur.GetString("auteur"),
                            NbPages = lecteur.GetInt32("nbpages"),
                        };
                    }
                }
            }

            return livre;
        }

        public static List<Categorie> GetAllCategories(string chaineConnexion)
        {
            List<Categorie> categories = new List<Categorie>();
            using (SqlConnection connexion = new SqlConnection(chaineConnexion))
            {
                string requete = "SELECT * FROM categories";
                var commande = new SqlCommand(requete, connexion);
                connexion.Open();

                using (var lecteur = commande.ExecuteReader())
                {
                    while (lecteur.Read())
                    {
                        categories.Add(new Categorie
                        {
                            Id = lecteur.GetInt32("id"),
                            Titre = lecteur.GetString("titre"),
                        });
                    }
                }
            }

            return categories;
        }
    }
}
