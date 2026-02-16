class ImageCarousel {
    constructor() {
        this.carousel = document.getElementById('carousel');
        this.prevBtn = document.getElementById('prevBtn');
        this.nextBtn = document.getElementById('nextBtn');
        this.indicatorsContainer = document.getElementById('indicators');
        this.images = document.querySelectorAll('.carousel-image');

        this.currentIndex = 0;
        this.imageWidth = this.images[0].offsetWidth + 20; // gap miatt

        this.init();
    }

    init() {
        this.createIndicators();
        this.updateIndicators();
        this.bindEvents();
    }

    createIndicators() {
        this.indicatorsContainer.innerHTML = '';
        this.images.forEach((_, index) => {
            const indicator = document.createElement('span');
            indicator.classList.add('indicator');
            indicator.addEventListener('click', () => {
                this.scrollToIndex(index);
            });
            this.indicatorsContainer.appendChild(indicator);
        });
    }

    bindEvents() {
        this.prevBtn.addEventListener('click', () => this.prev());
        this.nextBtn.addEventListener('click', () => this.next());

        // billentyűzet
        document.addEventListener('keydown', (e) => {
            if (e.key === 'ArrowLeft') this.prev();
            if (e.key === 'ArrowRight') this.next();
        });

        // swipe
        let startX = 0;
        this.carousel.addEventListener('touchstart', (e) => {
            startX = e.touches[0].clientX;
        });

        this.carousel.addEventListener('touchend', (e) => {
            const endX = e.changedTouches[0].clientX;
            if (startX - endX > 50) this.next();
            if (endX - startX > 50) this.prev();
        });
    }

    scrollToIndex(index) {
        this.carousel.scrollTo({
            left: index * this.imageWidth,
            behavior: 'smooth'
        });
        this.currentIndex = index;
        this.updateIndicators();
    }

    next() {
        if (this.currentIndex < this.images.length - 1) {
            this.currentIndex++;
        } else {
            this.currentIndex = 0;
        }
        this.scrollToIndex(this.currentIndex);
    }

    prev() {
        if (this.currentIndex > 0) {
            this.currentIndex--;
        } else {
            this.currentIndex = this.images.length - 1;
        }
        this.scrollToIndex(this.currentIndex);
    }

    updateIndicators() {
        const indicators = this.indicatorsContainer.querySelectorAll('.indicator');
        indicators.forEach((indicator, index) => {
            indicator.classList.toggle('active', index === this.currentIndex);
        });
    }
}

// DOM betöltés után indul
document.addEventListener('DOMContentLoaded', () => {
    new ImageCarousel();
    checkLoggedIn();
    showAdminPanelIfAllowed();
    
});

// ======================
// ======================
//  REGISZTRÁCIÓ + LOGIN (NINCS ADMIN)
// ======================

function openAuthModal(mode) {
    const modal = document.getElementById("authModal");
    const title = document.getElementById("modalTitle");
    const submitBtn = document.getElementById("submitBtn");
    const message = document.getElementById("authMessage");

    message.style.display = "none";
    message.className = "auth-message";

    if (mode === "register") {
        title.textContent = "Regisztráció";
        submitBtn.textContent = "Regisztrálok";
    } else {
        title.textContent = "Bejelentkezés";
        submitBtn.textContent = "Belépés";
    }

    modal.style.display = "flex";
    document.getElementById("inpUsername").value = "";
    document.getElementById("inpPassword").value = "";
}

function closeAuthModal() {
    document.getElementById("authModal").style.display = "none";
}

function showAuthMessage(text, type = "success") {
    const msg = document.getElementById("authMessage");
    msg.textContent = text;
    msg.className = `auth-message ${type}`;
    msg.style.display = "block";
}

function getUsers() {
    return JSON.parse(localStorage.getItem("minitrains_users") || "[]");
}

function saveUsers(users) {
    localStorage.setItem("minitrains_users", JSON.stringify(users));
}

function setLoggedInUser(username) {
    localStorage.setItem("minitrains_current_user", username);

    document.getElementById("btnLogin").style.display = "none";
    document.getElementById("btnRegister").style.display = "none";
    
    document.getElementById("currentUserDisplay").style.display = "inline";
    document.getElementById("currentUserName").textContent = username;
}

function logout() {
    localStorage.removeItem("minitrains_current_user");

    document.getElementById("btnLogin").style.display = "";
    document.getElementById("btnRegister").style.display = "";
    document.getElementById("currentUserDisplay").style.display = "none";

    showAuthMessage("Kijelentkeztél", "success");
}

function checkLoggedIn() {
    const user = localStorage.getItem("minitrains_current_user");
    if (user) {
        setLoggedInUser(user);
    }
}

// FORM KEZELÉS
document.getElementById("authForm").addEventListener("submit", function(e) {
    e.preventDefault();

    const username = document.getElementById("inpUsername").value.trim();
    const password = document.getElementById("inpPassword").value;

    let users = getUsers();

    const mode = document.getElementById("modalTitle").textContent;

    if (mode === "Regisztráció") {
        if (!username) return showAuthMessage("Felhasználónév kötelező!", "error");
        if (password.length < 5) return showAuthMessage("Jelszó min. 5 karakter!", "error");
        if (users.some(u => u.username === username)) {
            return showAuthMessage("Ez a név már foglalt!", "error");
        }

        users.push({ username, password });
        saveUsers(users);
        showAuthMessage("Sikeres regisztráció! Most beléphetsz.", "success");
        setTimeout(() => openAuthModal("login"), 1400);
    }
    else if (mode === "Bejelentkezés") {
        const found = users.find(u => u.username === username && u.password === password);
        if (found) {
            setLoggedInUser(username);
            showAuthMessage(`Üdv újra, ${username}!`, "success");
            setTimeout(closeAuthModal, 1200);
        } else {
            showAuthMessage("Hibás felhasználónév vagy jelszó", "error");
        }
    }
});

// Inicializálás (a carousel mellé)
document.addEventListener("DOMContentLoaded", () => {
    checkLoggedIn();
    // ... a meglévő ImageCarousel inicializálásod marad ...
});
// -------------------------------
// ADMIN + CRUD rész
// -------------------------------

const ADMIN_USERNAMES = ["admin", "Admin123", "gazda"]; // bővíthető

function isAdmin() {
    const user = localStorage.getItem("minitrains_current_user");
    return user && ADMIN_USERNAMES.includes(user);
}

function showAdminPanelIfAllowed() {
    const panel = document.getElementById("admin-panel");
    if (!panel) return;
    
    if (isAdmin()) {
        panel.style.display = "block";
        loadAdminItems();
    } else {
        panel.style.display = "none";
    }
}

// Tárolás
function getItems() {
    return JSON.parse(localStorage.getItem("minitrains_items") || "[]");
}

function saveItems(items) {
    localStorage.setItem("minitrains_items", JSON.stringify(items));
}

// Lista megjelenítése adminnak
function loadAdminItems() {
    const container = document.getElementById("adminItemsList");
    if (!container) return;
    
    container.innerHTML = "";
    const items = getItems();
    
    items.forEach(item => {
        const card = document.createElement("div");
        card.className = "admin-card";
        card.style.border = "1px solid #c3d9ff";
        card.style.borderRadius = "12px";
        card.style.overflow = "hidden";
        card.style.background = "white";
        card.style.boxShadow = "0 4px 12px rgba(0,0,0,0.1)";
        
        card.innerHTML = `
            <img src="${item.imageUrl}" alt="${item.title}" style="width:100%; height:180px; object-fit:cover;">
            <div style="padding:16px;">
                <h4>${item.title}</h4>
                <p style="color:#555; font-size:0.95rem;">${item.description.substring(0,80)}${item.description.length > 80 ? '...' : ''}</p>
                <div style="margin-top:12px; display:flex; gap:8px;">
                    <button class="btn-primary edit-btn" data-id="${item.id}">Szerkesztés</button>
                    <button class="btn-danger delete-btn" data-id="${item.id}">Törlés</button>
                </div>
            </div>
        `;
        
        container.appendChild(card);
    });
    
    // Eseménykezelők
    container.querySelectorAll(".edit-btn").forEach(btn => {
        btn.addEventListener("click", () => editItem(btn.dataset.id));
    });
    
    container.querySelectorAll(".delete-btn").forEach(btn => {
        btn.addEventListener("click", () => {
            if (confirm("Biztosan törlöd ezt a bejegyzést?")) {
                deleteItem(btn.dataset.id);
            }
        });
    });
}

function generateId() {
    return "mt-" + Date.now() + "-" + Math.floor(Math.random()*10000);
}

function addOrUpdateItem(isEdit = false, editId = null) {
    const title = document.getElementById("itemTitle").value.trim();
    const desc  = document.getElementById("itemDesc").value.trim();
    const url   = document.getElementById("itemImageUrl").value.trim();
    const cat   = document.getElementById("itemCategory").value;

    if (!title || !url) {
        alert("Cím és kép URL kötelező!");
        return;
    }

    let items = getItems();

    if (isEdit && editId) {
        // update
        items = items.map(it => 
            it.id === editId 
                ? { ...it, title, description: desc, imageUrl: url, category: cat }
                : it
        );
        showAuthMessage("Sikeresen módosítva!", "success");
    } else {
        // create
        items.push({
            id: generateId(),
            title,
            description: desc,
            imageUrl: url,
            category: cat,
            author: localStorage.getItem("minitrains_current_user"),
            createdAt: new Date().toISOString()
        });
        showAuthMessage("Sikeresen hozzáadva!", "success");
    }

    saveItems(items);
    loadAdminItems();
    document.getElementById("itemForm").reset();
    document.getElementById("cancelEdit").style.display = "none";
    // ha szerkesztés volt, visszaállítjuk a gombot hozzáadásra
    document.querySelector("#itemForm button[type='submit']").textContent = "Hozzáadás";
}

function editItem(id) {
    const items = getItems();
    const item = items.find(i => i.id === id);
    if (!item) return;

    document.getElementById("itemTitle").value = item.title;
    document.getElementById("itemDesc").value  = item.description;
    document.getElementById("itemImageUrl").value = item.imageUrl;
    document.getElementById("itemCategory").value = item.category;

    document.querySelector("#itemForm button[type='submit']").textContent = "Módosítás mentése";
    document.getElementById("cancelEdit").style.display = "inline-block";

    // ideiglenesen eltároljuk, melyik ID-t szerkesztjük
    document.getElementById("itemForm").dataset.editId = id;
}

function deleteItem(id) {
    let items = getItems();
    items = items.filter(i => i.id !== id);
    saveItems(items);
    loadAdminItems();
    showAuthMessage("Törölve!", "success");
}

// Form submit
document.getElementById("itemForm")?.addEventListener("submit", e => {
    e.preventDefault();
    const isEdit = !!document.getElementById("itemForm").dataset.editId;
    const editId = document.getElementById("itemForm").dataset.editId || null;
    
    addOrUpdateItem(isEdit, editId);
    
    // reset edit mód
    delete document.getElementById("itemForm").dataset.editId;
});
// Mégse gomb
document.getElementById("cancelEdit")?.addEventListener("click", () => {
    document.getElementById("itemForm").reset();
    document.getElementById("cancelEdit").style.display = "none";
    document.querySelector("#itemForm button[type='submit']").textContent = "Hozzáadás";
    delete document.getElementById("itemForm").dataset.editId;
});
showAdminPanelIfAllowed();

