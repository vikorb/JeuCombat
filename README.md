# JeuCombat

Jeu de combat tour par tour en console, développé en C# avec .NET 8.

Ce projet est réalisé dans le cadre d'un TP dont l'objectif est d'appliquer le clean code, les principes SOLID, une architecture en couches et plusieurs design patterns.

## Sommaire

- [Objectif](#objectif)
- [Fonctionnalités](#fonctionnalités)
- [Stack technique](#stack-technique)
- [Installation](#installation)
- [Commandes utiles](#commandes-utiles)
- [Structure du projet](#structure-du-projet)
- [Architecture](#architecture)
- [Design patterns utilisés](#design-patterns-utilisés)
- [Règles du jeu](#règles-du-jeu)
- [Tests](#tests)
- [Qualité de code](#qualité-de-code)
- [État du projet](#état-du-projet)

## Objectif

Le joueur incarne un héros qui affronte plusieurs vagues d'ennemis dans un combat tour par tour.

À chaque tour, le joueur choisit une action dans un menu. Ensuite, les ennemis encore vivants jouent leur tour.

Le projet respecte les objectifs suivants :

- Séparer la logique métier de la console.
- Éviter de tout coder dans `Program.cs`.
- Appliquer plusieurs design patterns de manière concrète.
- Garder un code lisible, testable et maintenable.
- Documenter l'architecture et les choix techniques.

## Fonctionnalités

- Saisie du nom du héros.
- Choix d'une classe de héros :
  - Guerrier
  - Mage
  - Voleur
- Combat contre 3 vagues :
  - Vague 1 : 1 ennemi faible
  - Vague 2 : 2 ennemis
  - Vague 3 : 1 boss
- Actions du joueur :
  - Attaque de base
  - Compétence spéciale de classe
  - Soin
  - Affichage du journal de combat
- Gestion des points de vie.
- Gestion des cooldowns.
- Gestion des soins limités.
- Gestion de l'armure ennemie.
- Journal des événements de combat.
- IA ennemie simple.
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

Lancer le jeu :

```bash
dotnet run --project src/JeuCombat.Cli/JeuCombat.Cli.csproj
```

## Commandes utiles

Compiler toute la solution :

```bash
dotnet build
```

Lancer tous les tests :

```bash
dotnet test
```

Formater le code :

```bash
dotnet format
```

Vérifier le formatage sans modifier les fichiers :

```bash
dotnet format --verify-no-changes
```

Lancer le projet console :

```bash
dotnet run --project src/JeuCombat.Cli/JeuCombat.Cli.csproj
```

## Structure du projet

```txt
JeuCombat/
├── src/
│   ├── JeuCombat.Domain/
│   │   ├── Constants/
│   │   ├── Enums/
│   │   └── Entites/
│   │
│   ├── JeuCombat.Application/
│   │   ├── Combat/
│   │   │   ├── Actions/
│   │   │   ├── Ai/
│   │   │   ├── Chance/
│   │   │   ├── Commands/
│   │   │   ├── Events/
│   │   │   ├── Journal/
│   │   │   ├── Sessions/
│   │   │   └── States/
│   │   └── Factories/
│   │
│   ├── JeuCombat.Infrastructure/
│   │   └── ConsoleUI/
│   │
│   └── JeuCombat.Cli/
│       └── Program.cs
│
├── tests/
│   └── JeuCombat.Tests/
│
├── docs/
│   └── architecture.md
│
├── .editorconfig
├── README.md
└── JeuCombat.sln
```

## Architecture

Le projet est séparé en plusieurs couches.

### JeuCombat.Domain

Contient le coeur métier du jeu.

Cette couche ne dépend d'aucune autre couche.

Elle contient notamment :

- `Personnage`
- `Heros`
- `Ennemi`
- `Vague`
- `ClasseHero`
- `TypeEnnemi`
- `CombatRules`

### JeuCombat.Application

Contient la logique applicative du combat.

Elle orchestre les actions, les états, les commandes, l'IA et les événements.

Elle contient notamment :

- Actions de combat
- Commandes joueur
- États du combat
- Session de combat
- IA ennemie
- Factories
- Journal
- Publisher d'événements

### JeuCombat.Infrastructure

Contient les détails techniques.

Actuellement, cette couche contient l'interface console :

- lecture utilisateur
- rendu console
- affichage des messages
- lancement du jeu console

### JeuCombat.Cli

Point d'entrée de l'application.

Le fichier `Program.cs` reste volontairement court. Il assemble les dépendances principales et lance le jeu.

### JeuCombat.Tests

Contient les tests unitaires.

Les tests ciblent principalement la logique métier et applicative, sans dépendre de l'affichage console.

## Règle de dépendance

Les dépendances suivent ce sens :

```txt
Cli -> Infrastructure -> Application -> Domain
Cli -> Application -> Domain
Tests -> Application -> Domain
```

Règles importantes :

- `Domain` ne référence aucun autre projet.
- `Application` référence `Domain`.
- `Infrastructure` référence `Application` et `Domain`.
- `Cli` assemble l'application.
- La console ne contient pas les règles de combat.
- `Program.cs` ne contient pas la logique métier.

## Design patterns utilisés

| Pattern | Classes / interfaces | Rôle dans le jeu |
| --- | --- | --- |
| Strategy | `ICombatAction`, `AttaqueBasiqueAction`, `SoinAction`, `CompetenceGuerrierAction`, `CompetenceMageAction`, `CompetenceVoleurAction` | Chaque action de combat est un algorithme interchangeable. Le menu console ne calcule pas les dégâts directement. |
| Strategy | `IEnnemiAiStrategy`, `AttaqueSimpleEnnemiAiStrategy` | L'IA ennemie est séparée de la session de combat. La session orchestre, la stratégie décide comment un ennemi agit. |
| State | `ICombatState`, `TourJoueurState`, `TourEnnemiState`, `EntreVaguesState`, `VictoireState`, `DefaiteState` | Le combat change d'état sans grosse cascade de `if/else`. Chaque état porte son propre comportement. |
| Factory | `IHerosFactory`, `HerosFactory`, `IEnnemiFactory`, `EnnemiFactory`, `IVagueFactory`, `VagueFactory` | La création des héros, ennemis et vagues est centralisée. Les `new` ne sont pas dispersés dans toute l'application. |
| Command | `ICommand`, `AttaquerCommand`, `UtiliserCompetenceCommand`, `SoignerCommand`, `AfficherJournalCommand`, `ActionInvoker` | Les choix du joueur sont encapsulés dans des commandes exécutables. |
| Observer | `ICombatObserver`, `ICombatEventPublisher`, `CombatEventPublisher`, `InMemoryCombatJournal`, `ConsoleCombatObserver` | Les événements de combat sont publiés à plusieurs observateurs : journal en mémoire et affichage console. |

## Justification des patterns

### Strategy

Les actions de combat sont représentées par des classes séparées.

Cela permet d'ajouter une nouvelle action sans modifier le menu ou la session de combat.

Exemple :

```txt
ICombatAction
├── AttaqueBasiqueAction
├── SoinAction
├── CompetenceGuerrierAction
├── CompetenceMageAction
└── CompetenceVoleurAction
```

### State

Le combat possède plusieurs états :

```txt
Tour du joueur
Tour ennemi
Entre deux vagues
Victoire
Défaite
```

Chaque état est représenté par une classe dédiée.

Cela évite une boucle de combat trop complexe avec beaucoup de conditions.

### Factory

Les factories créent les objets du jeu :

```txt
HerosFactory
EnnemiFactory
VagueFactory
```

Cela centralise les statistiques et évite de répéter les mêmes constructions dans plusieurs fichiers.

### Command

Chaque choix du joueur est représenté par une commande :

```txt
1 -> AttaquerCommand
2 -> UtiliserCompetenceCommand
3 -> SoignerCommand
4 -> AfficherJournalCommand
```

Cela sépare la saisie utilisateur de l'exécution métier.

### Observer

Les actions publient des événements.

Ces événements peuvent être reçus par plusieurs observateurs :

```txt
CombatEventPublisher
├── InMemoryCombatJournal
└── ConsoleCombatObserver
```

Le système est donc extensible. Par exemple, on pourrait ajouter plus tard un observateur qui sauvegarde les événements dans un fichier.

## Règles du jeu

### Démarrage

Au lancement, le joueur doit :

1. Saisir le nom du héros.
2. Choisir une classe.
3. Combattre les vagues d'ennemis.

### Classes de héros

| Classe | Points de vie | Attaque de base | Compétence spéciale |
| --- | ---: | ---: | --- |
| Guerrier | 120 | 18 | Frappe lourde : dégâts x1,5, cooldown de 2 tours |
| Mage | 80 | 12 | Éclair : dégâts magiques fixes, ignore 50 % de l'armure ennemie, cooldown de 3 tours |
| Voleur | 90 | 14 | Coup critique : 30 % de chance de doubler les dégâts, cooldown de 2 tours |

### Vagues

| Vague | Ennemis |
| --- | --- |
| 1 | 1 Gobelin |
| 2 | 1 Gobelin + 1 Gobelin archer |
| 3 | 1 Chef orc |

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

### Cooldown

Chaque compétence spéciale déclenche un cooldown.

Le cooldown diminue après le tour ennemi.

### Conditions de fin

| État | Condition |
| --- | --- |
| Victoire | Tous les ennemis de la vague 3 sont vaincus |
| Défaite | Les points de vie du héros sont inférieurs ou égaux à 0 |
| Entre vagues | Si le héros survit, il récupère 20 % de ses PV maximum avant la vague suivante |

## Tests

Le projet utilise xUnit.

Lancer tous les tests :

```bash
dotnet test
```

Les tests couvrent notamment :

- Les points de vie.
- Les dégâts.
- Le soin.
- Les cooldowns.
- Les factories.
- Les actions de combat.
- Les commandes.
- Les événements Observer.
- Les états du combat.
- L'IA ennemie.
- Les scénarios complets de victoire et de défaite.

## Qualité de code

Le projet applique les règles suivantes :

- Responsabilités séparées.
- Pas de logique métier dans `Program.cs`.
- Pas de logique métier dans la console.
- Pas de classe fourre-tout.
- Pas de nombres magiques pour les règles principales.
- Utilisation de constantes dans `CombatRules`.
- Actions séparées via Strategy.
- États séparés via State.
- Création centralisée via Factory.
- Événements de combat via Observer.
- Tests unitaires sur la logique sans console.

## Configuration qualité

Le fichier `.editorconfig` configure le formatage.

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

## État du projet

- [x] Mise en place de la solution .NET
- [x] Création des projets
- [x] Configuration du formatage
- [x] Configuration des hooks Git
- [x] Création du domaine métier
- [x] Création des factories
- [x] Création des actions de combat
- [x] Création des commandes joueur
- [x] Création du système d'événements Observer
- [x] Création de la machine à états
- [x] Création de l'IA ennemie
- [x] Création de l'interface console
- [x] Ajout de tests unitaires
- [x] Ajout de tests de scénarios complets
- [x] Documentation d'architecture
- [ ] Bonus Repository JSON éventuel

## Convention de commit

Format recommandé :

```txt
type(scope): message
```

Exemples :

```txt
chore(project): initialize dotnet solution
feat(domain): add characters and health rules
feat(factory): add hero enemy and wave factories
feat(strategy): add combat actions
feat(command): add player combat commands
feat(observer): add combat event publisher
feat(state): add combat session states
feat(ai): add enemy attack strategy
feat(console): add playable cli game
test(combat): add full session scenario tests
docs(readme): document architecture and patterns
```

## Auteur

Projet réalisé par Enzo Drissi.
