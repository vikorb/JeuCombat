# JeuCombat

Jeu de combat tour par tour en console, développé en C# avec .NET 8.

Ce projet est réalisé dans le cadre d'un TP visant à appliquer les principes de clean code, SOLID, l'architecture en couches et plusieurs design patterns.

## Objectif du projet

Le joueur incarne un héros qui affronte plusieurs vagues d'ennemis dans un combat tour par tour.

À chaque tour, le joueur choisit une action dans un menu, puis les ennemis encore vivants jouent leur tour.

## Fonctionnalités attendues

- Saisie du nom du héros.
- Choix d'une classe de héros :
  - Guerrier
  - Mage
  - Voleur
- Combat contre 3 vagues d'ennemis :
  - Vague 1 : 1 ennemi faible
  - Vague 2 : 2 ennemis
  - Vague 3 : 1 boss
- Actions possibles :
  - Attaque de base
  - Compétence spéciale de classe
  - Soin
  - Affichage du journal de combat
- Gestion des points de vie.
- Gestion des cooldowns.
- Journal des événements de combat.
- Conditions de victoire et de défaite.

## Stack technique

- C#
- .NET 8
- Application console
- xUnit pour les tests unitaires

## Prérequis

Vérifier que le SDK .NET est installé :

```bash
dotnet --version
```

Le projet cible actuellement .NET 8.

## Installation

Cloner le projet :

```bash
git clone <url-du-repo>
cd JeuCombat
```

Restaurer les dépendances :

```bash
dotnet restore
```

Compiler le projet :

```bash
dotnet build
```

Lancer les tests :

```bash
dotnet test
```

Lancer l'application :

```bash
dotnet run --project src/JeuCombat.Cli/JeuCombat.Cli.csproj
```

## Commandes utiles

Formater le code :

```bash
dotnet format
```

Vérifier le formatage sans modifier les fichiers :

```bash
dotnet format --verify-no-changes
```

Compiler toute la solution :

```bash
dotnet build
```

Lancer tous les tests :

```bash
dotnet test
```

## Structure du projet

```txt
JeuCombat/
├── src/
│   ├── JeuCombat.Domain/
│   ├── JeuCombat.Application/
│   ├── JeuCombat.Infrastructure/
│   └── JeuCombat.Cli/
├── tests/
│   └── JeuCombat.Tests/
├── docs/
├── .editorconfig
├── README.md
└── JeuCombat.sln
```

## Rôle des projets

### JeuCombat.Domain

Contient le coeur métier du jeu.

Il ne dépend d'aucun autre projet.

Exemples de responsabilités :

- Personnages
- Héros
- Ennemis
- Points de vie
- Dégâts
- Règles métier simples
- Constantes de combat

### JeuCombat.Application

Contient la logique applicative.

Exemples de responsabilités :

- Session de combat
- Actions de combat
- États du combat
- Commandes du joueur
- Factories
- Gestion des événements de combat
- Intelligence artificielle simple des ennemis

### JeuCombat.Infrastructure

Contient les détails techniques.

Exemples de responsabilités :

- Affichage console
- Lecture des choix utilisateur
- Persistance JSON éventuelle
- Implémentations techniques

### JeuCombat.Cli

Point d'entrée de l'application.

Ce projet assemble les dépendances et lance le jeu.

Le fichier `Program.cs` doit rester léger.

### JeuCombat.Tests

Contient les tests unitaires du projet.

Les tests doivent cibler principalement la logique métier et applicative, sans dépendre de la console.

## Règles d'architecture

Le projet suit une architecture simple en couches.

Les dépendances doivent aller dans ce sens :

```txt
Cli -> Infrastructure -> Application -> Domain
Cli -> Application -> Domain
Cli -> Domain
Tests -> Application -> Domain
```

Règles importantes :

- `Domain` ne référence aucun autre projet.
- `Application` référence `Domain`.
- `Infrastructure` peut référencer `Application` et `Domain`.
- `Cli` assemble le tout.
- La logique métier ne doit pas être écrite dans `Program.cs`.
- La console ne doit pas calculer les dégâts directement.
- La création des personnages doit être centralisée.
- Les actions doivent être extensibles sans gros `switch`.

## Design patterns prévus

| Pattern | Rôle dans le projet | Classes prévues |
| --- | --- | --- |
| Strategy | Représenter les actions de combat comme des algorithmes interchangeables | `ICombatAction`, `AttaqueBasiqueAction`, `SoinAction`, `CompetenceGuerrierAction`, `CompetenceMageAction`, `CompetenceVoleurAction` |
| State | Gérer les différentes phases du combat | `ICombatState`, `TourJoueurState`, `TourEnnemiState`, `EntreVaguesState`, `VictoireState`, `DefaiteState` |
| Factory | Centraliser la création des héros, ennemis et vagues | `HeroFactory`, `EnemyFactory`, `WaveFactory` |
| Command | Séparer la saisie utilisateur de l'exécution métier | `ICommand`, `AttaquerCommand`, `UtiliserCompetenceCommand`, `SoignerCommand`, `AfficherJournalCommand` |
| Observer | Journaliser et diffuser les événements de combat | `ICombatObserver`, `CombatEventPublisher`, `JournalCombatObserver`, `ConsoleCombatObserver` |

## Pattern bonus prévu

Le pattern bonus prévu est `Repository`.

Il pourra servir à sauvegarder des scores ou un historique de partie dans un fichier JSON.

Classes prévues :

- `IScoreRepository`
- `JsonScoreRepository`
- `Score`

Ce pattern reste optionnel dans un premier temps. Il sera ajouté seulement après le fonctionnement principal du jeu.

## Règles du jeu

### Classes de héros

| Classe | Points de vie | Attaque de base | Compétence spéciale |
| --- | ---: | ---: | --- |
| Guerrier | 120 | 18 | Frappe lourde : dégâts x1,5 avec cooldown de 2 tours |
| Mage | 80 | 12 | Éclair : dégâts magiques fixes et ignore une partie de l'armure ennemie avec cooldown de 3 tours |
| Voleur | 90 | 14 | Coup critique : chance de doubler les dégâts avec cooldown de 2 tours |

### Actions du joueur

À chaque tour, le joueur peut choisir :

```txt
1. Attaque de base
2. Compétence de classe
3. Se soigner
4. Afficher le journal du combat
```

### Soin

Le soin rend 25 points de vie.

Il est limité à 2 utilisations maximum par combat.

Le soin ne peut pas dépasser les points de vie maximum du héros.

### Conditions de fin

| État | Condition |
| --- | --- |
| Victoire | Tous les ennemis de la vague 3 sont vaincus |
| Défaite | Les points de vie du héros sont inférieurs ou égaux à 0 |
| Entre vagues | Le héros survit et récupère une partie de ses points de vie avant la vague suivante |

## Qualité de code

Le projet doit respecter les principes suivants :

- Noms explicites.
- Fonctions courtes.
- Classes avec une responsabilité claire.
- Pas de logique métier dans la console.
- Pas de gros `switch` central pour toute la logique.
- Pas de nombres magiques dans le code.
- Utilisation de constantes nommées.
- Tests unitaires sur la logique importante.
- Gestion propre des entrées invalides.

## Configuration qualité

Le fichier `.editorconfig` définit les règles de formatage principales.

Les warnings sont traités comme des erreurs avec :

```xml
<TreatWarningsAsErrors>true</TreatWarningsAsErrors>
```

Le hook Git `pre-commit` exécute :

```bash
dotnet format --verify-no-changes
dotnet build
dotnet test
```

## Tests prévus

Les premiers tests unitaires viseront les règles suivantes :

- Un personnage ne peut pas avoir plus de points de vie que son maximum.
- Un soin rend des points de vie sans dépasser le maximum.
- Une attaque réduit correctement les points de vie d'une cible.
- Une compétence en cooldown ne peut pas être exécutée.
- Un personnage est considéré comme vaincu quand ses points de vie sont à 0.

## État actuel du projet

- [x] Création de la solution .NET
- [x] Création des projets
- [x] Ajout des références entre projets
- [x] Mise en place du formatage
- [x] Mise en place des hooks Git
- [x] Documentation initiale
- [ ] Création du domaine métier
- [ ] Création des premières entités
- [ ] Création des premières actions de combat
- [ ] Création de la boucle de combat
- [ ] Ajout des tests unitaires
- [ ] Implémentation complète des design patterns
- [ ] Finalisation du README avec le tableau patterns vers classes réelles

## Convention de commit

Format recommandé :

```txt
type(scope): message
```

Exemples :

```txt
chore(project): initialize dotnet solution
docs(readme): add project documentation
feat(domain): add character entity
feat(combat): add basic attack action
test(domain): add health points tests
refactor(combat): split player and enemy turns
```

## Roadmap de développement

### Étape 1 — Mise en place

- Solution .NET
- Projets
- Références
- `.editorconfig`
- Hook Git
- README

### Étape 2 — Domaine métier

- Classe `Personnage`
- Classe `Heros`
- Classe `Ennemi`
- Enum `ClasseHero`
- Enum `TypeEnnemi`
- Constantes de combat

### Étape 3 — Tests du domaine

- Tests sur les points de vie
- Tests sur le soin
- Tests sur les dégâts
- Tests sur la mort d'un personnage

### Étape 4 — Actions de combat

- Interface `ICombatAction`
- Attaque de base
- Soin
- Compétences des classes

### Étape 5 — Factories

- Création des héros
- Création des ennemis
- Création des vagues

### Étape 6 — Boucle de combat

- Session de combat
- Tour joueur
- Tour ennemi
- Passage entre vagues
- Victoire
- Défaite

### Étape 7 — Console

- Menus
- Saisie utilisateur
- Affichage de l'état du combat
- Affichage du journal

### Étape 8 — Finalisation

- Nettoyage du code
- Tests complémentaires
- Documentation des choix d'architecture
- Schéma ou diagramme
- Démonstration finale

## Auteur

Projet réalisé par Victoria Oruba.
