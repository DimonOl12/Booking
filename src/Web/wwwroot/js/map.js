/* =============================================================
   map.js — Map modal with Leaflet.js
   Opens on "Перейти на карту", shows filters + hotel cards + map
   ============================================================= */

/* ── Approximate coordinates for mock hotels ─────────────────
   (Replace with real geodata from DB when available)
   ──────────────────────────────────────────────────────────── */
const HOTEL_COORDS = {
    1:  [49.8094, 23.8843], // Emily Resort (western Lviv)
    2:  [49.8418, 24.0315], // Rudolfo Hotel (Rynok Sq)
    3:  [49.8395, 24.0207], // Apart-HOTEL GALLERY 21
    4:  [49.8368, 24.0291], // Атлас Делюкс
    5:  [49.8423, 24.0213], // Cities Gallery Apart-hotel
    6:  [49.8441, 24.0288], // Jam Apartments Miskevycha
    7:  [49.8411, 24.0180], // 7 Apartments
    8:  [50.4475, 30.5219], // Premier Palace Kyiv
    9:  [46.4851, 30.7322], // Ribas Hotel Odesa
    10: [48.3598, 24.3890], // Буковель Хаус Шале
};

/* Map center per city */
const CITY_CENTER = {
    'Львів':    [49.8397, 24.0297],
    'Київ':     [50.4501, 30.5234],
    'Одеса':    [46.4825, 30.7233],
    'Буковель': [48.3598, 24.3890],
};

/* ── Helpers ─────────────────────────────────────────────────── */
function q(s)  { return document.querySelector(s); }
function qq(s) { return document.querySelectorAll(s); }

function fmtPrice(n) {
    return Number(n).toLocaleString('uk-UA');
}

/* ── Map state ───────────────────────────────────────────────── */
let leafletMap   = null;
let leafletMarkers = {}; // id → { marker, iconEl }
let activeMarkerId = null;

const mapFilters = {
    minPrice: 400,
    maxPrice: 6500,
    types: [],
    minRating: 0,
};

/* ── Modal open / close ──────────────────────────────────────── */
function openMapModal() {
    const modal = q('#mapModal');
    modal.style.display = 'flex';
    document.body.style.overflow = 'hidden';

    // Init map once
    if (!leafletMap) {
        setTimeout(initLeafletMap, 60); // wait for layout
    } else {
        leafletMap.invalidateSize();
    }

    // Render initial hotel list
    const listings = getMapFilteredListings();
    renderMapCards(listings);
    if (leafletMap) {
        placeMarkers(listings);
    }
}

function closeMapModal() {
    q('#mapModal').style.display = 'none';
    document.body.style.overflow = '';
}

/* ── Leaflet initialisation ──────────────────────────────────── */
function initLeafletMap() {
    const allData = window._allListings || [];
    const city = (window._searchParams && window._searchParams.city) || 'Львів';
    const center = CITY_CENTER[city] || CITY_CENTER['Львів'];

    leafletMap = L.map('leafletMap', {
        center: center,
        zoom: 13,
        zoomControl: true,
    });

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© <a href="https://openstreetmap.org/copyright">OpenStreetMap</a> contributors',
        maxZoom: 18,
    }).addTo(leafletMap);

    const listings = getMapFilteredListings();
    placeMarkers(listings);
    leafletMap.invalidateSize();
}

/* ── Marker factory ──────────────────────────────────────────── */
function createPriceIcon(item, isActive) {
    const cls = isActive ? 'mpm-active' : '';
    return L.divIcon({
        className: `map-price-marker ${cls}`,
        html: `<div class="mpm-inner" data-marker-id="${item.id}">UAH ${fmtPrice(item.pricePerNight)}</div>`,
        iconSize:   [110, 34],
        iconAnchor: [55, 41],
        popupAnchor: [0, -42],
    });
}

function placeMarkers(listings) {
    if (!leafletMap) return;

    // Remove old markers
    Object.values(leafletMarkers).forEach(m => m.remove());
    leafletMarkers = {};

    listings.forEach(item => {
        const coords = HOTEL_COORDS[item.id];
        if (!coords) return;

        const marker = L.marker(coords, {
            icon: createPriceIcon(item, false),
            title: item.name,
        }).addTo(leafletMap);

        marker.on('click', () => onMarkerClick(item, marker));

        leafletMarkers[item.id] = marker;
    });
}

/* ── Marker click ────────────────────────────────────────────── */
function onMarkerClick(item, marker) {
    setActiveMarker(item.id);

    marker.bindPopup(buildPopupHtml(item), {
        maxWidth: 260,
        closeButton: true,
    }).openPopup();

    // Scroll matching card into view
    const card = q(`#mapCardsList [data-id="${item.id}"]`);
    if (card) {
        card.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
        highlightCard(item.id);
    }
}

function setActiveMarker(id) {
    // Deactivate previous
    if (activeMarkerId && leafletMarkers[activeMarkerId]) {
        const prev = leafletMarkers[activeMarkerId];
        const allData = window._allListings || [];
        const prevItem = allData.find(l => l.id === activeMarkerId);
        if (prevItem) prev.setIcon(createPriceIcon(prevItem, false));
        const prevEl = q(`.mpm-inner[data-marker-id="${activeMarkerId}"]`);
        if (prevEl) prevEl.closest('.map-price-marker')?.classList.remove('mpm-active');
    }

    activeMarkerId = id;

    if (id && leafletMarkers[id]) {
        const allData = window._allListings || [];
        const item = allData.find(l => l.id === id);
        if (item) leafletMarkers[id].setIcon(createPriceIcon(item, true));
    }
}

/* ── Popup HTML ─────────────────────────────────────────────── */
function buildPopupHtml(item) {
    return `<div class="map-popup">
        <div class="map-popup-name">${item.name}</div>
        <div class="map-popup-loc">${item.location}</div>
        <div class="map-popup-price">UAH ${fmtPrice(item.pricePerNight)}</div>
        <button class="map-popup-btn" onclick="window.location.href='/Home/Property/${item.id}'">Переглянути</button>
    </div>`;
}

/* ── Card highlight ─────────────────────────────────────────── */
function highlightCard(id) {
    qq('#mapCardsList .listing-card').forEach(c => c.classList.remove('map-card-active'));
    const card = q(`#mapCardsList [data-id="${id}"]`);
    if (card) card.classList.add('map-card-active');
}

/* ── Map card renderer ───────────────────────────────────────── */
function renderMapCards(listings) {
    const container = q('#mapCardsList');
    const titleEl   = q('.map-cards-title');

    if (titleEl) titleEl.textContent = `${listings.length} помешкань`;

    if (!listings.length) {
        container.innerHTML = '<div style="padding:20px;color:#777;text-align:center">Нічого не знайдено</div>';
        return;
    }

    container.innerHTML = listings.map(item => buildCardHtml(item)).join('');

    // Card click → pan map to marker
    container.querySelectorAll('.listing-card').forEach(card => {
        const id = parseInt(card.dataset.id, 10);
        card.addEventListener('click', () => {
            const coords = HOTEL_COORDS[id];
            if (coords && leafletMap) {
                leafletMap.setView(coords, 15, { animate: true });
            }
            setActiveMarker(id);
            highlightCard(id);

            const m = leafletMarkers[id];
            if (m) {
                const allData = window._allListings || [];
                const item = allData.find(l => l.id === id);
                if (item) {
                    m.bindPopup(buildPopupHtml(item), { maxWidth: 260, closeButton: true }).openPopup();
                }
            }
        });
    });

    // "Переглянути" buttons
    container.querySelectorAll('.btn-view').forEach(btn => {
        btn.addEventListener('click', e => {
            e.stopPropagation();
            window.location.href = `/Home/Property/${btn.dataset.id}`;
        });
    });
}

/* ── Shared card HTML builder (used by both main page & map) ─── */
function buildCardHtml(item) {
    const rating = item.ratingScore.toFixed(1).replace('.', ',');
    const amenities = buildAmenityBadges(item);
    const oldPrice  = item.originalPricePerNight
        ? `<span class="lc-price-old">UAH ${fmtPrice(item.originalPricePerNight)}</span>` : '';
    const urgency = item.roomsLeft <= 2
        ? `<div class="lc-urgency">Залишився ${item.roomsLeft} номер!</div>` : '';

    return `
<article class="listing-card" data-id="${item.id}" data-price="${item.pricePerNight}" data-rating="${item.ratingScore}" data-type="${item.type}">
  <div class="lc-img-wrap">
    <img src="${item.imageUrl}" alt="${item.name}" loading="lazy" />
  </div>
  <div class="lc-body">
    <div class="lc-rating-row">
      <div class="lc-badge">${rating}</div>
      <div class="lc-rating-meta">
        <span class="lc-rl">${item.ratingLabel}</span>
        <span class="lc-rr">${item.reviewsCount} відгуків</span>
      </div>
    </div>
    <div class="lc-name-loc">
      <h2 class="lc-name">${item.name}</h2>
      <p class="lc-location">${item.location}</p>
    </div>
    <div class="lc-amenities">${amenities}</div>
    <p class="lc-desc">${item.description}</p>
    ${urgency}
    <div class="lc-footer">
      <div class="lc-price-block">
        ${oldPrice}
        <span class="lc-price">UAH ${fmtPrice(item.pricePerNight)}</span>
        <span class="lc-price-meta">за 1 ніч</span>
      </div>
      <button class="btn-view" data-id="${item.id}">Переглянути</button>
    </div>
  </div>
</article>`;
}

function buildAmenityBadges(item) {
    const SVG_WIFI = `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><path d="M1.5 8.5c5.5-5.5 15.5-5.5 21 0"/><path d="M5 12c3.9-3.9 11-3.9 14 0"/><path d="M8.5 15.5c2.2-2.2 6.8-2.2 7 0"/><circle cx="12" cy="19" r="1" fill="currentColor"/></svg>`;
    const SVG_PARK = `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><rect x="3" y="3" width="18" height="18" rx="2"/><path d="M9 17V8h4a3 3 0 010 6H9"/></svg>`;
    const SVG_POOL = `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><path d="M2 12h20M2 17c1.5-1 3.5-1 5 0s3.5 1 5 0 3.5-1 5 0"/><circle cx="8" cy="7" r="3"/></svg>`;
    const SVG_BKFT = `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><path d="M18 8h1a4 4 0 010 8h-1"/><path d="M2 8h16v9a4 4 0 01-4 4H6a4 4 0 01-4-4V8z"/></svg>`;
    let html = '';
    if (item.hasFreeWifi)    html += `<span class="lc-amenity" title="Wi-Fi">${SVG_WIFI}<span class="lc-amenity-label">Wi-Fi</span></span>`;
    if (item.hasFreeParking) html += `<span class="lc-amenity" title="Парковка">${SVG_PARK}<span class="lc-amenity-label">Парковка</span></span>`;
    if (item.hasPool)        html += `<span class="lc-amenity" title="Басейн">${SVG_POOL}<span class="lc-amenity-label">Басейн</span></span>`;
    if (item.hasBreakfast)   html += `<span class="lc-amenity" title="Сніданок">${SVG_BKFT}<span class="lc-amenity-label">Сніданок</span></span>`;
    return html;
}

/* ── Map filter logic ────────────────────────────────────────── */
function getMapFilteredListings() {
    const all = window._allListings || [];
    return all.filter(item => {
        if (item.pricePerNight < mapFilters.minPrice) return false;
        if (item.pricePerNight > mapFilters.maxPrice)  return false;
        if (mapFilters.types.length && !mapFilters.types.includes(item.type)) return false;
        if (mapFilters.minRating && item.ratingScore < mapFilters.minRating) return false;
        return true;
    });
}

function applyMapFilters() {
    const listings = getMapFilteredListings();
    renderMapCards(listings);
    placeMarkers(listings);
}

/* ── Map sidebar controls ────────────────────────────────────── */
function initMapSidebarControls() {
    /* Budget slider */
    const minR = q('#mapRangeMin');
    const maxR = q('#mapRangeMax');
    const fill = q('#mapSliderFill');
    const lblMin = q('#mapBudgetMin');
    const lblMax = q('#mapBudgetMax');

    function updateMapSlider() {
        const mn = parseInt(minR.value), mx = parseInt(maxR.value);
        if (mn > mx) minR.value = mx;
        if (mx < mn) maxR.value = mn;
        const total = parseInt(maxR.max) - parseInt(minR.min);
        fill.style.left  = ((parseInt(minR.value) - parseInt(minR.min)) / total * 100) + '%';
        fill.style.right = ((parseInt(maxR.max) - parseInt(maxR.value)) / total * 100) + '%';
        if (lblMin) lblMin.textContent = minR.value;
        if (lblMax) lblMax.textContent = maxR.value;
        mapFilters.minPrice = parseInt(minR.value);
        mapFilters.maxPrice = parseInt(maxR.value);
    }

    if (minR && maxR) {
        minR.addEventListener('input', () => { updateMapSlider(); applyMapFilters(); });
        maxR.addEventListener('input', () => { updateMapSlider(); applyMapFilters(); });
        updateMapSlider();
    }

    /* Type checkboxes */
    qq('.map-type-cb').forEach(cb => {
        cb.addEventListener('change', () => {
            mapFilters.types = Array.from(qq('.map-type-cb:checked')).map(c => c.value);
            applyMapFilters();
        });
    });

    /* Rating checkboxes */
    qq('.map-rating-cb').forEach(cb => {
        cb.addEventListener('change', () => {
            const checked = Array.from(qq('.map-rating-cb:checked')).map(c => parseFloat(c.value));
            mapFilters.minRating = checked.length ? Math.min(...checked) : 0;
            applyMapFilters();
        });
    });

    /* Accordion headers */
    qq('.map-acc-header').forEach(btn => {
        btn.addEventListener('click', () => {
            btn.closest('.map-acc-item').classList.toggle('open');
        });
    });
}

/* ── Boot ────────────────────────────────────────────────────── */
document.addEventListener('DOMContentLoaded', () => {
    /* Open map */
    const btnMap = q('.btn-map');
    if (btnMap) {
        btnMap.addEventListener('click', () => openMapModal());
    }

    /* Close map */
    const btnClose = q('#btnCloseMap');
    if (btnClose) {
        btnClose.addEventListener('click', () => closeMapModal());
    }

    /* Sidebar controls */
    initMapSidebarControls();

    /* ESC key closes modal */
    document.addEventListener('keydown', e => {
        if (e.key === 'Escape') closeMapModal();
    });
});

/* Export buildCardHtml so search.js can re-use it */
window._buildCardHtml = buildCardHtml;
window._buildAmenityBadges = buildAmenityBadges;
