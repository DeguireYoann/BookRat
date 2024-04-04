# BookRat

BookRat est une application Web développée en ASP.NET Core qui sert de système de gestion pour une bibliothèque. Les utilisateurs peuvent créer un compte et emprunter des livres. Les administrateurs ont accès à un système CRUD pour gérer l'inventaire des livres.

## Objectifs du projet

Ce projet a été réalisé dans le cadre du cours "Programmation serveur Web avec ASP" (IFT 1148) de la Direction de l'Enseignement de Service en Informatique (DESI). Les objectifs principaux du projet sont les suivants :

- Travailler avec .NET Framework MVC en utilisant plusieurs tables.
- Créer des tables liées par clé primaire et clé étrangère.
- Mettre en place un système de gestion de prêt de livres pour un centre communautaire.
- Utiliser Bootstrap pour l'interface utilisateur.

## Fonctionnalités principales

- Les utilisateurs peuvent créer un compte et se connecter.
- Les membres peuvent emprunter des livres.
- Les administrateurs ont accès à un système CRUD pour gérer l'inventaire des livres.
- Gestion des prêts avec une durée d'une semaine et un maximum de trois livres par emprunt.
- Suivi des dates de prêt et de retour des livres.
- Gestion des retards de retour.
- Catégorisation des livres (ex. enfants, jeunesse, etc.).

## Aspects techniques

- Utilisation de Bootstrap pour le design de l'interface utilisateur.
- Modèle MVC utilisé pour une architecture bien structurée.
- Utilisation de requêtes sans Entity Framework pour la gestion des données.
- Personnalisation de l'interface utilisateur pour une meilleure expérience utilisateur.
- Utilisation d'une base de données SQL pour stocker les données.

## Configuration de l'environnement de développement

1. **Installer le SDK .NET Core**: Téléchargez et installez le SDK .NET Core à partir du site officiel de .NET : [dotnet.microsoft.com](https://dotnet.microsoft.com/download).

2. **Cloner le projet**: Utilisez la commande `git clone` pour cloner le projet depuis GitHub vers votre machine locale.

3. **Configurer la base de données**: Assurez-vous d'avoir une instance SQL Server disponible. Mettez à jour la chaîne de connexion dans le fichier `appsettings.json` pour refléter les paramètres de votre base de données.

4. **Exécuter l'application**: Utilisez la commande `dotnet run` dans le répertoire racine de l'application pour lancer le serveur. Accédez à l'URL spécifiée dans votre navigateur Web pour accéder à l'application.

## Remarques supplémentaires

- Le projet utilise une base de données locale nommée "bdgplcc".
- Les différentes tables de la base de données sont liées par clé primaire et clé étrangère.
- Les administrateurs sont redirigés vers le contrôleur AdminController, tandis que les membres sont redirigés vers le contrôleur MembresController.
