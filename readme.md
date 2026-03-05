# 🚂 Minitrains

Digitális modellvasút vezérlő alkalmazás **Z21 központtal rendelkező modellvasutakhoz**.

---

## 📌 Projekt leírás

A **Minitrains** egy digitális modellvasút-vezérlő alkalmazás, amely lehetővé teszi a felhasználók számára, hogy **számítógépről irányítsák Z21 központtal rendelkező modellvasútjukat**.

Az alkalmazás egyik fő célja az **egyszerűség, átláthatóság és testreszabhatóság**.

A felhasználók több vasútmodellt is kezelhetnek, melyek beállításai **felhasználói fiókhoz kötötten** kerülnek eltárolásra. Így egy másik eszközről történő bejelentkezéskor a korábbi konfigurációk automatikusan betöltődnek.

---

## 🚀 Fő funkciók

- Digitális modellvasút vezérlése **Z21 központon keresztül**
- Felhasználói fiókok kezelése
- Több vasútmodell kezelése
- Funkciók elnevezése
- Funkciók alapállapotának beállítása
- Funkciók elrejtése / megjelenítése
- Ikonok rendelése a funkciókhoz
- Egyszerű, letisztult felhasználói felület

---

## 🛠️ Felhasznált technológiák

| Terület | Technológia |
|------|------|
| Asztali alkalmazás | C# (.NET WinForms) |
| Weboldal | HTML, CSS |
| REST API | JavaScript |
| Adatbázis | MySQL |
| Kommunikáció | Z21 digitális modellvasút központ |

---

## ⚙️ Fejlesztési környezet

A projekt az alábbi környezetben lett készítve és tesztelve:

- **Visual Studio 2026**
- **.NET 10.0**
- **XAMPP 3.3.0**

---

## 📋 Követelmények

A program futtatásához szükséges:

- Windows 11
- Visual Studio 2026
- XAMPP
- MySQL
- WiFi kapcsolat a **Z21 központhoz**

---

## 💻 Projekt telepítése

### 1️⃣ Repository klónozása

Nyissunk egy terminált és futtassuk:

```bash
git clone https://github.com/Alex20060812/minitrains
```

Ezután lépjünk be a mappába:

```bash
cd minitrains
```

---

## 🗄️ Adatbázis beállítása

1. Nyissuk meg a **XAMPP Control Panel-t**
2. Indítsuk el az alábbi szolgáltatásokat:

- Apache
- MySQL

3. Kattintsunk a **MySQL → Admin** gombra (phpMyAdmin)

4. Hozzunk létre egy új adatbázist:

```
modellvasut
```

5. Nyissuk meg az **Importálás** fület

6. Tallózzuk be a projektben található SQL fájlt:

```
database/modellvasut.sql
```

7. Futtassuk az importot

---

## 🖥️ Asztali alkalmazás futtatása

1. Nyissuk meg a projekt mappát
2. Lépjünk be a következő könyvtárba:

```
Desktop
```

3. Nyissuk meg a projekt fájlt:

```
minitrains.slnx
```

4. A projekt megnyitásához használjuk a **Visual Studio 2026** verziót  

⚠️ **Korábbi Visual Studio verzióval a projekt nem futtatható.**

---

## 📦 NuGet csomag telepítése

A projekt betöltése után:

1. Jobb klikk a projektre
2. **Manage NuGet Packages**
3. **Browse** fül
4. Keressünk rá:

```
MySql.Data
```

5. Telepítsük a csomagot

---

## ⚠️ Fontos indítási feltétel

A projekt futtatása előtt:

- csatlakozzunk **WiFi-n a Z21 központhoz**
- a **Z21 legyen bekapcsolva és áram alatt**

Csak ezután indítsuk el az alkalmazást.

---

## 🗄️ Adatbázis felépítése

Az adatbázis célja a felhasználók, vasútmodellek és a hozzájuk tartozó funkciók adatainak tárolása.

---

### 👤 users

Felhasználói fiókok adatai.

| Oszlop | Típus | Leírás |
|------|------|------|
| id | int(11) | Elsődleges kulcs |
| username | varchar(50) | Felhasználónév |
| email | varchar(255) | Email |
| password_hash | varchar(255) | Jelszó hash |
| remember_token | varchar(255) | Bejelentkezési token |

---

### 🚆 trains

A felhasználó által létrehozott vasútmodellek.

| Oszlop | Típus | Leírás |
|------|------|------|
| id | int(11) | Elsődleges kulcs |
| user_id | int(11) | Kapcsolódó felhasználó |
| name | varchar(100) | Vasútmodell neve |

---

### ⚙️ functions

Egy adott vasútmodellhez tartozó funkciók.

| Oszlop | Típus | Leírás |
|------|------|------|
| id | int(11) | Elsődleges kulcs |
| train_id | int(11) | Kapcsolódó vasútmodell |
| number | int(11) | Funkció száma |
| name | varchar(50) | Alapértelmezett funkciónév |
| icon | varchar(100) | Funkció ikon fájlneve |
| hidden | tinyint(1) | Funkció rejtett-e |

---

### 🧩 functions_settings

Felhasználó által testreszabott funkcióbeállítások.

| Oszlop | Típus | Leírás |
|------|------|------|
| id | int(11) | Elsődleges kulcs |
| function_id | int(11) | Kapcsolódó funkció |
| custom_name | varchar(50) | Egyedi funkciónév |
| default_state | tinyint(1) | Alapállapot |

---

### 🔌 train_details

Vasútmodellhez tartozó technikai adatok.

| Oszlop | Típus | Leírás |
|------|------|------|
| id | int(11) | Elsődleges kulcs |
| train_id | int(11) | Kapcsolódó vasútmodell |
| dcc_address | int(11) | DCC cím |

---

## 🔗 Adatbázis kapcsolatok

- Egy **felhasználó** több **vasútmodellt** hozhat létre
- Egy **vasútmodellhez** több **funkció** tartozhat
- Egy **funkcióhoz** tartozhat egyedi beállítás a `functions_settings` táblában
- A `train_details` tábla technikai adatokat tárol egy adott vonathoz

---

## 👥 Csapattagok

### Kovácsevics Alex
- Asztali alkalmazás (C# WinForms)
- Adatbázis tervezés és megvalósítás (MySQL)

### Kovács Ákos Gergő
- Weboldal (HTML, CSS)
- REST API (JavaScript)

---

## 🎯 Projekt cél

Egy **könnyen kezelhető, modern és ingyenes alkalmazás** létrehozása, amely segíti a modellvasút-rajongókat vasútjaik digitális vezérlésében.

---
