// FAQ accordion
function toggleFaq(item) {
    const wrap = item.closest('.realtor-faq-item-wrap');
    const isOpen = item.classList.contains('open');

    // Close all
    document.querySelectorAll('.realtor-faq-item').forEach(el => el.classList.remove('open'));

    if (!isOpen) {
        item.classList.add('open');
    }
}

// Room selector
document.querySelectorAll('.realtor-calc-room-btn').forEach(btn => {
    btn.addEventListener('click', function () {
        document.querySelectorAll('.realtor-calc-room-btn').forEach(b => b.classList.remove('active'));
        this.classList.add('active');
    });
});

// ── Calculator data ──────────────────────────────────────────────────────────
// baseRate = avg nightly rate in local currency
// currency = symbol, locale = toLocaleString locale, label = "/місяць" suffix
const COUNTRIES = {
    ukraine: {
        label: 'Україна', currency: '₴', locale: 'uk-UA', suffix: ' / місяць',
        cities: {
            kyiv:    { label: 'Київ',    baseRate: 1800,
                districts: { pecherskyi: { label: 'Печерський', m: 1.30 }, shevchenkivskyi: { label: 'Шевченківський', m: 1.15 }, obolon: { label: 'Оболонь', m: 0.95 }, podil: { label: 'Поділ', m: 1.10 }, sviatoshyn: { label: 'Святошин', m: 0.85 } } },
            lviv:    { label: 'Львів',   baseRate: 1400,
                districts: { galytskyi: { label: 'Галицький', m: 1.25 }, lychakivskyi: { label: 'Личаківський', m: 1.05 }, frankivskyi: { label: 'Франківський', m: 1.00 }, sykhivskyi: { label: 'Сихівський', m: 0.88 }, zaliznychnyi: { label: 'Залізничний', m: 0.90 } } },
            odesa:   { label: 'Одеса',   baseRate: 1300,
                districts: { prymorskyj: { label: 'Приморський', m: 1.30 }, kyivskyi: { label: 'Київський', m: 1.00 }, malinovskyi: { label: 'Малиновський', m: 0.85 }, suvorovskyi: { label: 'Суворовський', m: 0.80 }, khotejivka: { label: 'Хаджибейський', m: 0.90 } } },
            kharkiv: { label: 'Харків',  baseRate: 1100,
                districts: { shevchenkivskyi: { label: 'Шевченківський', m: 1.20 }, kholodnohirskyi: { label: 'Холодногірський', m: 0.95 }, nemyshlianskyi: { label: 'Немишлянський', m: 0.85 }, osnovianskyi: { label: 'Основʼянський', m: 0.88 }, saltivskyi: { label: 'Салтівський', m: 0.82 } } },
            dnipro:  { label: 'Дніпро',  baseRate: 1050,
                districts: { tsentralnyi: { label: 'Центральний', m: 1.20 }, sobornyi: { label: 'Соборний', m: 1.10 }, amur: { label: 'Амур-Нижньодніпровський', m: 0.85 }, chechelivskyi: { label: 'Чечелівський', m: 0.88 }, shevchenkivskyi: { label: 'Шевченківський', m: 0.92 } } },
        }
    },
    poland: {
        label: 'Польща', currency: 'zł', locale: 'pl-PL', suffix: ' / miesiąc',
        cities: {
            warsaw:  { label: 'Варшава',  baseRate: 350,
                districts: { srodmiescie: { label: 'Śródmieście', m: 1.30 }, mokotow: { label: 'Mokotów', m: 1.10 }, wola: { label: 'Wola', m: 1.00 }, praga: { label: 'Praga-Południe', m: 0.88 }, ursynow: { label: 'Ursynów', m: 0.92 } } },
            krakow:  { label: 'Краків',   baseRate: 300,
                districts: { stare_miasto: { label: 'Stare Miasto', m: 1.35 }, kazimierz: { label: 'Kazimierz', m: 1.20 }, podgorze: { label: 'Podgórze', m: 1.00 }, krowodrza: { label: 'Krowodrza', m: 0.90 }, nowa_huta: { label: 'Nowa Huta', m: 0.80 } } },
            wroclaw: { label: 'Вроцлав',  baseRate: 270,
                districts: { stare_miasto: { label: 'Stare Miasto', m: 1.30 }, krzyki: { label: 'Krzyki', m: 1.05 }, fabryczna: { label: 'Fabryczna', m: 0.95 }, psie_pole: { label: 'Psie Pole', m: 0.85 }, srodmiescie: { label: 'Śródmieście', m: 1.15 } } },
            gdansk:  { label: 'Гданськ',  baseRate: 290,
                districts: { srodmiescie: { label: 'Śródmieście', m: 1.28 }, wrzeszcz: { label: 'Wrzeszcz', m: 1.10 }, oliwa: { label: 'Oliwa', m: 1.05 }, przymorze: { label: 'Przymorze', m: 0.95 }, zaspa: { label: 'Zaspa', m: 0.88 } } },
        }
    },
    germany: {
        label: 'Німеччина', currency: '€', locale: 'de-DE', suffix: ' / Monat',
        cities: {
            berlin:  { label: 'Берлін',   baseRate: 120,
                districts: { mitte: { label: 'Mitte', m: 1.35 }, prenzlauer_berg: { label: 'Prenzlauer Berg', m: 1.20 }, friedrichshain: { label: 'Friedrichshain', m: 1.15 }, charlottenburg: { label: 'Charlottenburg', m: 1.25 }, neukolln: { label: 'Neukölln', m: 0.90 } } },
            munich:  { label: 'Мюнхен',   baseRate: 160,
                districts: { altstadt: { label: 'Altstadt-Lehel', m: 1.40 }, maxvorstadt: { label: 'Maxvorstadt', m: 1.25 }, schwabing: { label: 'Schwabing', m: 1.20 }, haidhausen: { label: 'Haidhausen', m: 1.10 }, moosach: { label: 'Moosach', m: 0.88 } } },
            hamburg: { label: 'Гамбург',  baseRate: 130,
                districts: { mitte: { label: 'Hamburg-Mitte', m: 1.30 }, altona: { label: 'Altona', m: 1.15 }, eimsbuettel: { label: 'Eimsbüttel', m: 1.10 }, nord: { label: 'Hamburg-Nord', m: 1.05 }, wandsbek: { label: 'Wandsbek', m: 0.90 } } },
            frankfurt: { label: 'Франкфурт', baseRate: 140,
                districts: { innenstadt: { label: 'Innenstadt', m: 1.35 }, sachsenhausen: { label: 'Sachsenhausen', m: 1.15 }, bornheim: { label: 'Bornheim', m: 1.05 }, nordend: { label: 'Nordend', m: 1.10 }, hausen: { label: 'Hausen', m: 0.88 } } },
        }
    },
    czech: {
        label: 'Чехія', currency: 'Kč', locale: 'cs-CZ', suffix: ' / měsíc',
        cities: {
            prague:  { label: 'Прага',    baseRate: 3200,
                districts: { praha1: { label: 'Praha 1', m: 1.40 }, praha2: { label: 'Praha 2', m: 1.20 }, praha3: { label: 'Praha 3', m: 1.05 }, praha5: { label: 'Praha 5', m: 1.00 }, praha10: { label: 'Praha 10', m: 0.88 } } },
            brno:    { label: 'Брно',     baseRate: 2200,
                districts: { stred: { label: 'Brno-střed', m: 1.30 }, sever: { label: 'Brno-sever', m: 1.00 }, jih: { label: 'Brno-jih', m: 0.90 }, kralovo_pole: { label: 'Královo Pole', m: 1.10 }, lisen: { label: 'Líšeň', m: 0.85 } } },
            ostrava: { label: 'Острава',  baseRate: 1600,
                districts: { moravska: { label: 'Moravská Ostrava', m: 1.25 }, slezska: { label: 'Slezská Ostrava', m: 0.95 }, vitkovice: { label: 'Vítkovice', m: 0.90 }, poruba: { label: 'Poruba', m: 0.88 }, trebovice: { label: 'Třebovice', m: 0.82 } } },
        }
    },
};

// rooms multiplier: studio=0, 1br=1, 2br=2, 3br=3
const ROOMS_MULTIPLIER = { 0: 1.00, 1: 1.45, 2: 2.00, 3: 2.70 };
const OCCUPANCY = 0.72; // 72% avg occupancy
const NIGHTS = 30;

// ── Selects ──────────────────────────────────────────────────────────────────
const selCountry  = document.getElementById('calcCountry');
const selCity     = document.getElementById('calcCity');
const selDistrict = document.getElementById('calcDistrict');

function populateSelect(sel, items, placeholder) {
    sel.innerHTML = `<option value="">${placeholder}</option>`;
    for (const [key, val] of Object.entries(items)) {
        const opt = document.createElement('option');
        opt.value = key;
        opt.textContent = val.label;
        sel.appendChild(opt);
    }
}

if (selCountry) {
    selCountry.addEventListener('change', () => {
        const country = COUNTRIES[selCountry.value];
        selCity.disabled = !country;
        selDistrict.disabled = true;
        selCity.innerHTML = '<option value="">Оберіть місто</option>';
        selDistrict.innerHTML = '<option value="">Оберіть район</option>';
        if (country) populateSelect(selCity, country.cities, 'Оберіть місто');
    });
}

if (selCity) {
    selCity.addEventListener('change', () => {
        const country = COUNTRIES[selCountry.value];
        const city = country && country.cities[selCity.value];
        selDistrict.disabled = !city;
        selDistrict.innerHTML = '<option value="">Оберіть район</option>';
        if (city) populateSelect(selDistrict, city.districts, 'Оберіть район');
    });
}

// ── Calculate ────────────────────────────────────────────────────────────────
const calcBtn    = document.getElementById('calcBtn');
const calcResult = document.getElementById('calcResult');

if (calcBtn) {
    calcBtn.addEventListener('click', () => {
        const countryKey  = selCountry  ? selCountry.value  : '';
        const cityKey     = selCity     ? selCity.value     : '';
        const districtKey = selDistrict ? selDistrict.value : '';

        const country  = COUNTRIES[countryKey];
        const city     = country  && country.cities[cityKey];
        const district = city     && city.districts[districtKey];

        if (!country || !city || !district) {
            calcResult.textContent = 'Оберіть країну, місто та район';
            calcResult.style.color = '#E73D00';
            return;
        }
        calcResult.style.color = '';

        const activeRoomBtn = document.querySelector('.realtor-calc-room-btn.active');
        const rooms = activeRoomBtn ? parseInt(activeRoomBtn.dataset.rooms) : 1;
        const rm = ROOMS_MULTIPLIER[rooms] ?? ROOMS_MULTIPLIER[1];

        // formula: base × district_multiplier × rooms_multiplier × 30 nights × occupancy ± 5%
        const raw      = city.baseRate * district.m * rm * NIGHTS * OCCUPANCY;
        const variance = raw * (0.97 + Math.random() * 0.06); // ±3%
        const amount   = Math.round(variance / 100) * 100;    // round to nearest 100
        const formatted = amount.toLocaleString(country.locale);
        calcResult.textContent = `≈ ${formatted} ${country.currency}${country.suffix}`;
        calcResult.style.color = '';
    });
}
