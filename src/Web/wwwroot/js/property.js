/* =============================================================
   property.js — Property detail page interactivity
   ============================================================= */

const listing = window._propListing || {};

/* ── Helpers ────────────────────────────────────────────────── */
function q(sel)  { return document.querySelector(sel); }
function qq(sel) { return document.querySelectorAll(sel); }

/* ── Nearby places per city ─────────────────────────────────── */
const NEARBY = {
    'Львів': [
        { name: 'Львівська філармонія',                  dist: '3 хв пішки' },
        { name: 'Ринкова площа',                         dist: '7 хв пішки' },
        { name: 'Львівський театр опери та балету',       dist: '12 хв пішки' },
        { name: 'Міжнародний аеропорт Львів',            dist: '25 хв авто' },
    ],
    'Київ': [
        { name: 'Майдан Незалежності',                   dist: '5 хв пішки' },
        { name: 'Хрещатик',                              dist: '3 хв пішки' },
        { name: 'Золоті ворота',                         dist: '10 хв пішки' },
        { name: 'Аеропорт Бориспіль',                    dist: '40 хв авто' },
    ],
    'Одеса': [
        { name: 'Дерибасівська вулиця',                  dist: '2 хв пішки' },
        { name: 'Потьомкінські сходи',                   dist: '8 хв пішки' },
        { name: 'Одеський театр опери та балету',         dist: '5 хв пішки' },
        { name: 'Аеропорт Одеса',                        dist: '20 хв авто' },
    ],
    'Буковель': [
        { name: 'Гірськолижний підйомник',               dist: '5 хв пішки' },
        { name: 'Озеро Буковель',                        dist: '10 хв пішки' },
        { name: 'Траса "Чорногора"',                     dist: '8 хв пішки' },
        { name: 'Аеропорт Івано-Франківськ',             dist: '80 хв авто' },
    ],
};

const SVG_PIN = `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
    <path d="M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7z"/>
    <circle cx="12" cy="9" r="2.5"/></svg>`;

function renderNearby() {
    const list = q('#propNearbyList');
    if (!list) return;
    const city = listing.city || 'Львів';
    const places = NEARBY[city] || NEARBY['Львів'];
    list.innerHTML = places.map(p => `
        <li>
            <span class="pnl-name">${SVG_PIN}${p.name}</span>
            <span class="pnl-dist">${p.dist}</span>
        </li>`).join('');
}

/* ── Mini map (Leaflet in the card) ─────────────────────────── */
const HOTEL_COORDS = {
    1:  [49.8094, 23.8843],
    2:  [49.8418, 24.0315],
    3:  [49.8395, 24.0207],
    4:  [49.8368, 24.0291],
    5:  [49.8423, 24.0213],
    6:  [49.8441, 24.0288],
    7:  [49.8411, 24.0180],
    8:  [50.4475, 30.5219],
    9:  [46.4851, 30.7322],
    10: [48.3598, 24.3890],
};
const CITY_CENTER = {
    'Львів':    [49.8397, 24.0297],
    'Київ':     [50.4501, 30.5234],
    'Одеса':    [46.4825, 30.7233],
    'Буковель': [48.3598, 24.3890],
};

let miniMap  = null;
let fullMap  = null;

function initMiniMap() {
    const el = q('#propMapPreview');
    if (!el || typeof L === 'undefined') return;

    const city   = listing.city || 'Львів';
    const coords = HOTEL_COORDS[listing.id] || CITY_CENTER[city] || CITY_CENTER['Львів'];

    miniMap = L.map('propMapPreview', {
        center: coords,
        zoom: 14,
        zoomControl: false,
        scrollWheelZoom: false,
        dragging: false,
        attributionControl: false,
    });

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 18,
    }).addTo(miniMap);

    const icon = L.divIcon({
        className: 'mini-map-marker',
        html: `<div style="
            background:#223300; color:#FFFFEE;
            padding:5px 10px; border-radius:16px;
            font-family:Rubik,sans-serif; font-size:13px; font-weight:700;
            white-space:nowrap; box-shadow:0 2px 6px rgba(0,0,0,.3);
            position:relative;">
            UAH ${Number(listing.pricePerNight).toLocaleString('uk-UA')}
            <span style="position:absolute;bottom:-7px;left:50%;transform:translateX(-50%);
                border:7px solid transparent;border-top:7px solid #223300;border-bottom:none;">
            </span></div>`,
        iconSize: [120, 36],
        iconAnchor: [60, 43],
    });
    L.marker(coords, { icon, title: listing.name }).addTo(miniMap);
}

/* ── Full map modal ──────────────────────────────────────────── */
function openMapModal() {
    const modal = q('#propMapModal');
    if (!modal) return;
    modal.style.display = 'flex';
    document.body.style.overflow = 'hidden';

    if (!fullMap) {
        setTimeout(() => {
            const city   = listing.city || 'Львів';
            const coords = HOTEL_COORDS[listing.id] || CITY_CENTER[city] || CITY_CENTER['Львів'];

            fullMap = L.map('propLeafletMap', { center: coords, zoom: 15 });
            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '© <a href="https://openstreetmap.org/copyright">OpenStreetMap</a> contributors',
                maxZoom: 18,
            }).addTo(fullMap);

            const icon = L.divIcon({
                className: '',
                html: `<div style="
                    background:#223300;color:#FFFFEE;
                    padding:6px 14px;border-radius:20px;
                    font-family:Rubik,sans-serif;font-size:14px;font-weight:700;
                    white-space:nowrap;box-shadow:0 2px 8px rgba(0,0,0,.3);position:relative;">
                    UAH ${Number(listing.pricePerNight).toLocaleString('uk-UA')}
                    <span style="position:absolute;bottom:-7px;left:50%;transform:translateX(-50%);
                        border:7px solid transparent;border-top:7px solid #223300;border-bottom:none;"></span>
                    </div>`,
                iconSize: [140, 38],
                iconAnchor: [70, 45],
            });

            const marker = L.marker(coords, { icon, title: listing.name }).addTo(fullMap);
            marker.bindPopup(`
                <div style="padding:14px;min-width:200px;font-family:Rubik,sans-serif;">
                    <div style="font-size:15px;font-weight:700;color:#223300;margin-bottom:4px;">${listing.name}</div>
                    <div style="font-size:12px;color:#777;margin-bottom:8px;">${listing.location}</div>
                    <div style="font-size:15px;font-weight:700;color:#0C1200;">
                        UAH ${Number(listing.pricePerNight).toLocaleString('uk-UA')}
                    </div>
                </div>`, { maxWidth: 260 }).openPopup();

            fullMap.invalidateSize();
        }, 80);
    } else {
        fullMap.invalidateSize();
    }
}

function closeMapModal() {
    const modal = q('#propMapModal');
    if (modal) modal.style.display = 'none';
    document.body.style.overflow = '';
}

/* ── Favourite button ────────────────────────────────────────── */
function initFavBtn() {
    const btn = q('#propFavBtn');
    if (!btn) return;
    btn.addEventListener('click', () => {
        btn.classList.toggle('active');
        const heart = btn.querySelector('path');
        if (btn.classList.contains('active')) {
            heart.setAttribute('fill', '#fff');
            heart.setAttribute('stroke', '#fff');
        } else {
            heart.setAttribute('fill', 'none');
            heart.setAttribute('stroke', '#fff');
        }
    });
}

/* ── Photos button ───────────────────────────────────────────── */
function initPhotosBtn() {
    const btn = q('#propPhotosBtn');
    if (!btn) return;
    btn.addEventListener('click', () => {
        // Scroll to gallery or open lightbox placeholder
        const gallery = q('#propGallery');
        if (gallery) gallery.scrollIntoView({ behavior: 'smooth' });
    });
}

/* ── Room type filter tabs ───────────────────────────────────── */
function initRoomTabs() {
    const tabs  = qq('.prn-tab');
    const cards = qq('.room-card');
    const count = q('.prn-count');

    tabs.forEach(tab => {
        tab.addEventListener('click', () => {
            tabs.forEach(t => t.classList.remove('active'));
            tab.classList.add('active');

            const val = tab.dataset.tab;
            let visible = 0;
            cards.forEach(card => {
                if (val === 'all' || card.dataset.beds === val) {
                    card.style.display = '';
                    visible++;
                } else {
                    card.style.display = 'none';
                }
            });
            if (count) count.textContent = `Показано ${visible} з ${cards.length} номерів`;
        });
    });
}

/* ── Book buttons ────────────────────────────────────────────── */
function initBookBtns() {
    qq('.rc-book-btn').forEach(btn => {
        btn.addEventListener('click', () => {
            // Find selected extras
            const card     = btn.closest('.room-card');
            const roomName = card?.querySelector('.rc-name')?.textContent || 'Номер';
            const checked  = Array.from(card?.querySelectorAll('.rc-cb:checked') || [])
                .map(cb => cb.closest('.rc-extra-opt')?.querySelector('span:nth-child(3)')?.textContent);

            let msg = `Бронювання: "${roomName}"\nГотель: ${listing.name}`;
            if (checked.length) msg += `\nДодатково: ${checked.join(', ')}`;
            msg += '\n\n(Функція бронювання буде реалізована після підключення бекенду)';
            alert(msg);
        });
    });
}

/* ── Rooms search buttons ────────────────────────────────────── */
function initRoomsSearch() {
    const btn = q('.prs-search-btn');
    if (btn) {
        btn.addEventListener('click', () => {
            // Reload page with new params (placeholder)
            alert('Пошук доступних кімнат за обраними датами (буде реалізовано)');
        });
    }
}

/* ── "Обрати кімнату" smooth scroll ─────────────────────────── */
function initChooseRoomBtn() {
    document.querySelectorAll('.btn-choose-room, a[href="#rooms"]').forEach(btn => {
        btn.addEventListener('click', e => {
            e.preventDefault();
            const target = q('#rooms');
            if (target) target.scrollIntoView({ behavior: 'smooth', block: 'start' });
        });
    });
}

/* ── DOMContentLoaded ────────────────────────────────────────── */
document.addEventListener('DOMContentLoaded', () => {
    renderNearby();
    initMiniMap();
    initFavBtn();
    initPhotosBtn();
    initRoomTabs();
    initBookBtns();
    initRoomsSearch();
    initChooseRoomBtn();

    /* Map button */
    const mapBtn = q('#propMapBtn');
    if (mapBtn) mapBtn.addEventListener('click', openMapModal);

    /* Close map */
    const closeBtn = q('#propCloseMapBtn');
    if (closeBtn) closeBtn.addEventListener('click', closeMapModal);

    /* ESC closes map */
    document.addEventListener('keydown', e => {
        if (e.key === 'Escape') closeMapModal();
    });
});
