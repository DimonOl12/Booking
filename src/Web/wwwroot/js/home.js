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