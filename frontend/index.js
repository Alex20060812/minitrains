// ======================
// IMAGE CAROUSEL
// ======================
class ImageCarousel {
    constructor() {
        this.carousel = document.getElementById('carousel');
        this.prevBtn = document.getElementById('prevBtn');
        this.nextBtn = document.getElementById('nextBtn');
        this.indicatorsContainer = document.getElementById('indicators');
        this.images = document.querySelectorAll('.carousel-image');

        if (!this.carousel || this.images.length === 0) return;

        this.currentIndex = 0;
        this.imageWidth = this.images[0].offsetWidth + 20;
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
            indicator.addEventListener('click', () => this.scrollToIndex(index));
            this.indicatorsContainer.appendChild(indicator);
        });
    }

    bindEvents() {
        this.prevBtn?.addEventListener('click', () => this.prev());
        this.nextBtn?.addEventListener('click', () => this.next());

        document.addEventListener('keydown', e => {
            if (e.key === 'ArrowLeft') this.prev();
            if (e.key === 'ArrowRight') this.next();
        });

        let startX = 0;
        this.carousel.addEventListener('touchstart', e => {
            startX = e.touches[0].clientX;
        });

        this.carousel.addEventListener('touchend', e => {
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
        this.currentIndex = (this.currentIndex + 1) % this.images.length;
        this.scrollToIndex(this.currentIndex);
    }

    prev() {
        this.currentIndex =
            (this.currentIndex - 1 + this.images.length) % this.images.length;
        this.scrollToIndex(this.currentIndex);
    }

    updateIndicators() {
        const indicators = this.indicatorsContainer.querySelectorAll('.indicator');
        indicators.forEach((ind, i) => {
            ind.classList.toggle('active', i === this.currentIndex);
        });
    }
}

// ======================
// CONTACT FORM (EMAIL)
// ======================
document.getElementById("contactForm").addEventListener("submit", function(e){
    e.preventDefault();
    const name = document.getElementById("contactName").value.trim();
    const email = document.getElementById("contactEmail").value.trim();
    const message = document.getElementById("contactMessage").value.trim();

    if(!name || !email || !message) return alert("Kérlek tölts ki minden mezőt!");

    const subject = encodeURIComponent("MiniTrains üzenet: " + name);
    const body = encodeURIComponent(message + "\n\nEmail: " + email);
    window.location.href = `mailto:kovako775@logiker.hu?subject=${subject}&body=${body}`;

    const msg = document.getElementById("formMessage");
    msg.style.display = "block";
    this.reset();
});

// ======================
// DOWNLOAD BUTTON (FETCH API)
// ======================
document.getElementById("downloadBtn").addEventListener("click", async () => {
    const messageEl = document.getElementById("downloadMessage");
    messageEl.style.display = "block";
    try {
        const response = await fetch(''); // ide jön majd az API URL
        if (!response.ok) throw new Error("Hiba a letöltés során");

        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = "MiniTrains.zip"; // fájl név
        document.body.appendChild(a);
        a.click();
        a.remove();
        window.URL.revokeObjectURL(url);
        messageEl.textContent = "Fájl letöltése sikeres!";
    } catch (error) {
        console.error(error);
        messageEl.textContent = "Hiba történt a letöltés során.";
        messageEl.style.color = "red";
    }
});

// ======================
// INIT
// ======================
document.addEventListener("DOMContentLoaded", () => {
    new ImageCarousel();
});