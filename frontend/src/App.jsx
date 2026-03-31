import React from "react";



function App() {
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
              <li><a href="#contact">Kapcsolat</a></li>
              <li><a href="#trains">Vonatok</a></li>
            </ul>

            <div className="auth-buttons">
              <button id="loginBtn">Bejelentkezés</button>
              <button id="registerBtn">Regisztráció</button>
              <button id="logoutBtn" style={{ display: "none" }}>
                Kijelentkezés
              </button>
            </div>
          </nav>
        </section>
      </header>

      {/* CAROUSEL */}
      <div className="carousel-container">
        <div className="carousel" id="carousel">
          <img src="https://www.trains.com/wp-content/uploads/2025/02/CTT-trzoniec-realistic-setting-0225.jpg" alt="Model train realistic landscape" className="carousel-image" />
          <img src="https://i.ytimg.com/vi/4m8inWO8cgg/sddefault.jpg" alt="HO scale train in snow" className="carousel-image" />
          <img src="https://cdn11.bigcommerce.com/s-stpzw4awzh/images/stencil/1240x826/uploaded_images/20211114-014209.jpg" alt="Model railway bridge scene" className="carousel-image" />
          <img src="https://i.ytimg.com/vi/NIrYZtX9Ym0/maxresdefault.jpg" alt="Detailed model train diorama" className="carousel-image" />
          <img src="IMG_1305.jpg" alt="Saját kép" className="carousel-image" />
          <img src="https://i.ytimg.com/vi/Ss-sf1hY_RU/maxresdefault.jpg" alt="Detailed HO scale city scene" className="carousel-image" />
        </div>

        <button className="carousel-btn prev-btn" id="prevBtn">❮</button>
        <button className="carousel-btn next-btn" id="nextBtn">❯</button>

        <div className="indicators" id="indicators"></div>
      </div>

      <main>
        <section id="userInfo" style={{ display: "none" }}>
          <p>Üdv, <span id="usernameDisplay"></span>!</p>
        </section>

        <section id="loginForm" className="card" style={{ display: "none" }}>
          <h2>Bejelentkezés</h2>
          <form>
            <input type="text" id="loginUsername" placeholder="Felhasználónév" required />
            <input type="password" id="loginPassword" placeholder="Jelszó" required />
            <button type="submit">Bejelentkezés</button>
          </form>
          <p id="loginError" className="error"></p>
        </section>

        <section id="registerForm" className="card" style={{ display: "none" }}>
          <h2>Regisztráció</h2>
          <form>
            <input type="text" id="registerUsername" placeholder="Felhasználónév" required />
            <input type="password" id="registerPassword" placeholder="Jelszó" required />
            <button type="submit">Regisztráció</button>
          </form>
          <p id="registerError" className="error"></p>
        </section>

        <section id="trains" className="card" style={{ display: "none" }}>
          <h2>Vonatok</h2>
          <p className="hint">Bejelentkezés után jelenik meg. (GET + POST)</p>

          <ul id="trainList" className="list"></ul>

          <h3>Új vonat</h3>
          <form id="trainForm">
            <input type="text" id="trainName" placeholder="Vonat neve" required />
            <button type="submit">Hozzáadás</button>
          </form>

          <p id="trainMessage" className="ok" style={{ display: "none" }}></p>
          <p id="trainError" className="error"></p>
        </section>
      </main>

      <section id="download" className="card">
        <div className="download-content">
          <h2>Letöltés</h2>
          <p>Kattints az alábbi gombra a MiniTrains letöltéséhez:</p>

          <button id="downloadBtn" className="download-button">Letöltés</button>
          <p id="downloadMessage" className="ok" style={{ display: "none" }}></p>

          <h3>Elérhető letöltések</h3>
          <ul id="downloadList" className="list"></ul>
        </div>
      </section>

      <section id="contact" className="card">
        <h2>Kapcsolat</h2>

        <form id="contactForm">
          <input type="text" id="contactName" placeholder="Név" required />
          <input type="email" id="contactEmail" placeholder="Email" required />
          <textarea id="contactMessage" placeholder="Üzenet" required></textarea>
          <button type="submit">Küldés</button>
          <p id="formMessage" className="ok" style={{ display: "none" }}>
            Üzenet elküldve!
          </p>
          <p id="contactError" className="error"></p>
        </form>

        <h3>Beérkezett üzenetek (GET)</h3>
        <ul id="contactList" className="list"></ul>
      </section>
      
    </>
  );
}

export default App;