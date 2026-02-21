# 🐢 Turtle Battle

![Unity](https://img.shields.io/badge/Unity-2022.3+-black?logo=unity) ![Language](https://img.shields.io/badge/Language-C%23-blue?logo=csharp) ![Genre](https://img.shields.io/badge/Genre-Survivor%20%2F%20Tower%20Defense-green)

**Turtle Battle** est un jeu hybride mêlant les genres **Survivor** et **Tower Defense** en vue de dessus (2D). Incarnez un ingénieur qui doit protéger une tortue géante, votre base mobile, tout en repoussant des hordes d'ennemis.

---

## 📖 Présentation du Projet
Le joueur incarne un ingénieur dont la survie dépend d'une tortue géante. La tortue suit le joueur et sert de plateforme de défense sur laquelle vous pouvez placer et gérer des tourelles. Le cœur du gameplay repose sur la synergie entre le mouvement pour collecter des ressources et la défense automatique.

* **Objectif :** Survivre pendant **5 minutes**.
* **Condition de défaite :** Mort du joueur ou de la tortue (PV à zéro).

---

## 🎮 Commandes
| Touche | Action |
| :--- | :--- |
| **Z Q S D** | Se déplacer |
| **Clic Gauche** | Sélectionner/Poser une tourelle (UI ou Carapace) |
| **Clic Droit** | Enlever une tourelle de la carapace |

---

## ⚙️ Mécaniques de Jeu

### 🛡️ La Tortue & Défense
* **Base Mobile :** La tortue suit fidèlement vos déplacements (`TurtleFollow.cs`).
* **Carapace :** Divisée en slots hexagonaux permettant de placer différents types de tourelles.
* **Combat :** Les tourelles détectent et attaquent automatiquement via un système de Raycast (`TurretShoot.cs`).

### 📈 Progression
* **Expérience :** Éliminer des ennemis rapporte de l'XP. Chaque montée de niveau permet de choisir un bonus parmi 3 statistiques (Joueur ou Tortue).
* **Événements :** Des zones de coffres apparaissent aléatoirement. Restez à l'intérieur pour débloquer de nouvelles tourelles.
* **Monde Infini :** Un système `AreaShift` repositionne le sol et les ennemis pour créer un espace de jeu sans fin.

---

## 🛠️ Architecture Technique
* **ScriptableObjects :** Configuration des types d'objets (dégâts, sprites, taille) pour un équilibrage facile.
* **PoolManager :** Optimisation de l'instanciation des projectiles et ennemis pour éviter les lags.
* **Singletons :** Centralisation via un `GameManager` (états, temps, XP) et un `AudioManager`.
* **UI Dynamique :** Gestion des barres de vie, menus de level-up et feedback visuel via `UIManager`.

---

## ✅ Fonctionnalités Réalisées
- [x] Mouvements fluides du joueur et IA de suivi de la tortue.
- [x] Système de combat complet (Raycast, projectiles, pénétration).
- [x] Gestion des tourelles (Placement/Retrait sur slots fixes).
- [x] Système de progression (Niveaux, stats, coffres).
- [x] Spawner d'ennemis avec difficulté progressive selon le temps.
- [x] Map infinie avec système de repositionnement d'objets.

---

## ⏳ Fonctionnalités Non Implémentées (Backlog)
* **Évolution de la Carapace :** Agrandir la taille de la tortue et ajouter des slots.
* **Tours Spéciales :** Lance-flammes, éclairs en chaîne ou tours de gel.
* **Effets de Statuts :** Brûlure (DoT), Ralentissement, Électricité.
* **IA Avancée :** Ennemis attaquant à distance.
* **Interactions :** Système pour monter sur la tortue ou la nourrir.

---

## 👥 Équipe
* **Marc SIROUX**
* **John LI**
* *Filière : LD*

---

## 📝 Conclusion
Ce projet a permis de mettre en pratique des concepts Unity avancés (Poolers, Singletons, Spawners). Malgré des contraintes de temps limitant certaines fonctionnalités secondaires, le cœur du jeu est fonctionnel et démontre une maîtrise du scripting C# et de l'architecture de jeu en 2D.
