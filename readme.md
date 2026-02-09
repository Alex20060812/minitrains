# 🚂 Minitrains

## 📌 Projekt leírás

A **Minitrains** egy digitális modellvasút-vezérlő alkalmazás, amely lehetővé teszi a felhasználók számára, hogy **számítógépről irányítsák Z21 központtal rendelkező modellvasútjukat**.

Az alkalmazás egyik fő célja az **egyszerűség, átláthatóság és testreszabhatóság**.  
A felhasználók több vasútmodellt is kezelhetnek, melyek beállításai **felhasználói fiókhoz kötötten** kerülnek eltárolásra, így egy másik eszközről történő bejelentkezéskor a korábbi konfigurációk automatikusan betöltődnek.

### Fő funkciók:
- Digitális modellvasút vezérlése Z21 központon keresztül
- Felhasználói fiókok kezelése
- Több vasútmodell kezelése
- Funkciók elnevezése, alapállapotának beállítása
- Funkciók elrejtése / megjelenítése
- Ikonok rendelése a funkciókhoz
- Egyszerű, ingyenes, letisztult felhasználói felület

---

## 👥 Csapattagok és feladatok

- **Kovácsevics Alex**
  - Asztali alkalmazás (C# WinForms)
  - Adatbázis tervezés és megvalósítás (MySQL)

- **Kovács Ákos Gergő**
  - Weboldal (HTML, CSS)
  - REST API(Javascript)

---

## 🛠️ Felhasznált technológiák

- **Asztali alkalmazás:** C# (.NET WinForms)
- **Weboldal:** HTML, CSS
- **Adatbázis:** MySQL
- **REST API:** Javascript
- **Kommunikáció:** Z21 digitális modellvasút központ

---

## 🗄️ Adatbázis felépítése

Az adatbázis célja a felhasználók, vasútmodellek és a hozzájuk tartozó funkciók adatainak tárolása.

### 📄 users
Felhasználói fiókok adatai.

| Oszlop neve | Típus | Leírás |
|------------|------|--------|
| id | int(11) | Elsődleges kulcs |
| username | varchar(50) | Felhasználónév |
| password_hash | varchar(255) | Jelszó hash |
| remember_token | varchar(255) | Bejelentkezési token |

---

### 🚆 trains
A felhasználó által létrehozott vasútmodellek.

| Oszlop neve | Típus | Leírás |
|------------|------|--------|
| id | int(11) | Elsődleges kulcs |
| user_id | int(11) | Kapcsolódó felhasználó |
| name | varchar(100) | Vasútmodell neve |

---

### ⚙️ functions
Egy adott vasútmodellhez tartozó funkciók (pl. világítás, kürt, hangok).

| Oszlop neve | Típus | Leírás |
|------------|------|--------|
| id | int(11) | Elsődleges kulcs |
| train_id | int(11) | Kapcsolódó vasútmodell |
| number | int(11) | Funkció száma (pl. F0, F1, F2…) |
| name | varchar(50) | Alapértelmezett funkciónév |
| icon | varchar(100) | Funkció ikon fájlneve |
| hidden | tinyint(1) | Funkció rejtett-e (0 = látható, 1 = rejtett) |

---

### 🧩 functions_settings
Felhasználó által testreszabott funkcióbeállítások.

| Oszlop neve | Típus | Leírás |
|------------|------|--------|
| id | int(11) | Elsődleges kulcs |
| function_id | int(11) | Kapcsolódó funkció |
| custom_name | varchar(50) | Egyedi funkciónév |
| default_state | tinyint(1) | Alapállapot (0 = kikapcsolt, 1 = bekapcsolt) |

---

### 🔌 train_details
Vasútmodellhez tartozó technikai adatok.

| Oszlop neve | Típus | Leírás |
|------------|------|--------|
| id | int(11) | Elsődleges kulcs |
| train_id | int(11) | Kapcsolódó vasútmodell |
| dcc_address | int(11) | DCC cím |

---

## 🔗 Kapcsolatok összefoglalása

- Egy **felhasználó** több **vasútmodellt** hozhat létre
- Egy **vasútmodellhez** több **funkció** tartozhat
- Egy **funkcióhoz** tartozhat egyedi beállítás a `functions_settings` táblában
- A `train_details` tábla technikai adatokat tárol egy adott vonathoz

---

## 🎯 Projekt cél

Egy **könnyen kezelhető, modern és ingyenes** alkalmazás készítése, amely segíti a modellvasút-rajongókat vasútjaik digitális vezérlésében, minden felesleges bonyolítás nélkül.

---

