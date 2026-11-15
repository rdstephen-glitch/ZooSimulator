const POLL_MS = 2000;
const EMOJI = { Monkey: '🐒', Giraffe: '🦒', Elephant: '🐘' };

// ---- Sound and game-over effects ----
let audioCtx;
let soundOn = true;
let gameOverBusy = false;
let lastAllDead = null; // null until the first status arrives, so a refresh never replays it

const soundButton = document.getElementById('sound-toggle');

try { soundOn = localStorage.getItem('zooSound') !== 'off'; } catch { /* storage unavailable */ }

function showSoundState() {
    soundButton.textContent = soundOn ? '🔊' : '🔇';
    soundButton.setAttribute('aria-pressed', String(soundOn));
    soundButton.title = soundOn ? 'Sound on' : 'Sound off';
}
showSoundState();

soundButton.addEventListener('click', () => {
    soundOn = !soundOn;
    try { localStorage.setItem('zooSound', soundOn ? 'on' : 'off'); } catch { /* ignore */ }
    showSoundState();
});

// Browsers only allow sound after the user has interacted with the page,
// so the audio context is created on the first click or key press
function unlockAudio() {
    try {
        const Ctx = window.AudioContext || window.webkitAudioContext;
        if (!Ctx) return;
        audioCtx ??= new Ctx();
        if (audioCtx.state === 'suspended') audioCtx.resume();
    } catch (error) {
        console.warn('Audio is unavailable', error);
    }
}
document.addEventListener('pointerdown', unlockAudio, { once: true });
document.addEventListener('keydown', unlockAudio, { once: true });

function bell(freq, start, duration, volume) {
    const tone = audioCtx.createOscillator();
    const overtone = audioCtx.createOscillator();
    const overtoneLevel = audioCtx.createGain();
    const envelope = audioCtx.createGain();

    tone.type = 'sine';
    tone.frequency.value = freq;
    overtone.type = 'sine';
    overtone.frequency.value = freq * 2.4; // slightly off-harmonic, which gives a bell-like colour
    overtoneLevel.gain.value = 0.25;

    envelope.gain.setValueAtTime(0.0001, start);
    envelope.gain.exponentialRampToValueAtTime(volume, start + 0.03);
    envelope.gain.exponentialRampToValueAtTime(0.0001, start + duration);

    tone.connect(envelope);
    overtone.connect(overtoneLevel).connect(envelope);
    envelope.connect(audioCtx.destination);

    for (const osc of [tone, overtone]) {
        osc.start(start);
        osc.stop(start + duration + 0.1);
    }
}

function playFuneral() {

    if (!soundOn || !audioCtx || audioCtx.state !== 'running') return;

    const now = audioCtx.currentTime;
    // Three slow tolls, then a falling phrase
    const notes = [196, 196, 196, 233.08, 220, 196];
    notes.forEach((freq, i) => bell(freq, now + i * 0.9, 1.8, 0.18));
}

function floatTombstone() {
    const stone = document.createElement('div');
    stone.className = 'tombstone';
    stone.textContent = '🪦';
    stone.setAttribute('aria-hidden', 'true');
    document.body.appendChild(stone);
    setTimeout(() => stone.remove(), 7500);
}

function playGameOver() {
    if (gameOverBusy) return; // ignore repeat clicks while it is playing
    gameOverBusy = true;
    setTimeout(() => (gameOverBusy = false), 7500);

    // The effects are extras: if either fails, the app carries on without it
    try { floatTombstone(); } catch (error) { console.warn('Tombstone animation failed', error); }
    try { playFuneral(); } catch (error) { console.warn('Funeral sound unavailable', error); }
}

// Each habitat section declares its type and danger threshold in the HTML
const habitats = Object.fromEntries(
    [...document.querySelectorAll('.habitat')].map(section => [
        section.dataset.type,
        {
            container: section.querySelector('.cards'),
            button: section.querySelector('.feed-type'),
            threshold: Number(section.dataset.threshold)
        }
    ])
);

// Cards are created once and then updated, so the health bars animate smoothly
const cards = new Map();

const cardKey = animal => `${animal.type}:${animal.name}`;

function createCard(animal) {
    const { container, threshold } = habitats[animal.type];

    const el = document.createElement('article');
    el.className = 'card';
    el.innerHTML = `
    <div class="card-head">
      <span class="avatar" aria-hidden="true"></span>
      <span class="name"></span>
      <span class="badge"></span>
    </div>
    <div class="bar" role="progressbar" aria-valuemin="0" aria-valuemax="100">
      <div class="fill"></div>
      <div class="marker"></div>
    </div>
    <span class="percent"></span>
    <p class="warning" hidden>⚠️ Cannot walk. Feed now or risk death!</p>`;

    el.querySelector('.name').textContent = animal.name;
    const marker = el.querySelector('.marker');
    marker.style.left = threshold + '%';
    marker.title = `Danger level: ${threshold}%`;

    container.appendChild(el);
    return el;
}

function updateCard(el, animal) {
    const state = !animal.isAlive ? 'dead' : animal.canWalk ? 'ok' : 'warn';
    const label = { dead: 'Dead', ok: 'Alive', warn: 'Cannot walk' }[state];

    el.classList.toggle('dead', !animal.isAlive);
    el.querySelector('.avatar').textContent = animal.isAlive ? EMOJI[animal.type] : '🪦';

    const badge = el.querySelector('.badge');
    badge.textContent = label;
    badge.className = 'badge ' + state;

    const atRisk = animal.isAlive && !animal.canWalk;
    el.classList.toggle('at-risk', atRisk);
    el.querySelector('.warning').hidden = !atRisk;

    const fill = el.querySelector('.fill');
    fill.style.width = animal.health + '%';
    // Hue 120 is green, 60 is yellow, 0 is red
    fill.style.backgroundColor = `hsl(${Math.round(animal.health * 1.2)} 65% 45%)`;

    const bar = el.querySelector('.bar');
    bar.setAttribute('aria-label', `${animal.name} health`);
    bar.setAttribute('aria-valuenow', animal.health.toFixed(0));

    el.querySelector('.percent').textContent = animal.health.toFixed(2) + '% health';
}

function render(status) {
    const time = new Date(status.time);
    document.getElementById('zoo-time').textContent =
        time.toLocaleString([], { dateStyle: 'medium', timeStyle: 'short' });

    const hour = time.getHours();
    document.getElementById('sky').textContent = hour >= 6 && hour < 18 ? '☀️' : '🌙';

    const alive = status.animals.filter(a => a.isAlive).length;
    document.getElementById('alive-count').textContent =
        `${alive} of ${status.animals.length} alive`;

    const allDead = status.animals.every(a => !a.isAlive);
    if (allDead && lastAllDead === false) playGameOver();
    lastAllDead = allDead;

    for (const [type, habitat] of Object.entries(habitats)) {
        habitat.button.disabled = !status.animals.some(a => a.type === type && a.isAlive);
    }

    const atRiskCount = status.animals.filter(a => a.isAlive && !a.canWalk).length;
    const banner = document.getElementById('risk-banner');
    const bannerText = atRiskCount === 0
        ? ''
        : `⚠️ ${atRiskCount} ${atRiskCount === 1 ? 'animal cannot' : 'animals cannot'} walk. Feed now or risk death!`;

    if (banner.textContent !== bannerText) banner.textContent = bannerText;
    banner.hidden = atRiskCount === 0;

    const current = new Set(status.animals.map(cardKey));

    // Remove cards for animals that no longer exist (after a reset)
    for (const [key, el] of cards) {
        if (!current.has(key)) {
            el.remove();
            cards.delete(key);
        }
    }

    for (const animal of status.animals) {
        const key = cardKey(animal);
        let el = cards.get(key);
        if (!el) {
            el = createCard(animal);
            cards.set(key, el);
        }
        updateCard(el, animal);
    }
}

async function refresh() {
    const error = document.getElementById('error');
    try {
        const response = await fetch('/api/zoo');
        if (!response.ok) throw new Error(response.status);
        render(await response.json());
        error.hidden = true;
    } catch {
        error.hidden = false;
    }
}

let messageTimer;

document.getElementById('feed-button').addEventListener('click', async event => {
    const button = event.currentTarget;
    const message = document.getElementById('feed-message');
    button.disabled = true;

    try {
        const response = await fetch('/api/zoo/feed', { method: 'POST' });
        if (!response.ok) throw new Error(response.status);
        const rolls = await response.json();

        if (lastAllDead) {
            message.textContent = 'Too late, there is nobody left to feed. Press Reset zoo to start again.';
            playGameOver();
        } else {
            message.textContent =
                `Fed! Monkeys +${rolls.monkeyPercent.toFixed(2)}%, ` +
                `Giraffes +${rolls.giraffePercent.toFixed(2)}%, ` +
                `Elephants +${rolls.elephantPercent.toFixed(2)}%`;
        }
        refresh();
    } catch {
        message.textContent = 'Feeding failed, please try again.';
    } finally {
        button.disabled = false;
        clearTimeout(messageTimer);
        messageTimer = setTimeout(() => (message.textContent = ''), 8000);
    }
});

document.getElementById('reset-button').addEventListener('click', async event => {
    if (!confirm('Reset the zoo? All animals will return to full health.')) return;

    const button = event.currentTarget;
    const message = document.getElementById('feed-message');
    button.disabled = true;

    try {
        const response = await fetch('/api/zoo/reset', { method: 'POST' });
        if (!response.ok) throw new Error(response.status);
        message.textContent = 'The zoo has been reset.';
        refresh();
    } catch {
        message.textContent = 'Reset failed, please try again.';
    } finally {
        button.disabled = false;
        clearTimeout(messageTimer);
        messageTimer = setTimeout(() => (message.textContent = ''), 8000);
    }
});

refresh();

for (const button of document.querySelectorAll('.feed-type')) {
    button.addEventListener('click', async () => {
        const message = document.getElementById('feed-message');
        const type = button.dataset.type;
        button.disabled = true;

        try {
            const response = await fetch(`/api/zoo/feed/${type}`, { method: 'POST' });
            if (!response.ok) throw new Error(response.status);
            const result = await response.json();
            message.textContent = `Fed the ${type.toLowerCase()}s! +${result.percent.toFixed(2)}%`;
            refresh();
        } catch {
            message.textContent = 'Feeding failed, please try again.';
        } finally {
            clearTimeout(messageTimer);
            messageTimer = setTimeout(() => (message.textContent = ''), 8000);
            // render() decides whether the button should stay disabled on the next refresh
            button.disabled = false;
        }
    });
}

setInterval(refresh, POLL_MS);