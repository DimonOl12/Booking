/* =============================================================
   search.js — catalog / search results page interactive logic
   Real-time client-side filtering (no page reload)
   ============================================================= */

const SP = window._searchParams || {};

/* ── Helpers ────────────────────────────────────────────────── */
function q(sel)  { return document.querySelector(sel); }
function qq(sel) { return document.querySelectorAll(sel); }

function fmtUAH(n) { return Number(n).toLocaleString('uk-UA'); }

/* ── SVG icon strings reused in card template ───────────────── */
const SVG_WIFI = `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><path d="M1.5 8.5c5.5-5.5 15.5-5.5 21 0"/><path d="M5 12c3.9-3.9 11-3.9 14 0"/><path d="M8.5 15.5c2.2-2.2 6.8-2.2 7 0"/><circle cx="12" cy="19" r="1" fill="currentColor"/></svg>`;
const SVG_PARK = `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><rect x="3" y="3" width="18" height="18" rx="2"/><path d="M9 17V8h4a3 3 0 010 6H9"/></svg>`;
const SVG_POOL = `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><path d="M2 12h20M2 17c1.5-1 3.5-1 5 0s3.5 1 5 0 3.5-1 5 0"/><circle cx="8" cy="7" r="3"/></svg>`;
const SVG_BKFT = `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><path d="M18 8h1a4 4 0 010 8h-1"/><path d="M2 8h16v9a4 4 0 01-4 4H6a4 4 0 01-4-4V8z"/></svg>`;

/* ── Current filter & sort state ────────────────────────────── */
const filters = {
    minPrice:     SP.minPrice  || 400,
    maxPrice:     SP.maxPrice  || 6500,
    types:        SP.filterType ? [SP.filterType] : [],
    minRating:    0,
    sortBy:       SP.sortBy    || 'recommended',
};

/* ── Client-side filter + render ────────────────────────────── */
function getFilteredListings() {
    const all = window._allListings || [];
    let result = all.filter(item => {
        if (item.pricePerNight < filters.minPrice) return false;
        if (item.pricePerNight > filters.maxPrice)  return false;
        if (filters.types.length && !filters.types.includes(item.type)) return false;
        if (filters.minRating   && item.ratingScore < filters.minRating) return false;
        return true;
    });

    switch (filters.sortBy) {
        case 'price_asc':  result.sort((a,b) => a.pricePerNight - b.pricePerNight); break;
        case 'price_desc': result.sort((a,b) => b.pricePerNight - a.pricePerNight); break;
        case 'rating':     result.sort((a,b) => b.ratingScore   - a.ratingScore);   break;
    }
    return result;
}

function applyFilters() {
    const listings = getFilteredListings();
    renderCards(listings);
    const titleEl = q('.results-title');
    if (titleEl) {
        const city = SP.city ? `${SP.city}: ` : '';
        titleEl.textContent = `${city}Знайдено ${listings.length} помешкань`;
    }
}

/* ── Card HTML builder ──────────────────────────────────────── */
function buildCardHtml(item) {
    const rating = item.ratingScore.toFixed(1).replace('.', ',');
    let amenities = '';
    if (item.hasFreeWifi)    amenities += `<span class="lc-amenity" title="Wi-Fi">${SVG_WIFI}<span class="lc-amenity-label">Wi-Fi</span></span>`;
    if (item.hasFreeParking) amenities += `<span class="lc-amenity" title="Парковка">${SVG_PARK}<span class="lc-amenity-label">Парковка</span></span>`;
    if (item.hasPool)        amenities += `<span class="lc-amenity" title="Басейн">${SVG_POOL}<span class="lc-amenity-label">Басейн</span></span>`;
    if (item.hasBreakfast)   amenities += `<span class="lc-amenity" title="Сніданок">${SVG_BKFT}<span class="lc-amenity-label">Сніданок</span></span>`;

    const oldPrice = item.originalPricePerNight
        ? `<span class="lc-price-old">UAH ${fmtUAH(item.originalPricePerNight)}</span>` : '';
    const urgency  = item.roomsLeft <= 2
        ? `<div class="lc-urgency">Залишився ${item.roomsLeft} номер!</div>` : '';

    return `
<article class="listing-card" data-id="${item.id}" data-price="${item.pricePerNight}" data-rating="${item.ratingScore}" data-type="${item.type}">
  <div class="lc-img-wrap"><img src="${item.imageUrl}" alt="${item.name}" loading="lazy" /></div>
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
        <span class="lc-price">UAH ${fmtUAH(item.pricePerNight)}</span>
        <span class="lc-price-meta">за 1 ніч · ${SP.adults || 2} дорослих</span>
      </div>
      <button class="btn-view" data-id="${item.id}">Переглянути</button>
    </div>
  </div>
</article>`;
}

function renderCards(listings) {
    const container = q('#listingsContainer');
    if (!container) return;

    if (!listings.length) {
        container.innerHTML = '<div class="no-results" style="grid-column:1/-1"><p>На жаль, за вашим запитом нічого не знайдено.</p><p>Спробуйте змінити параметри пошуку.</p></div>';
    } else {
        container.innerHTML = listings.map(buildCardHtml).join('');
    }

    // Re-attach "Переглянути" click handlers
    container.querySelectorAll('.btn-view').forEach(btn => {
        btn.addEventListener('click', e => {
            e.stopPropagation();
            const params = new URLSearchParams({
                checkIn:  SP.checkIn  || '',
                checkOut: SP.checkOut || '',
                adults:   SP.adults   || 2,
                children: SP.children || 0,
                rooms:    SP.rooms    || 1,
            });
            window.location.href = `/Home/Property/${btn.dataset.id}?${params}`;
        });
    });
}

/* ── Price range slider (real-time, no page reload) ─────────── */
const rangeMin = q('#rangeMin');
const rangeMax = q('#rangeMax');
const fill     = q('#sliderFill');
const budMin   = q('#budgetMin');
const budMax   = q('#budgetMax');

function updateSlider() {
    if (!rangeMin || !rangeMax) return;
    let mn = parseInt(rangeMin.value), mx = parseInt(rangeMax.value);
    if (mn > mx) { rangeMin.value = mx; mn = mx; }
    if (mx < mn) { rangeMax.value = mn; mx = mn; }
    const total = parseInt(rangeMax.max) - parseInt(rangeMin.min);
    if (fill) {
        fill.style.left  = ((mn - parseInt(rangeMin.min)) / total * 100) + '%';
        fill.style.right = ((parseInt(rangeMax.max) - mx) / total * 100) + '%';
    }
    if (budMin) budMin.textContent = mn;
    if (budMax) budMax.textContent = mx;
    filters.minPrice = mn;
    filters.maxPrice = mx;
}

if (rangeMin && rangeMax) {
    rangeMin.addEventListener('input', () => { updateSlider(); applyFilters(); });
    rangeMax.addEventListener('input', () => { updateSlider(); applyFilters(); });
    updateSlider();
}

/* ── Accordion ──────────────────────────────────────────────── */
qq('.accordion-header').forEach(btn => {
    btn.addEventListener('click', () => {
        btn.closest('.accordion-item').classList.toggle('open');
    });
});

/* ── Filter checkboxes (real-time) ──────────────────────────── */
qq('input[name="filterType"]').forEach(cb => {
    // Pre-check from URL param
    if (SP.filterType && cb.value === SP.filterType) cb.checked = true;

    cb.addEventListener('change', () => {
        filters.types = Array.from(qq('input[name="filterType"]:checked')).map(c => c.value);
        applyFilters();
    });
});

qq('input[name="minRating"]').forEach(cb => {
    cb.addEventListener('change', () => {
        const checked = Array.from(qq('input[name="minRating"]:checked')).map(c => parseFloat(c.value));
        filters.minRating = checked.length ? Math.min(...checked) : 0;
        applyFilters();
    });
});

/* ── Inline counter (bedrooms / bathrooms) ──────────────────── */
qq('.ci-btn').forEach(btn => {
    btn.addEventListener('click', () => {
        const target  = btn.dataset.target;
        const action  = btn.dataset.action;
        const display = q(`#${target}Count`);
        if (!display) return;
        let val = parseInt(display.textContent) || 0;
        if (action === 'inc') val++;
        if (action === 'dec' && val > 0) val--;
        display.textContent = val;
    });
});

/* ── Sort dropdown (real-time, no page reload) ──────────────── */
const sortBtn   = q('#sortDropdownBtn');
const sortMenu  = q('#sortDropdownMenu');
const sortLabel = q('#sortLabel');

const SORT_LABELS = {
    recommended: 'Наші рекомендації',
    rating:      'Рейтинг',
    price_asc:   'Ціна: від низької',
    price_desc:  'Ціна: від високої',
};

if (sortBtn && sortMenu) {
    sortBtn.addEventListener('click', e => {
        e.stopPropagation();
        sortMenu.style.display = sortMenu.style.display === 'none' ? 'block' : 'none';
    });

    sortMenu.querySelectorAll('button').forEach(b => {
        const s = b.dataset.sort;
        if (s === filters.sortBy) b.classList.add('active');

        b.addEventListener('click', () => {
            filters.sortBy = s;
            sortMenu.querySelectorAll('button').forEach(x => x.classList.remove('active'));
            b.classList.add('active');
            if (sortLabel) sortLabel.textContent = SORT_LABELS[s] || s;
            sortMenu.style.display = 'none';
            applyFilters();
        });
    });

    if (sortLabel && SORT_LABELS[filters.sortBy]) {
        sortLabel.textContent = SORT_LABELS[filters.sortBy];
    }

    document.addEventListener('click', () => { sortMenu.style.display = 'none'; });
}

/* ── View toggle ─────────────────────────────────────────────
   "Список"  (data-view="list") → 3-col grid   → remove .grid-view
   "Таблиця" (data-view="grid") → horizontal   → add    .grid-view
   ─────────────────────────────────────────────────────────── */
const listingsContainer = q('#listingsContainer');
qq('.view-btn').forEach(btn => {
    btn.addEventListener('click', () => {
        qq('.view-btn').forEach(b => b.classList.remove('active'));
        btn.classList.add('active');
        if (btn.dataset.view === 'grid') {
            listingsContainer.classList.add('grid-view');
        } else {
            listingsContainer.classList.remove('grid-view');
        }
    });
});

/* ── "Show more results" (simulated pagination) ─────────────── */
const loadMoreBtn = q('#loadMoreBtn');
if (loadMoreBtn) {
    loadMoreBtn.addEventListener('click', () => {
        loadMoreBtn.textContent = 'Завантаження...';
        loadMoreBtn.disabled = true;
        setTimeout(() => {
            loadMoreBtn.textContent = 'Більше результатів не знайдено';
        }, 1200);
    });
}

/* ═══════════════════════════════════════════════════════════════
   DATE PICKER
   ═══════════════════════════════════════════════════════════════ */
const datepickerPopup = q('#datepickerPopup');
const sbDateField     = q('#sbDateField');
const sbDateLabel     = q('#sbDateLabel');
const hidCheckIn      = q('#hidCheckIn');
const hidCheckOut     = q('#hidCheckOut');

let dp = {
    viewYear:  new Date().getFullYear(),
    viewMonth: new Date().getMonth(),   // 0-based
    selectStart: SP.checkIn  ? parseDate(SP.checkIn)  : null,
    selectEnd:   SP.checkOut ? parseDate(SP.checkOut) : null,
    selecting: false
};

function parseDate(str) {
    // Accepts dd.mm.yyyy or yyyy-mm-dd
    if (!str) return null;
    if (str.includes('.')) {
        const [d, m, y] = str.split('.');
        return new Date(+y, +m - 1, +d);
    }
    const parts = str.split('-');
    if (parts.length === 3) return new Date(+parts[0], +parts[1] - 1, +parts[2]);
    return null;
}

function fmtDate(d) {
    if (!d) return '';
    return `${String(d.getDate()).padStart(2,'0')}.${String(d.getMonth()+1).padStart(2,'0')}.${d.getFullYear()}`;
}

function sameDay(a, b) {
    return a && b && a.toDateString() === b.toDateString();
}

function renderDPMonth(containerId, year, month, offset) {
    const wrap = q(`#${containerId}`);
    if (!wrap) return;

    const y = year;
    const m = month + offset;
    const date = new Date(y, m, 1);
    const realY = date.getFullYear();
    const realM = date.getMonth();

    const DAYS = ['Пн','Вт','Ср','Чт','Пт','Сб','Нд'];
    const MONTHS = ['Січень','Лютий','Березень','Квітень','Травень','Червень',
                    'Липень','Серпень','Вересень','Жовтень','Листопад','Грудень'];

    const firstDay = new Date(realY, realM, 1).getDay(); // 0=Sun
    const firstMon = (firstDay + 6) % 7; // 0=Mon
    const daysInMonth = new Date(realY, realM + 1, 0).getDate();
    const today = new Date(); today.setHours(0,0,0,0);

    let html = `<div class="dp-month-header">`;
    if (offset === 0) {
        html += `<button id="dpPrev" title="Попередній місяць">‹</button>`;
    } else {
        html += `<span></span>`;
    }
    html += `<span>${MONTHS[realM]} ${realY}</span>`;
    if (offset === 1) {
        html += `<button id="dpNext" title="Наступний місяць">›</button>`;
    } else {
        html += `<span></span>`;
    }
    html += `</div><div class="dp-weekdays">`;
    DAYS.forEach(d => { html += `<div>${d}</div>`; });
    html += `</div><div class="dp-days">`;

    for (let i = 0; i < firstMon; i++) html += `<div class="dp-day dp-empty"></div>`;
    for (let d = 1; d <= daysInMonth; d++) {
        const cur = new Date(realY, realM, d);
        let cls = 'dp-day';
        if (cur < today) cls += ' dp-disabled';
        if (sameDay(cur, today)) cls += ' dp-today';
        if (dp.selectStart && sameDay(cur, dp.selectStart)) cls += ' dp-selected';
        if (dp.selectEnd   && sameDay(cur, dp.selectEnd))   cls += ' dp-selected';
        if (dp.selectStart && dp.selectEnd) {
            if (cur > dp.selectStart && cur < dp.selectEnd) cls += ' dp-in-range';
        }
        html += `<div class="${cls}" data-date="${cur.toISOString()}">${d}</div>`;
    }
    html += `</div>`;
    wrap.innerHTML = html;

    // nav buttons
    const prev = q('#dpPrev');
    const next = q('#dpNext');
    if (prev) prev.addEventListener('click', e => { e.stopPropagation(); dp.viewMonth--; if (dp.viewMonth < 0) { dp.viewMonth = 11; dp.viewYear--; } renderDP(); });
    if (next) next.addEventListener('click', e => { e.stopPropagation(); dp.viewMonth++; if (dp.viewMonth > 11) { dp.viewMonth = 0; dp.viewYear++; } renderDP(); });

    // day clicks
    wrap.querySelectorAll('.dp-day:not(.dp-disabled):not(.dp-empty)').forEach(cell => {
        cell.addEventListener('click', e => {
            e.stopPropagation();
            const date = new Date(cell.dataset.date);
            if (!dp.selecting || dp.selectEnd) {
                dp.selectStart = date;
                dp.selectEnd   = null;
                dp.selecting   = true;
            } else {
                if (date < dp.selectStart) {
                    dp.selectEnd   = dp.selectStart;
                    dp.selectStart = date;
                } else {
                    dp.selectEnd = date;
                }
                dp.selecting = false;
            }
            renderDP();
            updateDateLabel();
        });
    });
}

function renderDP() {
    renderDPMonth('dpMonth1', dp.viewYear, dp.viewMonth, 0);
    renderDPMonth('dpMonth2', dp.viewYear, dp.viewMonth, 1);
}

function updateDateLabel() {
    if (dp.selectStart && dp.selectEnd) {
        sbDateLabel.textContent = `${fmtDate(dp.selectStart)} – ${fmtDate(dp.selectEnd)}`;
        if (hidCheckIn)  hidCheckIn.value  = fmtDate(dp.selectStart);
        if (hidCheckOut) hidCheckOut.value = fmtDate(dp.selectEnd);
    } else if (dp.selectStart) {
        sbDateLabel.textContent = `${fmtDate(dp.selectStart)} – ...`;
        if (hidCheckIn) hidCheckIn.value = fmtDate(dp.selectStart);
    } else {
        sbDateLabel.textContent = 'Дата заїзду – Дата виїзду';
        if (hidCheckIn)  hidCheckIn.value  = '';
        if (hidCheckOut) hidCheckOut.value = '';
    }
}

if (sbDateField && datepickerPopup) {
    sbDateField.addEventListener('click', e => {
        e.stopPropagation();
        const gpOpen = guestsPopup && guestsPopup.style.display !== 'none';
        if (gpOpen) guestsPopup.style.display = 'none';
        const isOpen = datepickerPopup.style.display !== 'none';
        if (!isOpen) { renderDP(); }
        datepickerPopup.style.display = isOpen ? 'none' : 'block';
    });

    q('#dpClear').addEventListener('click', e => {
        e.stopPropagation();
        dp.selectStart = null;
        dp.selectEnd   = null;
        dp.selecting   = false;
        renderDP();
        updateDateLabel();
    });

    q('#dpDone').addEventListener('click', e => {
        e.stopPropagation();
        datepickerPopup.style.display = 'none';
        updateDateLabel();
    });
}

// Initialise label from server values
updateDateLabel();

/* ═══════════════════════════════════════════════════════════════
   GUESTS POPUP
   ═══════════════════════════════════════════════════════════════ */
const guestsPopup  = q('#guestsPopup');
const sbGuestsField= q('#sbGuestsField');
const sbGuestsLabel= q('#sbGuestsLabel');
const hidAdults    = q('#hidAdults');
const hidChildren  = q('#hidChildren');
const hidRooms     = q('#hidRooms');

let guests = {
    adults:   SP.adults   || 2,
    children: SP.children || 0,
    rooms:    SP.rooms    || 1
};

function updateGuestsDisplay() {
    if (q('#gp-adults'))   q('#gp-adults').textContent   = guests.adults;
    if (q('#gp-children')) q('#gp-children').textContent = guests.children;
    if (q('#gp-rooms'))    q('#gp-rooms').textContent    = guests.rooms;

    const adultsStr   = `${guests.adults} ${guests.adults === 1 ? 'дорослий' : 'дорослих'}`;
    const childrenStr = `${guests.children} ${guests.children === 0 ? 'дітей' : guests.children === 1 ? 'дитина' : 'дітей'}`;
    const roomsStr    = `${guests.rooms} номер`;
    if (sbGuestsLabel) sbGuestsLabel.textContent = `${adultsStr} · ${childrenStr} · ${roomsStr}`;
    if (hidAdults)   hidAdults.value   = guests.adults;
    if (hidChildren) hidChildren.value = guests.children;
    if (hidRooms)    hidRooms.value    = guests.rooms;
}

if (sbGuestsField && guestsPopup) {
    sbGuestsField.addEventListener('click', e => {
        e.stopPropagation();
        if (datepickerPopup && datepickerPopup.style.display !== 'none')
            datepickerPopup.style.display = 'none';
        const isOpen = guestsPopup.style.display !== 'none';
        guestsPopup.style.display = isOpen ? 'none' : 'block';
    });

    guestsPopup.querySelectorAll('.gp-btn').forEach(btn => {
        btn.addEventListener('click', e => {
            e.stopPropagation();
            const target = btn.dataset.target;
            const action = btn.dataset.action;
            const mins = { adults: 1, children: 0, rooms: 1 };
            if (action === 'inc') guests[target]++;
            if (action === 'dec' && guests[target] > mins[target]) guests[target]--;
            updateGuestsDisplay();
        });
    });

    q('#gpDone').addEventListener('click', e => {
        e.stopPropagation();
        guestsPopup.style.display = 'none';
    });
}

updateGuestsDisplay();

/* ── Close popups on outside click ─────────────────────────── */
document.addEventListener('click', () => {
    if (datepickerPopup) datepickerPopup.style.display = 'none';
    if (guestsPopup)     guestsPopup.style.display     = 'none';
    if (sortMenu)        sortMenu.style.display         = 'none';
});
[datepickerPopup, guestsPopup].forEach(p => {
    if (p) p.addEventListener('click', e => e.stopPropagation());
});

/* ── Initial render: apply filters from URL params ──────────── */
// If _allListings is available, do a client-side render immediately
// (handles bookmarked/shared filtered URLs without relying solely on server HTML)
if (window._allListings && window._allListings.length > 0) {
    applyFilters();
}
