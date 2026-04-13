# Weboldal – Backend

Ez a repository a weboldal backend részét tartalmazza. Az alábbi útmutató segítségével letöltheti és elindíthatja a szervert lokális környezetben.

---

## 📥 Backend letöltése

1. Klónozza vagy töltse le a repository-t a GitHub-ról.
2. Nyisson meg egy terminált a projekt mappájában.
3. Navigáljon a **backend** mappába:

```bash
cd backend
```

---

## 📦 Függőségek telepítése

A backend működéséhez szükséges csomagok telepítéséhez futtassa az alábbi parancsokat:

```bash
npm install bcrypt cors dotenv express jsonwebtoken mysql2
vagy
npm install
```

---

## ▶️ Backend indítása

A szerver indításához futtassa a következő parancsot:

```bash
node index.js
```

Sikeres indítás esetén a terminálban az alábbi üzenet jelenik meg:

```
Backend fut a http://localhost:3001
Adatbázis kapcsolat OK
```

---

## ⚠️ Hibakezelés

Ha az alábbi hiba jelenik meg:

```
Adatbázis kapcsolat HIBA: ECONNREFUSED
```

akkor valószínűleg az adatbázis portszáma nem megfelelő.

### Megoldás

1. Nyissa meg a `.env` fájlt a `routes` mappában.
2. Módosítsa a MySQL portszámát például:

```env
DB_PORT=3306
```

vagy állítsa arra a portra, amelyen a MySQL szerver fut.

---

## 🛠 Használt technológiák

- Node.js  
- Express  
- MySQL  
- JWT (jsonwebtoken)  
- bcrypt

---

## 📌 Megjegyzés

Győződjön meg róla, hogy a **MySQL szerver fut**, mielőtt elindítja a backendet, különben a szerver nem tud csatlakozni az adatbázishoz.
