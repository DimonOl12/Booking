// Search tabs — change form content per tab
const TAB_CONFIGS = {
    'Помешкання': {
        field1Icon: '<circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/>',
        field1Placeholder: 'Куди ви вирушаєте?',
        showDate: true,
        showGuests: true,
        btnText: 'Шукати',
        notice: null
    },
    'Переліт': {
        field1Icon: '<path d="M22 16.92v3a2 2 0 01-2.18 2 19.79 19.79 0 01-8.63-3.07 19.5 19.5 0 01-6-6A19.79 19.79 0 012.12 4.18 2 2 0 014.11 2h3a2 2 0 012 1.72c.127.96.361 1.903.7 2.81a2 2 0 01-.45 2.11L8.09 9.91a16 16 0 006 6l1.27-1.27a2 2 0 012.11-.45c.907.339 1.85.573 2.81.7A2 2 0 0122 16.92z"/><path d="M14.5 2.5c1.5 1.5 2 3.5 2 5.5m3.5-5.5c2 2 3 5 3 8"/>',
        field1Placeholder: 'Звідки летіти?',
        showDate: true,
        showGuests: true,
        btnText: 'Знайти рейс',
        notice: 'Незабаром — пошук авіарейсів'
    },
    'Оренда авто': {
        field1Icon: '<rect x="1" y="3" width="15" height="13" rx="2"/><polygon points="16 8 20 8 23 11 23 16 16 16 16 8"/><circle cx="5.5" cy="18.5" r="2.5"/><circle cx="18.5" cy="18.5" r="2.5"/>',
        field1Placeholder: 'Місто отримання авто',
        showDate: true,
        showGuests: true,
        btnText: 'Знайти авто',
        notice: 'Незабаром — оренда автомобілів'
    },
    'Дозвілля': {
        field1Icon: '<polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2"/>',
        field1Placeholder: 'Місто або назва події',
        showDate: true,
        showGuests: true,
        btnText: 'Знайти дозвілля',
        notice: 'Незабаром — концерти, тури, атракції'
    },
    'Таксі з/до аеропорту': {
        field1Icon: '<circle cx="12" cy="12" r="10"/><polyline points="12 6 12 12 16 14"/>',
        field1Placeholder: 'Адреса подачі або аеропорт',
        showDate: true,
        showGuests: false,
        btnText: 'Замовити таксі',
        notice: 'Незабаром — таксі до аеропорту'
    }
};

(function initSearchTabs() {
    const cityInput      = document.getElementById('homeCity');
    const field1         = cityInput && cityInput.closest('.search-input-group');
    const dateField      = document.getElementById('homeDateField');
    const guestsField    = document.getElementById('homeGuestsField');
    const searchBtn      = document.querySelector('.search-btn');
    const field1SvgEl    = field1 && field1.querySelector('svg');

    // Notice element (injected once)
    let noticeEl = document.getElementById('searchTabNotice');
    if (!noticeEl) {
        noticeEl = document.createElement('p');
        noticeEl.id = 'searchTabNotice';
        noticeEl.className = 'search-tab-notice';
        const wrapper = document.querySelector('.search-btn-wrapper');
        if (wrapper) wrapper.insertAdjacentElement('afterend', noticeEl);
    }

    function applyTabConfig(tabName) {
        const cfg = TAB_CONFIGS[tabName] || TAB_CONFIGS['Помешкання'];

        if (cityInput) cityInput.placeholder = cfg.field1Placeholder;

        if (field1SvgEl) {
            field1SvgEl.setAttribute('viewBox', '0 0 24 24');
            field1SvgEl.innerHTML = cfg.field1Icon;
        }

        if (dateField) dateField.style.display = cfg.showDate ? '' : 'none';
        if (guestsField) guestsField.style.display = cfg.showGuests ? '' : 'none';
        if (searchBtn) searchBtn.textContent = cfg.btnText;

        if (noticeEl) {
            noticeEl.textContent = cfg.notice || '';
            noticeEl.style.display = cfg.notice ? '' : 'none';
        }
    }

    document.querySelectorAll('.search-tab').forEach(tab => {
        tab.addEventListener('click', function () {
            document.querySelectorAll('.search-tab').forEach(t => t.classList.remove('active'));
            this.classList.add('active');
            applyTabConfig(this.textContent.trim());
        });
    });

    // Init with active tab
    applyTabConfig('Помешкання');
})();

// Category pills — toggle active + filter city-grid cards
const PILL_TYPE_MAP = {
    'Місто': 'місто',
    'Пляж': 'пляж',
    'Активний відпочинок': 'активний',
    'Спокійний відпочинок': 'спокійний',
    'Романтика': 'романтика',
    'Їжа': 'їжа'
};

function filterCityGrid(type) {
    document.querySelectorAll('#cityGrid .city-card').forEach(card => {
        const types = card.dataset.type || '';
        card.style.display = types.split(' ').includes(type) ? '' : 'none';
    });
}

document.querySelectorAll('.category-pill').forEach(pill => {
    pill.addEventListener('click', function() {
        document.querySelectorAll('.category-pill').forEach(p => p.classList.remove('active'));
        this.classList.add('active');
        const type = PILL_TYPE_MAP[this.textContent.trim()] || 'місто';
        filterCityGrid(type);
    });
});

// Traveler tabs
document.querySelectorAll('.traveler-tab').forEach(tab => {
    tab.addEventListener('click', function() {
        document.querySelectorAll('.traveler-tab').forEach(t => t.classList.remove('active'));
        this.classList.add('active');
    });
});

// Arrow navigation for section-header nav-arrows → .property-scroll / .types-grid
document.querySelectorAll('.section-header').forEach(header => {
    const arrows = header.querySelectorAll('.arrow-btn');
    if (arrows.length < 2) return;

    const section = header.closest('.section');
    const scrollEl = section ? section.querySelector('.property-scroll, .types-grid') : null;
    if (!scrollEl) return;

    const [prevBtn, nextBtn] = arrows;
    const SCROLL_AMOUNT = 320;

    const updateArrows = () => {
        prevBtn.classList.toggle('disabled', scrollEl.scrollLeft <= 0);
        nextBtn.classList.toggle('disabled',
            scrollEl.scrollLeft >= scrollEl.scrollWidth - scrollEl.clientWidth - 1);
    };

    prevBtn.addEventListener('click', () => {
        if (!prevBtn.classList.contains('disabled')) {
            scrollEl.scrollBy({ left: -SCROLL_AMOUNT, behavior: 'smooth' });
        }
    });

    nextBtn.addEventListener('click', () => {
        if (!nextBtn.classList.contains('disabled')) {
            scrollEl.scrollBy({ left: SCROLL_AMOUNT, behavior: 'smooth' });
        }
    });

    scrollEl.addEventListener('scroll', updateArrows);
    updateArrows();
});

// Category pills arrow navigation
const categoryPills = document.querySelector('.category-pills');
if (categoryPills) {
    const arrows = categoryPills.querySelectorAll('.arrow-btn');
    if (arrows.length >= 2) {
        const [prevBtn, nextBtn] = arrows;
        prevBtn.addEventListener('click', () => {
            categoryPills.scrollBy({ left: -200, behavior: 'smooth' });
        });
        nextBtn.addEventListener('click', () => {
            categoryPills.scrollBy({ left: 200, behavior: 'smooth' });
        });
    }
}

/* ================================================================
   HOME PAGE SEARCH — Date picker + Guests picker
   ================================================================ */

// ── shared helpers ────────────────────────────────────────────────
function fmtDate(d) {
    if (!d) return '';
    return `${String(d.getDate()).padStart(2,'0')}.${String(d.getMonth()+1).padStart(2,'0')}.${d.getFullYear()}`;
}
function sameDay(a, b) { return a && b && a.toDateString() === b.toDateString(); }

// ── DATE PICKER ───────────────────────────────────────────────────
const homeDateField    = document.getElementById('homeDateField');
const homeDatePopup    = document.getElementById('homeDatepickerPopup');
const homeDateLabel    = document.getElementById('homeDateLabel');
const homeCheckIn      = document.getElementById('homeCheckIn');
const homeCheckOut     = document.getElementById('homeCheckOut');

let hdp = {
    viewYear: new Date().getFullYear(),
    viewMonth: new Date().getMonth(),
    selectStart: null,
    selectEnd: null,
    selecting: false
};

function renderHomeDP() {
    renderHDPMonth('hdpMonth1', 0);
    renderHDPMonth('hdpMonth2', 1);
}

function renderHDPMonth(containerId, offset) {
    const wrap = document.getElementById(containerId);
    if (!wrap) return;

    const d = new Date(hdp.viewYear, hdp.viewMonth + offset, 1);
    const y = d.getFullYear();
    const m = d.getMonth();
    const DAYS   = ['Пн','Вт','Ср','Чт','Пт','Сб','Нд'];
    const MONTHS = ['Січень','Лютий','Березень','Квітень','Травень','Червень',
                    'Липень','Серпень','Вересень','Жовтень','Листопад','Грудень'];

    const firstMon = (new Date(y, m, 1).getDay() + 6) % 7;
    const dim = new Date(y, m + 1, 0).getDate();
    const today = new Date(); today.setHours(0,0,0,0);

    let html = `<div class="dp-month-header">`;
    if (offset === 0) {
        html += `<button id="hdpPrev">‹</button>`;
    } else {
        html += `<span></span>`;
    }
    html += `<span>${MONTHS[m]} ${y}</span>`;
    if (offset === 1) {
        html += `<button id="hdpNext">›</button>`;
    } else {
        html += `<span></span>`;
    }
    html += `</div><div class="dp-weekdays">`;
    DAYS.forEach(day => { html += `<div>${day}</div>`; });
    html += `</div><div class="dp-days">`;

    for (let i = 0; i < firstMon; i++) html += `<div class="dp-day dp-empty"></div>`;
    for (let i = 1; i <= dim; i++) {
        const cur = new Date(y, m, i);
        let cls = 'dp-day';
        if (cur < today) cls += ' dp-disabled';
        if (sameDay(cur, today)) cls += ' dp-today';
        if (hdp.selectStart && sameDay(cur, hdp.selectStart)) cls += ' dp-selected';
        if (hdp.selectEnd   && sameDay(cur, hdp.selectEnd))   cls += ' dp-selected';
        if (hdp.selectStart && hdp.selectEnd && cur > hdp.selectStart && cur < hdp.selectEnd)
            cls += ' dp-in-range';
        html += `<div class="${cls}" data-date="${cur.toISOString()}">${i}</div>`;
    }
    html += `</div>`;
    wrap.innerHTML = html;

    const prev = document.getElementById('hdpPrev');
    const next = document.getElementById('hdpNext');
    if (prev) prev.addEventListener('click', e => {
        e.stopPropagation();
        hdp.viewMonth--; if (hdp.viewMonth < 0) { hdp.viewMonth = 11; hdp.viewYear--; }
        renderHomeDP();
    });
    if (next) next.addEventListener('click', e => {
        e.stopPropagation();
        hdp.viewMonth++; if (hdp.viewMonth > 11) { hdp.viewMonth = 0; hdp.viewYear++; }
        renderHomeDP();
    });

    wrap.querySelectorAll('.dp-day:not(.dp-disabled):not(.dp-empty)').forEach(cell => {
        cell.addEventListener('click', e => {
            e.stopPropagation();
            const date = new Date(cell.dataset.date);
            if (!hdp.selecting || hdp.selectEnd) {
                hdp.selectStart = date; hdp.selectEnd = null; hdp.selecting = true;
            } else {
                if (date < hdp.selectStart) { hdp.selectEnd = hdp.selectStart; hdp.selectStart = date; }
                else { hdp.selectEnd = date; }
                hdp.selecting = false;
            }
            renderHomeDP();
            updateHomeDateLabel();
        });
    });
}

function updateHomeDateLabel() {
    if (hdp.selectStart && hdp.selectEnd) {
        homeDateLabel.textContent = `${fmtDate(hdp.selectStart)} – ${fmtDate(hdp.selectEnd)}`;
        if (homeCheckIn)  homeCheckIn.value  = fmtDate(hdp.selectStart);
        if (homeCheckOut) homeCheckOut.value = fmtDate(hdp.selectEnd);
    } else if (hdp.selectStart) {
        homeDateLabel.textContent = `${fmtDate(hdp.selectStart)} – ...`;
        if (homeCheckIn) homeCheckIn.value = fmtDate(hdp.selectStart);
    } else {
        homeDateLabel.textContent = 'Дата заїзду – Дата виїзду';
        if (homeCheckIn)  homeCheckIn.value  = '';
        if (homeCheckOut) homeCheckOut.value = '';
    }
}

if (homeDateField && homeDatePopup) {
    homeDateField.addEventListener('click', e => {
        e.stopPropagation();
        if (homeGuestsPopup) homeGuestsPopup.style.display = 'none';
        const isOpen = homeDatePopup.style.display !== 'none';
        if (!isOpen) renderHomeDP();
        homeDatePopup.style.display = isOpen ? 'none' : 'block';
    });
    const hdpClear = document.getElementById('hdpClear');
    const hdpDone  = document.getElementById('hdpDone');
    if (hdpClear) hdpClear.addEventListener('click', e => {
        e.stopPropagation();
        hdp.selectStart = hdp.selectEnd = null; hdp.selecting = false;
        renderHomeDP(); updateHomeDateLabel();
    });
    if (hdpDone) hdpDone.addEventListener('click', e => {
        e.stopPropagation();
        homeDatePopup.style.display = 'none';
    });
}

// ── GUESTS PICKER ─────────────────────────────────────────────────
const homeGuestsField  = document.getElementById('homeGuestsField');
const homeGuestsPopup  = document.getElementById('homeGuestsPopup');
const homeGuestsLabel  = document.getElementById('homeGuestsLabel');
const homeAdults       = document.getElementById('homeAdults');
const homeChildren     = document.getElementById('homeChildren');
const homeRooms        = document.getElementById('homeRooms');

let hg = { adults: 2, children: 0, rooms: 1 };

function updateHomeGuestsDisplay() {
    const el = (id) => document.getElementById(id);
    if (el('hgp-adults'))   el('hgp-adults').textContent   = hg.adults;
    if (el('hgp-children')) el('hgp-children').textContent = hg.children;
    if (el('hgp-rooms'))    el('hgp-rooms').textContent    = hg.rooms;

    const aStr = `${hg.adults} ${hg.adults === 1 ? 'дорослий' : 'дорослих'}`;
    const cStr = `${hg.children} ${hg.children === 0 ? 'дітей' : hg.children === 1 ? 'дитина' : 'дітей'}`;
    const rStr = `${hg.rooms} номер`;
    if (homeGuestsLabel) homeGuestsLabel.textContent = `${aStr} · ${cStr} · ${rStr}`;
    if (homeAdults)   homeAdults.value   = hg.adults;
    if (homeChildren) homeChildren.value = hg.children;
    if (homeRooms)    homeRooms.value    = hg.rooms;
}

if (homeGuestsField && homeGuestsPopup) {
    homeGuestsField.addEventListener('click', e => {
        e.stopPropagation();
        if (homeDatePopup) homeDatePopup.style.display = 'none';
        const isOpen = homeGuestsPopup.style.display !== 'none';
        homeGuestsPopup.style.display = isOpen ? 'none' : 'block';
    });

    homeGuestsPopup.querySelectorAll('.gp-btn').forEach(btn => {
        btn.addEventListener('click', e => {
            e.stopPropagation();
            const target = btn.dataset.target;
            const action = btn.dataset.action;
            const mins = { adults: 1, children: 0, rooms: 1 };
            const maxs = { adults: 16, children: 10, rooms: 8 };
            if (action === 'inc' && hg[target] < maxs[target]) hg[target]++;
            if (action === 'dec' && hg[target] > mins[target]) hg[target]--;
            updateHomeGuestsDisplay();
        });
    });

    const hgpDone = document.getElementById('hgpDone');
    if (hgpDone) hgpDone.addEventListener('click', e => {
        e.stopPropagation();
        homeGuestsPopup.style.display = 'none';
    });
}

updateHomeGuestsDisplay();

// ── Close all popups on outside click ────────────────────────────
document.addEventListener('click', () => {
    if (homeDatePopup)   homeDatePopup.style.display   = 'none';
    if (homeGuestsPopup) homeGuestsPopup.style.display = 'none';
});
[homeDatePopup, homeGuestsPopup].forEach(p => {
    if (p) p.addEventListener('click', e => e.stopPropagation());
});

// ── Property card clicks (popular + deals) ───────────────────────
document.querySelectorAll('.property-card[data-id]').forEach(card => {
    card.addEventListener('click', function(e) {
        // Don't navigate if clicking the fav button
        if (e.target.closest('.property-card-fav')) return;
        window.location.href = '/Home/Property/' + this.dataset.id;
    });
});