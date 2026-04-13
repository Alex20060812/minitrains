import React, { useState, useEffect } from "react";

function App() {
  const [token, setToken] = useState(localStorage.getItem("token") || "");
  const [username, setUsername] = useState("");
  const [trains, setTrains] = useState([]);
  const [showLogin, setShowLogin] = useState(false);
  const [showRegister, setShowRegister] = useState(false);
  const images = [
  "/kepek/20251229-IMG_4029.jpg",
  "/kepek/20251229-IMG_4033.jpg",
  "/kepek/20251229-IMG_4075.jpg",
  "/kepek/20251229-IMG_4006.jpg",
  "/kepek/20251229-IMG_4002.jpg",
  "/kepek/20251229-IMG_3985.jpg",
  "/kepek/20251229-IMG_3973.jpg",
  "/kepek/20251229-IMG_3964.jpg"
  ];
  
  const [currentIndex, setCurrentIndex] = useState(0);
  
  // ⏱ AUTO SLIDE 2 másodpercenként
  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentIndex((prev) => (prev + 1) % images.length);
    }, 2000);
  
    return () => clearInterval(interval);
  }, []);


  // Bejelentkezett felhasználó neve lekérése
  useEffect(() => {
    if (token) {
      fetch("http://localhost:3000/api/users/me", {
        headers: { Authorization: `Bearer ${token}` },
      })
        .then((res) => res.json())
        .then((data) => {
          if (data.username) setUsername(data.username);
        })
        .catch(() => {});
    }
  }, [token]);

  // Vonatok lekérése bejelentkezés után
  useEffect(() => {
    if (token) {
      fetch("http://localhost:3000/api/trains", {
        headers: { Authorization: `Bearer ${token}` },
      })
        .then((res) => res.json())
        .then((data) => setTrains(data))
        .catch(() => setTrains([]));
    }
  }, [token]);

  const handleLogin = async (e) => {
    e.preventDefault();
  
    const username = e.target.loginUsername.value;
    const email = e.target.loginEmail.value;
    const password = e.target.loginPassword.value;
  
    try {
      const res = await fetch("http://localhost:3001/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, email, password }),
      });
  
      const data = await res.json();
  
      if (data.token) {
        localStorage.setItem("token", data.token);
        setToken(data.token);
        setShowLogin(false);
      } else {
        alert(data.error || "Hibás bejelentkezés");
      }
    } catch {
      alert("Szerver hiba");
    }
  };
  const handleRegister = async (e) => {
    e.preventDefault();
  
    const username = e.target.registerUsername.value;
    const email = e.target.registerEmail.value;
    const password = e.target.registerPassword.value;
  
    try {
      const res = await fetch("http://localhost:3001/auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, email, password }),
      });
  
      const data = await res.json();
  
      if (res.ok) {
        alert(data.message || "Sikeres regisztráció");
        setShowRegister(false);
        setShowLogin(true);
      } else {
        alert(data.error || "Hiba történt");
      }
    } catch {
      alert("Szerver hiba");
    }
  };
  const handleLogout = () => {
    localStorage.removeItem("token");
    setToken("");
    setUsername("");
    setTrains([]);
  };

 

  return (
    <>
      <header>
        <section id="home">
          <div className="menu">
            <div className="cim">
              <h1>Welcome to MiniTrains</h1>
            </div>
          </div>

          <nav>
            <ul className="nav-links">
              <li><a href="#home">Főoldal</a></li>
              
              <li><a href="#download">Letöltés</a></li>
              <li><a href="#descriptionra">Leíras</a></li>
              
              
            </ul>

            <div className="auth-buttons">
              {!token ? (
                <>
                  <button onClick={() => { setShowLogin(true); setShowRegister(false); }}>
                    Bejelentkezés
                  </button>
                  <button onClick={() => { setShowRegister(true); setShowLogin(false); }}>
                    Regisztráció
                  </button>
                </>
              ) : (
                <>
                  <span>Üdv, {username}!</span>
                  <button onClick={handleLogout}>Kijelentkezés</button>
                </>
              )}
            </div>
          </nav>
        </section>
      </header>

     {/* CAROUSEL – AUTO SLIDE */}
<div className="carousel-container">
  <div className="carousel" id="carousel">
    <img
      src={images[currentIndex]}
      alt="carousel"
      className="carousel-image"
    />
  </div>
</div>

      <main>
        {/* Bejelentkezési form */}
        {showLogin && !token && (
          <section id="loginForm" className="card">
            <h2>Bejelentkezés</h2>
            <form onSubmit={handleLogin}>
              <input type="text" name="loginUsername" placeholder="Felhasználónév" required />
              <input type="email" name="loginEmail" placeholder="Email" required />
              <input type="password" name="loginPassword" placeholder="Jelszó" required />
              <button type="submit">Bejelentkezés</button>
            </form>
          </section>
        )}

        {/* Regisztrációs form */}
        {showRegister && !token && (
          <section id="registerForm" className="card">
            <h2>Regisztráció</h2>
            <form onSubmit={handleRegister}>
              <input type="text" name="registerUsername" placeholder="Felhasználónév" required />
              <input type="email" name="registerEmail" placeholder="Email" required />
              <input type="password" name="registerPassword" placeholder="Jelszó" required />
              <button type="submit">Regisztráció</button>
            </form>
          </section>
        )}
        <section id="download" className="card">
      <div className="download-content">
        <h2>Letöltés</h2>
        <p>Kattints az alábbi gombra a MiniTrains letöltéséhez:</p>

        <button
           id="downloadBtn"
            className="download-button"
          onClick={() => window.location.href = "https://github.com/Alex20060812/minitrains"}
          >
            Letöltés
        </button>

        <p id="downloadMessage" className="ok" style={{ display: "none" }}></p>
        </div>
      </section>
      {/* LEÍRÁS SZEKCIÓ */}
      <section id="descriptionra" className="card">
         <h2>Leírás</h2>
         <p>
         A Minitrains egy digitális modellvasút-vezérlő alkalmazás, amely lehetővé teszi a felhasználók számára, hogy számítógépről irányítsák Z21 központtal rendelkező modellvasútjukat.
         Az alkalmazás egyik fő célja az egyszerűség, átláthatóság és testreszabhatóság.
         A felhasználók több vasútmodellt is kezelhetnek, melyek beállításai felhasználói fiókhoz kötötten kerülnek eltárolásra. Így egy másik eszközről történő bejelentkezéskor a korábbi konfigurációk automatikusan betöltődnek.
         
         <h2>Fő funkciók</h2>
         Digitális modellvasút vezérlése Z21 központon keresztül
         Felhasználói fiókok kezelése
         Több vasútmodell kezelése
         Funkciók elnevezése
         Funkciók alapállapotának beállítása
         Funkciók elrejtése / megjelenítése
         Ikonok rendelése a funkciókhoz
         Egyszerű, letisztult felhasználói felület
              </p>
          <h2>Követelmények</h2>
          <p>
          A program futtatásához szükséges:

            Windows 11 ,
            Visual Studio 2026,
            XAMPP,
            MySQL,
            WiFi kapcsolat a Z21 központhoz,
          </p>
              </section>

        

       
      </main>
    </>
  );
}

export default App;