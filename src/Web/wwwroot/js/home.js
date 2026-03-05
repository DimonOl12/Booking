// Search tabs
document.querySelectorAll('.search-tab').forEach(tab => {
    tab.addEventListener('click', function() {
        document.querySelectorAll('.search-tab').forEach(t => t.classList.remove('active'));
        this.classList.add('active');
    });
});

// Category pills
document.querySelectorAll('.category-pill').forEach(pill => {
    pill.addEventListener('click', function() {
        document.querySelectorAll('.category-pill').forEach(p => p.classList.remove('active'));
        this.classList.add('active');
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
            const mins   = { adults: 1, children: 0, rooms: 1 };
            if (action === 'inc') hg[target]++;
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