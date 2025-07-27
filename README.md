# 🎮 LoL Champion Mastery Booster

Une API ASP.NET Core qui permet de **lier un compte League of Legends** (via l’API Riot) et de récupérer **les maîtrises de champions**.  
Elle est accompagnée d’un petit frontend (HTML/CSS/JS) pour tester l’API visuellement.

---

## 🚀 Présentation du projet

**LoL Champion Mastery Booster** permet :
- De créer un profil lié à un compte Riot (`GameName#TAG` et région).
- De récupérer en temps réel la liste des champions maîtrisés, triée par points de maîtrise.
- De s’authentifier via JWT pour protéger les données.

✅ Architecture propre (Controllers, Services, DTOs, InMemory DB)  
✅ Authentification JWT avec comptes tests  
✅ Swagger documenté  
✅ Frontend minimal HTML/CSS/JS pour tester

---

## ⚡ Comment démarrer le backend

### 1. Prérequis
- .NET 7 ou plus
- Visual Studio ou VS Code

### 2. Configuration Riot API
👉 **IMPORTANT :**  
Vous devez créer un compte développeur sur le [Riot Developer Portal](https://developer.riotgames.com/) pour obtenir une **clé d’API Riot**.  
Ensuite, placez cette clé dans vos secrets utilisateur (ou appsettings) :
```bash
dotnet user-secrets set "RiotApiKey" "<VOTRE_CLÉ_RIOT>"
```

### 3. Lancer le backend
```bash
cd Lol-Champion-Mastery-Booster
dotnet restore
dotnet run
```

Le backend démarre par défaut sur :
```
https://localhost:7213
```

Swagger sera disponible sur :
```
https://localhost:7213/swagger
```

---

## 🖥️ Comment démarrer le frontend

### 1. Prérequis
Pas besoin de backend supplémentaire, un simple serveur statique suffit.

### 2. Lancer le front
Ouvrez un terminal dans le dossier `Frontend` :
```bash
cd Frontend
npx serve
```
Puis ouvrez l’URL affichée, par exemple :
```
http://localhost:3000
```

👉 **Alternative :** Ouvrez `index.html` directement dans un navigateur ou utilisez l’extension VS Code **Live Server**.

---

## 🔑 Identifiants de connexion pour tester

Un compte test est déjà prévu dans le backend :

| Email | Mot de passe |
|-------|--------------|
| testuser@example.com | Passw0rd! |

Procédure :
1. `POST /api/Auth/login` avec les identifiants ci-dessus pour obtenir un JWT.
2. Utilisez ce JWT dans le header **Authorization** :
```
Authorization: Bearer <votre_token>
```

---

## 📚 Endpoints principaux

| Méthode | Endpoint | Description |
|---------|----------|-------------|
| POST | `/api/Auth/register` | Créer un compte |
| POST | `/api/Auth/login` | Obtenir un JWT |
| POST | `/api/Profiles` | Créer un profil LoL |
| GET | `/api/Profiles/{id}/mastery` | Obtenir les maîtrises |

---
## ✨ Bonus

- Frontend HTML/CSS/JS minimal dans le dossier `Frontend` :
  - Saisir un token
  - Saisir un ProfileId
  - Visualiser les champions avec leur nom, niveau et points

---

👉 **Si besoin d’aide pour la clé Riot ou les secrets utilisateur, consulte la doc officielle :**  
[https://learn.microsoft.com/fr-fr/aspnet/core/security/app-secrets](https://learn.microsoft.com/fr-fr/aspnet/core/security/app-secrets)
