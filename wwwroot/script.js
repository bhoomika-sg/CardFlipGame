let lock = false;
let flippedCards = 0;

async function startGame() {
    await fetch('/api/game/start', { method: 'POST' });
    await loadBoard();
}

async function loadBoard() {
    const res = await fetch('/api/game/board');
    const data = await res.json();
    const board = data.board;
    const boardDiv = document.getElementById('game-board');
    boardDiv.innerHTML = '';

    const size = board.length;
    boardDiv.style.gridTemplateColumns = `repeat(${size}, 80px)`;

    board.forEach((row, i) => {
        row.forEach((cell, j) => {
            const card = document.createElement('div');
            card.className = 'card';
            if (cell.visible) card.classList.add('flipped');

            const inner = document.createElement('div');
            inner.className = 'card-inner';

            const front = document.createElement('div');
            front.className = 'card-front';
            front.innerText = cell.value;

            const back = document.createElement('div');
            back.className = 'card-back';
            back.innerText = '🎴';

            inner.appendChild(front);
            inner.appendChild(back);
            card.appendChild(inner);

            if (cell.value !== '?') {
                card.onclick = () => flipCard(i, j);
            } else {
                card.style.visibility = "hidden";
            }

            boardDiv.appendChild(card);
        });
    });

    document.getElementById('score').innerText = `Score: ${data.score}`;
    document.getElementById('level').innerText = `Level: ${data.level}`;
    document.getElementById('lives').innerText = `Lives: ${10 - data.wrongMoves}`;

    if (data.gameOver) {
        alert("😢 Game Over! Restarting...");
        await startGame();
    }
}

async function flipCard(row, col) {
    if (lock) return;
    lock = true;

    await fetch('/api/game/flip', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ row, col })
    });

    flippedCards++;

    await loadBoard();

    if (flippedCards === 2) {
        flippedCards = 0;
        setTimeout(async () => {
            await fetch('/api/game/hide', { method: 'POST' });
            await loadBoard();
            lock = false;
        }, 700);
    } else {
        lock = false;
    }
}

startGame();
