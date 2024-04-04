-- Création de la table Livre
CREATE TABLE Livres (
    Id INT PRIMARY KEY IDENTITY,
    Titre VARCHAR(100),
    Categorie INT,
    Auteur VARCHAR(100),
    NbPages INT
);

INSERT INTO Livres (Titre, Categorie, Auteur, NbPages) VALUES
('Le Seigneur des Anneaux', 1, 'J.R.R. Tolkien', 1178),
('Harry Potter à l''école des sorciers', 2, 'J.K. Rowling', 320),
('1984', 3, 'George Orwell', 328),
('Le Petit Prince', 4, 'Antoine de Saint-Exupéry', 96),
('Orgueil et Préjugés', 5, 'Jane Austen', 279),
('Le Nom de la Rose', 6, 'Umberto Eco', 592),
('Les Misérables', 7, 'Victor Hugo', 1905),
('La Guerre et la Paix', 8, 'Léon Tolstoï', 1225),
('Fondation', 9, 'Isaac Asimov', 239),
('Le Vieil Homme et la Mer', 10, 'Ernest Hemingway', 127),
('Crime et Châtiment', 11, 'Fiodor Dostoïevski', 671),
('Les Raisins de la colère', 12, 'John Steinbeck', 464),
('L''Odyssée', 13, 'Homère', 374),
('Moby Dick', 14, 'Herman Melville', 625),
('Le Comte de Monte-Cristo', 15, 'Alexandre Dumas', 1276),
('Les Trois Mousquetaires', 16, 'Alexandre Dumas', 686),
('La Métamorphose', 17, 'Franz Kafka', 89),
('Vingt mille lieues sous les mers', 18, 'Jules Verne', 579),
('Anna Karénine', 19, 'Léon Tolstoï', 864),
('Guerre et Paix', 20, 'Léon Tolstoï', 1392);



-- Création de la table Membre
CREATE TABLE Membres (
    Id INT PRIMARY KEY IDENTITY,
    Nom VARCHAR(50),
    Prenom VARCHAR(50),
    Courriel VARCHAR(100),
    Sexe VARCHAR(10),
    DateNaissance DATE,
);

-- Insertion de 20 membres pour les tests
INSERT INTO Membres (Nom, Prenom, Courriel, Sexe, DateNaissance, MotDePasse) VALUES
('Dupont', 'Jean', 'jean.dupont@example.com', 'M', '1990-05-15'),
('Tremblay', 'Marie', 'marie.tremblay@example.com', 'F', '1988-09-21'),
('Smith', 'John', 'john.smith@example.com', 'M', '1985-03-10'),
('Dubois', 'Sophie', 'sophie.dubois@example.com', 'F', '1995-07-02'),
('Garcia', 'Carlos', 'carlos.garcia@example.com', 'M', '1982-11-30'),
('Martinez', 'Ana', 'ana.martinez@example.com', 'F', '1993-04-18'),
('Lopez', 'Pedro', 'pedro.lopez@example.com', 'M', '1987-08-25'),
('Nguyen', 'Linh', 'linh.nguyen@example.com', 'F', '1991-12-12'),
('Kim', 'Minho', 'minho.kim@example.com', 'M', '1984-06-05'),
('Chen', 'Wei', 'wei.chen@example.com', 'M', '1997-02-28'),
('Müller', 'Hans', 'hans.muller@example.com', 'M', '1986-09-14'),
('Schmidt', 'Anna', 'anna.schmidt@example.com', 'F', '1990-01-07'),
('González', 'Marta', 'marta.gonzalez@example.com', 'F', '1983-05-20'),
('Hernández', 'Javier', 'javier.hernandez@example.com', 'M', '1992-03-03'),
('Ruiz', 'Laura', 'laura.ruiz@example.com', 'F', '1989-07-16'),
('Gomez', 'David', 'david.gomez@example.com', 'M', '1981-10-11'),
('Pérez', 'Sofía', 'sofia.perez@example.com', 'F', '1994-12-24'),
('Wilson', 'Michael', 'michael.wilson@example.com', 'M', '1980-02-09'),
('Jones', 'Emma', 'emma.jones@example.com', 'F', '1996-08-07'),
('Brown', 'Daniel', 'daniel.brown@example.com', 'M', '1988-04-02');

-- Création de la table Pret
CREATE TABLE Prets (
    Id INT PRIMARY KEY IDENTITY,
    MembreId INT,
    LivreId INT,
    TitreLivre VARCHAR(100),
    Retourner BIT,
    DateLocation DATE,
    DateRetour DATE
);

-- Insertion de données de prêts
INSERT INTO Prets (MembreId, LivreId, TitreLivre, Retourner, DateLocation, DateRetour) VALUES
(1, 2, 'Harry Potter à l''école des sorciers', 0, '2023-05-10', NULL),
(3, 5, 'Orgueil et Préjugés', 1, '2023-06-15', '2023-07-05'),
(5, 9, 'Fondation', 1, '2023-07-20', '2023-08-15'),
(2, 14, 'Moby Dick', 0, '2023-08-01', NULL),
(4, 8, 'La Guerre et la Paix', 0, '2023-09-05', NULL),
(6, 12, 'Les Raisins de la colère', 1, '2023-10-10', '2023-11-01'),
(7, 17, 'La Métamorphose', 1, '2023-11-15', '2023-12-10'),
(8, 6, 'Le Nom de la Rose', 0, '2023-12-20', NULL),
(10, 1, 'Le Seigneur des Anneaux', 1, '2024-01-05', '2024-02-01'),
(9, 15, 'Le Comte de Monte-Cristo', 0, '2024-02-15', NULL),
(12, 3, '1984', 0, '2024-03-01', NULL),
(11, 11, 'Crime et Châtiment', 1, '2024-03-15', '2024-04-10');

CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY,
    Titre NVARCHAR(100) NOT NULL
);

INSERT INTO Categories (Titre) VALUES
('Fiction'),
('Non-fiction'),
('Science-fiction'),
('Fantastique'),
('Policier / Mystère'),
('Romance'),
('Thriller'),
('Horreur'),
('Biographie / Autobiographie'),
('Histoire'),
('Philosophie'),
('Psychologie'),
('Développement personnel'),
('Sciences'),
('Technologie'),
('Art'),
('Musique'),
('Cuisine / Gastronomie'),
('Voyage'),
('Religion / Spiritualité');

