using Microsoft.EntityFrameworkCore;

namespace BookRat.Models
{
    public class BdBiblioContext : DbContext
    {
        public BdBiblioContext() { }
        public BdBiblioContext(DbContextOptions<BdBiblioContext> options) :base(options) { }

        public virtual DbSet<Categorie> Categories { get; set; }
        public virtual DbSet<Livre> Livres { get; set; }
        public virtual DbSet<Membre> Membres { get; set; }
        public virtual DbSet<Pret> Prets { get; set; }
        public virtual DbSet<Connexion> Connexions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categorie>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Categories");
                entity.Property(e => e.Titre).HasMaxLength(100);
            });
            modelBuilder.Entity<Livre>(entity =>
            {
                entity.ToTable("Livres");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titre).HasMaxLength(100);
                entity.Property(e => e.Auteur).HasMaxLength(100);
                entity.Property(e => e.NbPages);
                entity.Property(e => e.CategorieId).IsRequired();
                entity.HasOne(e => e.Categorie)
                       .WithMany()
                       .HasForeignKey(e => e.CategorieId);
            });
            modelBuilder.Entity<Membre>(entity =>
            {
                entity.ToTable("Membres");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nom).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Prenom).HasMaxLength(50);
                entity.Property(e => e.Courriel).HasMaxLength(100);
                entity.Property(e => e.Sexe).HasMaxLength(10);
            });

            modelBuilder.Entity<Pret>(entity =>
            {
                entity.ToTable("Prets");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MembreId).IsRequired();
                entity.Property(e => e.LivreId).IsRequired();
                entity.Property(e => e.Retourner).HasColumnType("bit");
                entity.Property(e => e.DateLocation).HasColumnType("date");
                entity.Property(e => e.DateRetour).HasColumnType("date");
                entity.HasOne(e => e.Membre)
                    .WithMany()
                    .HasForeignKey(e => e.MembreId);

                entity.HasOne(e => e.Livre)
                    .WithMany()
                    .HasForeignKey(e => e.LivreId);
            });
            modelBuilder.Entity<Connexion>(entity =>
            {
                entity.ToTable("Connexions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MembreId).IsRequired();
                entity.Property(e => e.Statut).HasMaxLength(20);
                entity.Property(e => e.Role).HasMaxLength(20);

                entity.HasOne(e => e.Membre)
                    .WithMany()
                    .HasForeignKey(e => e.MembreId);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
