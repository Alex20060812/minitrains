import { initCarousel } from "./index.js";
import {
  register,
  login,
  me,
  logoutLocal,
  createContact,
  getContacts,
  getDownloads,
  getTrains,
  createTrain
} from "./api.js";

function show(el, on = true) {
  if (!el) return;
  el.style.display = on ? "block" : "none";
}

function setText(id, text) {
  const el = document.getElementById(id);
  if (el) el.textContent = text;
}

function clearText(id) {
  setText(id, "");
}

document.addEventListener("DOMContentLoaded", async () => {
  // Carousel
  initCarousel();

  // DOM elemek
  const loginBtn = document.getElementById("loginBtn");
  const registerBtn = document.getElementById("registerBtn");
  const logoutBtn = document.getElementById("logoutBtn");

  const loginFormSection = document.getElementById("loginForm");
  const registerFormSection = document.getElementById("registerForm");

  const userInfo = document.getElementById("userInfo");
  const usernameDisplay = document.getElementById("usernameDisplay");

  const trainsSection = document.getElementById("trains");
  const trainList = document.getElementById("trainList");
  const trainForm = document.getElementById("trainForm");
  const trainName = document.getElementById("trainName");

  const downloadBtn = document.getElementById("downloadBtn");
  const downloadList = document.getElementById("downloadList");
  const downloadMessage = document.getElementById("downloadMessage");

  const contactForm = document.getElementById("contactForm");
  const contactList = document.getElementById("contactList");

  let currentUser = null;

  // UI frissítés login állapot szerint
  function setLoggedInUI(isLoggedIn) {
    loginBtn.style.display = isLoggedIn ? "none" : "inline-block";
    registerBtn.style.display = isLoggedIn ? "none" : "inline-block";
    logoutBtn.style.display = isLoggedIn ? "inline-block" : "none";

    show(userInfo, isLoggedIn);
    show(trainsSection, isLoggedIn);

    // űrlapok bezárása
    show(loginFormSection, false);
    show(registerFormSection, false);
    clearText("loginError");
    clearText("registerError");
  }

  // Auth gombok
  loginBtn.addEventListener("click", () => {
    show(loginFormSection, true);
    show(registerFormSection, false);
    clearText("loginError");
  });

  registerBtn.addEventListener("click", () => {
    show(registerFormSection, true);
    show(loginFormSection, false);
    clearText("registerError");
  });

  logoutBtn.addEventListener("click", () => {
    logoutLocal();
    currentUser = null;
    usernameDisplay.textContent = "";
    setLoggedInUI(false);
  });

  // Regisztráció submit
  registerFormSection.querySelector("form").addEventListener("submit", async (e) => {
    e.preventDefault();
    clearText("registerError");

    const username = document.getElementById("registerUsername").value.trim();
    const password = document.getElementById("registerPassword").value;

    try {
      await register(username, password);
      alert("Sikeres regisztráció! Most jelentkezz be.");
      show(registerFormSection, false);
    } catch (err) {
      setText("registerError", err.message);
    }
  });

  // Login submit
  loginFormSection.querySelector("form").addEventListener("submit", async (e) => {
    e.preventDefault();
    clearText("loginError");

    const username = document.getElementById("loginUsername").value.trim();
    const password = document.getElementById("loginPassword").value;

    try {
      const data = await login(username, password);
      // backendből jöhet user is
      currentUser = data?.user || null;

      setLoggedInUI(true);
      usernameDisplay.textContent = currentUser?.username || username;

      await loadTrains();
    } catch (err) {
      setText("loginError", err.message);
    }
  });

  // Trains: list
  async function loadTrains() {
    if (!trainList) return;

    clearText("trainError");
    show(document.getElementById("trainMessage"), false);

    try {
      const trains = await getTrains();
      trainList.innerHTML = "";

      if (!Array.isArray(trains) || trains.length === 0) {
        const li = document.createElement("li");
        li.textContent = "Nincs még vonat.";
        trainList.appendChild(li);
        return;
      }

      trains.forEach((t) => {
        const li = document.createElement("li");
        li.textContent = t.name ?? JSON.stringify(t);
        trainList.appendChild(li);
      });
    } catch (err) {
      setText("trainError", err.message);
    }
  }

  // Trains: create
  trainForm?.addEventListener("submit", async (e) => {
    e.preventDefault();
    clearText("trainError");
    const msg = document.getElementById("trainMessage");

    const name = trainName.value.trim();
    if (!name) return;

    try {
      await createTrain({ name, user_id: currentUser?.id });
      trainForm.reset();
      if (msg) {
        msg.textContent = "Vonat hozzáadva!";
        msg.style.display = "block";
      }
      await loadTrains();
    } catch (err) {
      setText("trainError", err.message);
    }
  });

  // Downloads: list
  async function loadDownloads() {
    if (!downloadList) return;
    downloadList.innerHTML = "";

    try {
      const files = await getDownloads();
      if (!Array.isArray(files) || files.length === 0) {
        const li = document.createElement("li");
        li.textContent = "Nincs elérhető letöltés.";
        downloadList.appendChild(li);
        return;
      }

      files.forEach((f) => {
        const li = document.createElement("li");
        const name = f.name || "Letöltés";
        const url = f.url || f.link || null;

        if (url) {
          const a = document.createElement("a");
          a.href = url;
          a.textContent = name;
          a.target = "_blank";
          a.rel = "noopener";
          li.appendChild(a);
        } else {
          li.textContent = name;
        }
        downloadList.appendChild(li);
      });
    } catch (err) {
      const li = document.createElement("li");
      li.textContent = `Hiba: ${err.message}`;
      downloadList.appendChild(li);
    }
  }

  // Downloads: button -> első url megnyitása, ha van
  downloadBtn?.addEventListener("click", async () => {
    if (!downloadMessage) return;
    downloadMessage.style.display = "none";

    try {
      const files = await getDownloads();

      if (!Array.isArray(files) || files.length === 0) {
        downloadMessage.textContent = "Nincs elérhető letöltés.";
        downloadMessage.style.display = "block";
        return;
      }

      const first = files[0];
      const url = first.url || first.link;

      if (!url) {
        downloadMessage.textContent = "A letöltéshez nincs URL megadva az adatbázisban.";
        downloadMessage.style.display = "block";
        return;
      }

      downloadMessage.textContent = "Letöltés megnyitása...";
      downloadMessage.style.display = "block";
      window.open(url, "_blank");
    } catch (err) {
      downloadMessage.textContent = `Hiba: ${err.message}`;
      downloadMessage.style.display = "block";
    }
  });

  // Contacts: POST
  contactForm?.addEventListener("submit", async (e) => {
    e.preventDefault();
    clearText("contactError");
    const ok = document.getElementById("formMessage");
    if (ok) ok.style.display = "none";

    const name = document.getElementById("contactName").value.trim();
    const email = document.getElementById("contactEmail").value.trim();
    const message = document.getElementById("contactMessage").value.trim();

    try {
      await createContact({ name, email, message });
      if (ok) ok.style.display = "block";
      contactForm.reset();
      await loadContacts();
    } catch (err) {
      setText("contactError", err.message);
    }
  });

  // Contacts: GET list
  async function loadContacts() {
    if (!contactList) return;
    contactList.innerHTML = "";

    try {
      const contacts = await getContacts();
      if (!Array.isArray(contacts) || contacts.length === 0) {
        const li = document.createElement("li");
        li.textContent = "Nincs még üzenet.";
        contactList.appendChild(li);
        return;
      }

      contacts.slice(-10).reverse().forEach((c) => {
        const li = document.createElement("li");
        li.textContent = `${c.name} (${c.email}): ${c.message}`;
        contactList.appendChild(li);
      });
    } catch (err) {
      const li = document.createElement("li");
      li.textContent = `Hiba: ${err.message}`;
      contactList.appendChild(li);
    }
  }

  // Induláskor: letöltések + contacts betöltés
  await loadDownloads();
  await loadContacts();

  // Token ellenőrzés / me()
  try {
    const user = await me();
    currentUser = user;
    usernameDisplay.textContent = user?.username || "";
    setLoggedInUI(true);
    await loadTrains();
  } catch {
    // nincs bejelentkezve (vagy lejárt token)
    setLoggedInUI(false);
  }
});