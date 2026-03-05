class ImageCarousel {

    constructor(){
    
    this.images = [
    "images/train1.jpg",
    "images/train2.jpg",
    "images/train3.jpg"
    ];
    
    this.index = 0;
    
    this.imageElement = document.getElementById("carouselImage");
    
    document
    .getElementById("prevBtn")
    .addEventListener("click",()=>this.prev());
    
    document
    .getElementById("nextBtn")
    .addEventListener("click",()=>this.next());
    
    }
    
    update(){
    
    this.imageElement.src = this.images[this.index];
    
    }
    
    next(){
    
    this.index++;
    
    if(this.index >= this.images.length){
    this.index = 0;
    }
    
    this.update();
    
    }
    
    prev(){
    
    this.index--;
    
    if(this.index < 0){
    this.index = this.images.length - 1;
    }
    
    this.update();
    
    }
    
    }
    
    async function loadFiles(){
    
    try{
    
    const response = await fetch("http://localhost:3000/api/downloads");
    
    const data = await response.json();
    
    const list = document.getElementById("fileList");
    
    list.innerHTML="";
    
    data.forEach(file=>{
    
    const li = document.createElement("li");
    
    li.textContent = file.name;
    
    list.appendChild(li);
    
    });
    
    }
    catch(error){
    
    console.error("Hiba a lista betöltésekor",error);
    
    }
    
    }
    
    async function downloadFile(){
    
    try{
    
    const response = await fetch("http://localhost:3000/api/download");
    
    const blob = await response.blob();
    
    const url = window.URL.createObjectURL(blob);
    
    const a = document.createElement("a");
    
    a.href = url;
    
    a.download = "file.zip";
    
    a.click();
    
    }
    catch(error){
    
    console.error("Letöltési hiba",error);
    
    }
    
    }
    
    async function sendForm(event){
    
    event.preventDefault();
    
    const name = document.getElementById("contactName").value;
    const email = document.getElementById("contactEmail").value;
    const message = document.getElementById("contactMessage").value;
    
    try{
    
    const response = await fetch("http://localhost:3000/api/contact",{
    
    method:"POST",
    
    headers:{
    "Content-Type":"application/json"
    },
    
    body:JSON.stringify({
    name,
    email,
    message
    })
    
    });
    
    const data = await response.json();
    
    document.getElementById("formMessage").style.display="block";
    
    document.getElementById("contactForm").reset();
    
    }
    catch(error){
    
    console.error("Küldési hiba",error);
    
    }
    
    }
    
    document.addEventListener("DOMContentLoaded",()=>{
    
    new ImageCarousel();
    
    loadFiles();
    
    document
    .getElementById("downloadBtn")
    .addEventListener("click",downloadFile);
    
    document
    .getElementById("contactForm")
    .addEventListener("submit",sendForm);
    
    });
    document.addEventListener('DOMContentLoaded', () => {
        const loginBtn = document.getElementById('loginBtn');
        const registerBtn = document.getElementById('registerBtn');
        const logoutBtn = document.getElementById('logoutBtn');
    
        const loginForm = document.getElementById('loginForm');
        const registerForm = document.getElementById('registerForm');
        const userInfo = document.getElementById('userInfo');
        const usernameDisplay = document.getElementById('usernameDisplay');
    
        // Form toggles
        loginBtn.addEventListener('click', () => {
            loginForm.style.display = 'block';
            registerForm.style.display = 'none';
        });
        registerBtn.addEventListener('click', () => {
            registerForm.style.display = 'block';
            loginForm.style.display = 'none';
        });
        logoutBtn.addEventListener('click', async () => {
            await fetch('/api/logout', { method: 'POST' });
            location.reload();
        });
    
        // Regisztráció
        registerForm.querySelector('form').addEventListener('submit', async (e) => {
            e.preventDefault();
            const username = document.getElementById('registerUsername').value;
            const password = document.getElementById('registerPassword').value;
            const res = await fetch('/api/register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username, password })
            });
            const data = await res.json();
            if (res.ok) {
                alert('Sikeres regisztráció! Jelentkezz be.');
                registerForm.style.display = 'none';
            } else {
                document.getElementById('registerError').innerText = data.error;
            }
        });
    
        // Bejelentkezés
        loginForm.querySelector('form').addEventListener('submit', async (e) => {
            e.preventDefault();
            const username = document.getElementById('loginUsername').value;
            const password = document.getElementById('loginPassword').value;
            const res = await fetch('/api/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username, password })
            });
            const data = await res.json();
            if (res.ok) {
                loginForm.style.display = 'none';
                loginBtn.style.display = 'none';
                registerBtn.style.display = 'none';
                logoutBtn.style.display = 'inline-block';
                userInfo.style.display = 'block';
                usernameDisplay.innerText = username;
            } else {
                document.getElementById('loginError').innerText = data.error;
            }
        });
    
        // Ellenőrizzük, ha a user már be van jelentkezve
        fetch('/api/me')
            .then(res => res.json())
            .then(data => {
                if (!data.error) {
                    loginBtn.style.display = 'none';
                    registerBtn.style.display = 'none';
                    logoutBtn.style.display = 'inline-block';
                    userInfo.style.display = 'block';
                    usernameDisplay.innerText = data.username;
                }
            });
    });
    export function initCarousel() {
  const carousel = document.getElementById("carousel");
  const images = Array.from(carousel.querySelectorAll("img"));
  const prevBtn = document.getElementById("prevBtn");
  const nextBtn = document.getElementById("nextBtn");
  const indicators = document.getElementById("indicators");

  if (!carousel || images.length === 0) return;

  let index = 0;

  // Indikátorok
  indicators.innerHTML = "";
  const dots = images.map((_, i) => {
    const dot = document.createElement("button");
    dot.className = "dot";
    dot.type = "button";
    dot.setAttribute("aria-label", `Kép ${i + 1}`);
    dot.addEventListener("click", () => {
      index = i;
      update();
    });
    indicators.appendChild(dot);
    return dot;
  });

  function update() {
    carousel.style.transform = `translateX(${-index * 100}%)`;
    dots.forEach((d, i) => d.classList.toggle("active", i === index));
  }

  prevBtn?.addEventListener("click", () => {
    index = (index - 1 + images.length) % images.length;
    update();
  });

  nextBtn?.addEventListener("click", () => {
    index = (index + 1) % images.length;
    update();
  });

  // indulás
  update();
}
