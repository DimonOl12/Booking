/* messages.js — interactivity for the Messages page */
(function () {
    'use strict';

    // ── Mock conversations ──────────────────────────────────────────
    var conversations = {
        atlas: {
            name:    'Атлас Делюкс',
            sub:     'Андрій Брункевич',
            initial: 'А',
            messages: [
                { type: 'recv', text: 'Доброго дня! Ми підтвердили ваше бронювання 😊\nЗаїзд можливий з 14:00.', time: '14:30' },
                { type: 'sent', text: 'Дякую! Ще такі питання: чи можна зробити пізній виїзд?', time: '14:32' },
                { type: 'recv', text: 'Та доречно, давайте! Якщо ще маєте питання — завжди готові відповісти.', time: '14:34' },
                { type: 'sent', text: 'Дякую, доречно! Ще одне — чи можна замовити трансфер?', time: '14:35' },
                { type: 'recv', text: 'Узгодимо з вами! Але спочатку — чи є ще питання?', time: '14:36' },
                { type: 'sent', text: 'Суперр, дякую вам за відповідь. Підтверджуйте ціну бронювання!', time: '14:38' },
            ]
        },
        kvartyr: {
            name:    'Квартира в центрі Львова',
            sub:     'Власник помешкання',
            initial: 'К',
            messages: [
                { type: 'recv', text: 'Доброго дня! Ми підтвердили ваше бронювання 😊', time: '14:30' },
                { type: 'sent', text: 'Добрий день! Підкажіть, чи є паркомісце?', time: '14:34' },
            ]
        },
        support: {
            name:    'Служба підтримки Reservio',
            sub:     'Служба підтримки',
            initial: 'RS',
            isSupport: true,
            messages: [
                { type: 'recv', text: 'Дякуємо за звернення! Ваш запит прийнято в обробку.', time: 'вчора' },
                { type: 'recv', text: 'Очікуйте відповідь протягом 24 годин. Ми завжди раді допомогти! 🙂', time: 'вчора' },
            ]
        },
        swiss: {
            name:    'Готель Швейцарський',
            sub:     'Адміністратор готелю',
            initial: 'Ш',
            messages: [
                { type: 'recv', text: 'Будь ласка зачекайте підтвердження вашого бронювання.', time: '15.01.2026' },
            ]
        }
    };

    var currentChat = null;
    var currentTab  = 'all';

    // ── DOM refs ────────────────────────────────────────────────────
    var emptyPanel   = document.getElementById('emptyPanel');
    var archivePanel = document.getElementById('archivePanel');
    var chatView     = document.getElementById('chatView');
    var chatPanel    = document.getElementById('chatPanel');
    var msgMessages  = document.getElementById('msgMessages');
    var msgInput     = document.getElementById('msgInput');

    // ── Tab switch ──────────────────────────────────────────────────
    window.switchTab = function (tab) {
        currentTab = tab;
        var btnAll     = document.getElementById('tabAll');
        var btnArchive = document.getElementById('tabArchive');
        if (tab === 'all') {
            btnAll.classList.add('active');
            btnArchive.classList.remove('active');
            if (archivePanel) archivePanel.style.display = 'none';
            if (!currentChat) {
                if (emptyPanel) emptyPanel.style.display = 'flex';
                chatView.style.display = 'none';
            }
        } else {
            btnAll.classList.remove('active');
            btnArchive.classList.add('active');
            if (emptyPanel) emptyPanel.style.display = 'none';
            chatView.style.display = 'none';
            if (archivePanel) archivePanel.style.display = 'flex';
            currentChat = null;
            setActiveItem(null);
        }
    };

    // ── Open chat ───────────────────────────────────────────────────
    window.openChat = function (key) {
        if (currentTab !== 'all') return;
        currentChat = key;
        var conv = conversations[key];
        if (!conv) return;

        // Update header
        var avatar = document.getElementById('chatHeaderAvatar');
        avatar.textContent = conv.initial;
        avatar.className = 'msg-chat-header-avatar' + (conv.isSupport ? ' support' : '');
        document.getElementById('chatHeaderName').textContent = conv.name;
        document.getElementById('chatHeaderSub').textContent  = conv.sub;

        // Render messages
        renderMessages(conv.messages);

        // Show chat panel
        if (emptyPanel) emptyPanel.style.display = 'none';
        if (archivePanel) archivePanel.style.display = 'none';
        chatView.style.display = 'flex';

        // Remove unread badge from list item
        var item = document.querySelector('[data-chat="' + key + '"]');
        if (item) {
            var badge = item.querySelector('.msg-unread-badge');
            if (badge) badge.remove();
        }

        setActiveItem(key);
        scrollToBottom();
    };

    // ── Close chat ──────────────────────────────────────────────────
    window.closeChat = function () {
        currentChat = null;
        chatView.style.display = 'none';
        if (emptyPanel) emptyPanel.style.display = 'flex';
        setActiveItem(null);
    };

    // ── Send message ────────────────────────────────────────────────
    window.sendMsg = function () {
        if (!currentChat || !msgInput) return;
        var text = msgInput.value.trim();
        if (!text) return;

        var conv = conversations[currentChat];
        var now  = new Date();
        var time = now.getHours() + ':' + String(now.getMinutes()).padStart(2, '0');
        conv.messages.push({ type: 'sent', text: text, time: time });
        msgInput.value = '';
        renderMessages(conv.messages);
        scrollToBottom();

        // Simulate reply after 1.5s
        setTimeout(function () {
            conv.messages.push({ type: 'recv', text: 'Дякуємо за повідомлення! Ми відповімо найближчим часом 😊', time: time });
            renderMessages(conv.messages);
            scrollToBottom();
        }, 1500);
    };

    // ── Search filter ───────────────────────────────────────────────
    window.filterChats = function (query) {
        var q = query.toLowerCase();
        var items = document.querySelectorAll('#chatList .msg-item');
        items.forEach(function (item) {
            var name = item.querySelector('.msg-item-name');
            if (!name) return;
            item.style.display = name.textContent.toLowerCase().includes(q) ? '' : 'none';
        });
    };

    // ── Helpers ─────────────────────────────────────────────────────
    function renderMessages(msgs) {
        msgMessages.innerHTML = '';
        msgs.forEach(function (m) {
            var wrap = document.createElement('div');
            wrap.className = 'msg-bubble-wrap ' + m.type;
            var bubble = document.createElement('div');
            bubble.className = 'msg-bubble';
            bubble.innerHTML = m.text.replace(/\n/g, '<br>');
            var timeEl = document.createElement('div');
            timeEl.className = 'msg-bubble-time';
            timeEl.textContent = m.time;
            wrap.appendChild(bubble);
            wrap.appendChild(timeEl);
            msgMessages.appendChild(wrap);
        });
    }

    function scrollToBottom() {
        if (msgMessages) msgMessages.scrollTop = msgMessages.scrollHeight;
    }

    function setActiveItem(key) {
        document.querySelectorAll('#chatList .msg-item').forEach(function (el) {
            el.classList.toggle('active', el.getAttribute('data-chat') === key);
        });
    }

    // Auto-open first chat if there's one
    // (no auto-open — show empty panel by default)

})();
