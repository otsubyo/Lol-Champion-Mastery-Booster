const CHAMPION_JSON_URL = "https://ddragon.leagueoflegends.com/cdn/14.13.1/data/fr_FR/champion.json";
let championMapping = {};

// Charge le mapping championId → nom
async function loadChampionMapping() {
    const res = await fetch(CHAMPION_JSON_URL);
    const data = await res.json();
    Object.values(data.data).forEach(champ => {
        championMapping[parseInt(champ.key)] = champ.name;
    });
    console.log("Champion mapping chargé :", Object.keys(championMapping).length, "champions");
}

// Charge les masteries
async function loadMasteries() {
    const rawToken = document.getElementById("token").value.trim();
    const profileId = document.getElementById("profileId").value.trim();

    if (!rawToken || !profileId) {
        alert("Veuillez saisir un token et un Profile ID !");
        return;
    }

    // S'assure d'avoir un header Authorization correct
    const authHeader = rawToken.startsWith("Bearer ") ? rawToken : `Bearer ${rawToken}`;
    console.log("Header utilisé :", authHeader);

    try {
        const res = await fetch(`https://localhost:7213/api/Profiles/${profileId}/mastery`, {
            headers: {
                "Authorization": authHeader
            }
        });

        if (res.status === 401) {
            alert("Erreur 401 : Token invalide ou manquant.");
            return;
        }

        if (!res.ok) {
            const errText = await res.text();
            alert("Erreur " + res.status + " : " + errText);
            return;
        }

        const masteries = await res.json();
        renderTable(masteries);
    } catch (err) {
        console.error("Erreur réseau ou CORS :", err);
        alert("Erreur réseau ou CORS : " + err.message);
    }
}

// Affiche les masteries dans le tableau
function renderTable(masteries) {
    const tbody = document.querySelector("#masteryTable tbody");
    tbody.innerHTML = "";

    masteries.forEach(m => {
        const tr = document.createElement("tr");
        const champName = championMapping[m.championId] || `ID ${m.championId}`;
        tr.innerHTML = `
      <td>${champName}</td>
      <td>${m.championLevel}</td>
      <td>${m.championPoints.toLocaleString()}</td>
    `;
        tbody.appendChild(tr);
    });
}

document.getElementById("load").addEventListener("click", loadMasteries);

// Charger le mapping champion dès le démarrage
loadChampionMapping();
