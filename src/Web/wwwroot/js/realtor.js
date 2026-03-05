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

// Calculator
const calcBtn = document.getElementById('calcBtn');
const calcResult = document.getElementById('calcResult');

const incomeTable = {
    0: { base: 18000, label: '≈ 18\u202f000 ₴ / місяць' },
    1: { base: 25999, label: '≈ 25\u202f999 ₴ / місяць' },
    2: { base: 35500, label: '≈ 35\u202f500 ₴ / місяць' },
    3: { base: 52000, label: '≈ 52\u202f000 ₴ / місяць' },
};

if (calcBtn) {
    calcBtn.addEventListener('click', () => {
        const activeRoomBtn = document.querySelector('.realtor-calc-room-btn.active');
        const rooms = activeRoomBtn ? parseInt(activeRoomBtn.dataset.rooms) : 1;
        const entry = incomeTable[rooms] || incomeTable[1];
        // Small random variation ±5%
        const variance = Math.round(entry.base * (0.95 + Math.random() * 0.1));
        const formatted = variance.toLocaleString('uk-UA');
        calcResult.textContent = `≈ ${formatted} ₴ / місяць`;
    });
}
