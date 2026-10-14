(function () {
    const panel = document.getElementById('chat-panel');
    const log = document.getElementById('chat-log');
    const form = document.getElementById('chat-form');
    const input = document.getElementById('chat-input');
    const sendBtn = document.getElementById('chat-send');
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    const CART_BADGE = '#cartCount';   // apne navbar ke cart badge ka id/selector yahan rakho

    async function refreshCartCount() {
        try {
            const r = await fetch('/Cart/GetCartItemCount');
            const d = await r.json();
            const el = document.querySelector(CART_BADGE);
            if (el) el.textContent = d.count ?? d.Count;
        } catch { }
    }

    document.getElementById('chat-fab').addEventListener('click', () => {
        panel.classList.toggle('open');
        if (panel.classList.contains('open')) input.focus();
    });

    function addMessage(text, cls) {
        const div = document.createElement('div');
        div.className = 'chat-msg ' + cls;
        div.textContent = text;            // textContent, innerHTML nahi: LLM ka output untrusted hai (XSS)
        log.appendChild(div);
        log.scrollTop = log.scrollHeight;
        return div;
    }

    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        const message = input.value.trim();
        if (!message) return;

        addMessage(message, 'chat-user');
        input.value = '';
        input.disabled = sendBtn.disabled = true;

        const thinking = addMessage('Assistant soch raha hai... 0s (kabhi kabhi 1-2 minute lagte hain)', 'chat-bot');
        const started = Date.now();
        const timer = setInterval(() => {
            thinking.textContent = 'Assistant soch raha hai... ' + Math.floor((Date.now() - started) / 1000) + 's';
        }, 1000);

        try {
            const res = await fetch('/Assistant/Chat', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json', 'RequestVerificationToken': token },
                body: JSON.stringify({ message })
            });

            if (res.status === 401 || res.redirected) { window.location.href = '/Login/Login'; return; }

            let data = {};
            try { data = await res.json(); } catch { }
            thinking.textContent = data.reply || 'Kuch gadbad ho gayi, dobara try karo.';

            if (res.ok) refreshCartCount();
        } catch {
            thinking.textContent = 'Network error. Dobara try karo.';
        } finally {
            clearInterval(timer);
            input.disabled = sendBtn.disabled = false;
            input.focus();
        }
    });
})();