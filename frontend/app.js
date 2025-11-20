const API_BASE_URL = 'http://localhost:5159/api';

// Elements
const cardStack = document.getElementById('cardStack');
const statusEl = document.getElementById('status');
const emptyState = document.getElementById('emptyState');
const userIdInput = document.getElementById('userIdInput');
const reloadButton = document.getElementById('reloadButton');
const resetButton = document.getElementById('resetButton');
const likeButton = document.getElementById('likeButton');
const skipButton = document.getElementById('skipButton');
const registerForm = document.getElementById('registerForm');
const loginForm = document.getElementById('loginForm');
const loginEmailInput = document.getElementById('loginEmail');
const loginUserIdInput = document.getElementById('loginUserId');
const matchesList = document.getElementById('matchesList');
const messagesList = document.getElementById('messagesList');
const messageForm = document.getElementById('messageForm');
const messageInput = document.getElementById('messageInput');
const activeMatchTitle = document.getElementById('activeMatchTitle');

const state = {
  rooms: [],
  userId: Number(userIdInput.value) || 1,
  currentIndex: 0,
  users: [],
  matches: [],
  messages: [],
  activeMatchId: null,
};

reloadButton.addEventListener('click', () => {
  loadRooms();
  loadMatches();
});

resetButton.addEventListener('click', () => {
  state.currentIndex = 0;
  renderCards();
});

userIdInput.addEventListener('change', () => {
  const newId = Math.max(1, Number(userIdInput.value) || 1);
  setActiveUser(newId);
});

likeButton.addEventListener('click', () => swipeCurrent(true));
skipButton.addEventListener('click', () => swipeCurrent(false));

registerForm.addEventListener('submit', async (event) => {
  event.preventDefault();
  const data = new FormData(registerForm);
  const payload = {
    id: 0,
    firstName: data.get('firstName'),
    lastName: data.get('lastName'),
    email: data.get('email'),
    phone: data.get('phone') || '',
    bio: data.get('bio') || '',
    birthDate: data.get('birthDate') || new Date().toISOString(),
    gender: data.get('gender') || '',
    isSmoker: false,
    hasPets: false,
    createdAt: new Date().toISOString(),
  };

  try {
    const response = await fetch(`${API_BASE_URL}/User`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload),
    });

    if (!response.ok) throw new Error('Failed to register');
    setStatus('Registered successfully. Loading your profile…');
    await loadUsers();
    const created = state.users.find((u) => u.email === payload.email);
    if (created) {
      setActiveUser(created.id);
      userIdInput.value = created.id;
    }
  } catch (error) {
    console.warn('Registration failed, using demo account.', error);
    setStatus('Could not reach API to register — using demo user.');
    const demoUser = state.users[0] || sampleUsers()[0];
    setActiveUser(demoUser.id);
  }
});

loginForm.addEventListener('submit', async (event) => {
  event.preventDefault();
  await loadUsers();
  const email = loginEmailInput.value.trim().toLowerCase();
  const userId = Number(loginUserIdInput.value);
  const found = state.users.find(
    (u) => (email && u.email?.toLowerCase() === email) || (userId && u.id === userId),
  );

  if (found) {
    setActiveUser(found.id);
    userIdInput.value = found.id;
    setStatus(`Logged in as ${found.firstName || 'user'} (#${found.id}).`);
  } else {
    setStatus('User not found. Double-check your email or ID.');
  }
});

messageForm.addEventListener('submit', async (event) => {
  event.preventDefault();
  if (!state.activeMatchId) return;
  const text = messageInput.value.trim();
  if (!text) return;

  const payload = {
    id: 0,
    matchId: state.activeMatchId,
    fromUserId: state.userId,
    messageText: text,
    sentAt: new Date().toISOString(),
  };

  try {
    const response = await fetch(`${API_BASE_URL}/Message`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload),
    });

    if (!response.ok) throw new Error('Send failed');
    messageInput.value = '';
    await loadMessages(state.activeMatchId);
    setStatus('Message sent.');
  } catch (error) {
    console.warn('Message send failed', error);
    setStatus('Could not reach API to send message.');
    state.messages.push(payload);
    renderMessages();
  }
});

function setStatus(text) {
  statusEl.textContent = text;
}

async function loadUsers() {
  try {
    const res = await fetch(`${API_BASE_URL}/User`);
    if (!res.ok) throw new Error('API not reachable');
    state.users = await res.json();
  } catch (error) {
    console.warn('Falling back to sample users:', error);
    state.users = sampleUsers();
  }
}

async function loadRooms() {
  setStatus('Loading rooms from API…');
  try {
    const [roomsRes, photosRes] = await Promise.all([
      fetch(`${API_BASE_URL}/Room`),
      fetch(`${API_BASE_URL}/RoomPhoto`),
    ]);

    if (!roomsRes.ok || !photosRes.ok) {
      throw new Error('API not reachable');
    }

    const rooms = await roomsRes.json();
    const photos = await photosRes.json();
    const photosByRoom = photos.reduce((map, photo) => {
      map[photo.roomId] = map[photo.roomId] || [];
      map[photo.roomId].push(photo);
      return map;
    }, {});

    state.rooms = rooms.map((room) => {
      const roomPhotos = photosByRoom[room.id] || [];
      roomPhotos.sort((a, b) => (a.position ?? 0) - (b.position ?? 0));
      return {
        ...room,
        photoUrl:
          roomPhotos[0]?.url ||
          `https://images.unsplash.com/photo-1505693416388-ac5ce068fe85?auto=format&fit=crop&w=1200&q=80&sig=${room.id}`,
      };
    });

    state.currentIndex = 0;
    renderCards();
    setStatus(`Loaded ${state.rooms.length} room${state.rooms.length === 1 ? '' : 's'} from API.`);
  } catch (error) {
    console.warn('Falling back to sample data:', error);
    state.rooms = sampleRooms();
    state.currentIndex = 0;
    renderCards();
    setStatus('Could not reach API — showing demo rooms.');
  }
}

async function loadMatches() {
  if (!state.userId) return;
  try {
    const res = await fetch(`${API_BASE_URL}/Match/user/${state.userId}`);
    if (!res.ok) throw new Error('API not reachable');
    state.matches = await res.json();
    setStatus(`Loaded ${state.matches.length} match${state.matches.length === 1 ? '' : 'es'}.`);
  } catch (error) {
    console.warn('Falling back to sample matches:', error);
    state.matches = sampleMatches(state.userId);
    setStatus('Could not reach API — showing demo matches.');
  }
  renderMatches();
  if (state.matches.length) {
    setActiveMatch(state.matches[0].id);
  } else {
    state.activeMatchId = null;
    state.messages = [];
    renderMessages();
  }
}

async function loadMessages(matchId) {
  try {
    const res = await fetch(`${API_BASE_URL}/Message/match/${matchId}`);
    if (!res.ok) throw new Error('API not reachable');
    state.messages = await res.json();
  } catch (error) {
    console.warn('Falling back to sample messages:', error);
    state.messages = sampleMessages(matchId, state.userId);
  }
  renderMessages();
}

function sampleUsers() {
  return [
    { id: 1, firstName: 'Alex', lastName: 'Kim', email: 'alex@example.com' },
    { id: 2, firstName: 'Riley', lastName: 'Chen', email: 'riley@example.com' },
    { id: 3, firstName: 'Sam', lastName: 'Patel', email: 'sam@example.com' },
  ];
}

function sampleRooms() {
  return [
    {
      id: 100,
      title: 'Sunny loft near university',
      city: 'Barcelona',
      address: 'Carrer de Mallorca 43',
      price: 620,
      squareMeters: 28,
      description: 'Cozy loft with lots of natural light, perfect for students.',
      availabilityDate: '2024-07-01',
      photoUrl:
        'https://images.unsplash.com/photo-1505692069463-5e3405e3e7ee?auto=format&fit=crop&w=1200&q=80',
    },
    {
      id: 101,
      title: 'Minimalist studio with skyline view',
      city: 'Chicago',
      address: 'Michigan Ave 350',
      price: 980,
      squareMeters: 34,
      description: 'Floor-to-ceiling windows, fast wifi, and in-building gym access.',
      availabilityDate: '2024-08-10',
      photoUrl:
        'https://images.unsplash.com/photo-1505691938895-1758d7feb511?auto=format&fit=crop&w=1200&q=80',
    },
    {
      id: 102,
      title: 'Garden-level room in shared house',
      city: 'Portland',
      address: 'SE Hawthorne Blvd',
      price: 540,
      squareMeters: 20,
      description: 'Shared kitchen, pets allowed, steps away from local cafes.',
      availabilityDate: '2024-06-18',
      photoUrl:
        'https://images.unsplash.com/photo-1460317442991-0ec209397118?auto=format&fit=crop&w=1200&q=80',
    },
  ];
}

function sampleMatches(userId) {
  const otherUserId = userId === 1 ? 2 : 1;
  return [
    { id: 501, user1Id: userId, user2Id: otherUserId, roomId: 100, matchedAt: new Date().toISOString() },
    { id: 502, user1Id: userId, user2Id: 3, roomId: 101, matchedAt: new Date().toISOString() },
  ];
}

function sampleMessages(matchId, userId) {
  return [
    { id: 9001, matchId, fromUserId: userId, messageText: 'Hey! When can we tour?', sentAt: new Date().toISOString() },
    { id: 9002, matchId, fromUserId: 2, messageText: 'Tomorrow evening works for me.', sentAt: new Date().toISOString() },
  ];
}

function renderCards() {
  cardStack.innerHTML = '';
  emptyState.classList.toggle('hidden', state.rooms.length > state.currentIndex);

  const activeRooms = state.rooms.slice(state.currentIndex);

  if (!activeRooms.length) return;

  activeRooms.forEach((room, idx) => {
    const card = createCard(room, idx);
    cardStack.appendChild(card);
  });
}

function createCard(room, orderIndex) {
  const card = document.createElement('article');
  card.className = 'room-card';
  card.style.backgroundImage = `url(${room.photoUrl})`;
  card.style.zIndex = 100 - orderIndex;

  card.innerHTML = `
    <div class="meta">
      <span class="badge">${room.city}</span>
      <h2>${room.title}</h2>
      <div class="chips">
        <span class="chip">$${room.price}/mo</span>
        <span class="chip">${room.squareMeters || '?'} m²</span>
        <span class="chip">Available ${formatDate(room.availabilityDate)}</span>
      </div>
      <p>${room.description || 'No description provided.'}</p>
    </div>
  `;

  setupSwipe(card, room);
  return card;
}

function formatDate(input) {
  if (!input) return 'soon';
  const date = new Date(input);
  if (Number.isNaN(date.getTime())) return 'soon';
  return date.toLocaleDateString(undefined, { month: 'short', day: 'numeric' });
}

function setupSwipe(card, room) {
  let startX = 0;
  let startY = 0;
  let currentX = 0;
  let currentY = 0;
  let pointerId = null;
  const threshold = 120;

  const handleDown = (event) => {
    pointerId = event.pointerId;
    startX = event.clientX;
    startY = event.clientY;
    card.setPointerCapture(pointerId);
  };

  const handleMove = (event) => {
    if (pointerId !== event.pointerId) return;
    currentX = event.clientX - startX;
    currentY = event.clientY - startY;
    card.style.transform = `translate(${currentX}px, ${currentY}px) rotate(${currentX / 15}deg)`;
  };

  const handleUp = (event) => {
    if (pointerId !== event.pointerId) return;
    const swipedRight = currentX > threshold;
    const swipedLeft = currentX < -threshold;

    card.releasePointerCapture(pointerId);
    pointerId = null;

    if (swipedRight || swipedLeft) {
      const isLiked = swipedRight;
      animateSwipe(card, isLiked);
      handleSwipe(room, isLiked);
    } else {
      card.style.transition = 'transform 180ms ease';
      card.style.transform = 'translate(0, 0) rotate(0deg)';
      setTimeout(() => (card.style.transition = ''), 200);
    }

    currentX = 0;
    currentY = 0;
  };

  card.addEventListener('pointerdown', handleDown);
  card.addEventListener('pointermove', handleMove);
  card.addEventListener('pointerup', handleUp);
  card.addEventListener('pointercancel', handleUp);
}

function animateSwipe(card, isLiked) {
  const direction = isLiked ? 1 : -1;
  card.style.transition = 'transform 240ms ease, opacity 240ms ease';
  card.style.transform = `translate(${direction * 520}px, 30px) rotate(${direction * 18}deg)`;
  card.style.opacity = '0';
}

function handleSwipe(room, isLiked) {
  state.currentIndex += 1;
  cardStack.firstChild?.remove();
  emptyState.classList.toggle('hidden', state.rooms.length > state.currentIndex);
  sendSwipe(room, isLiked);
}

async function sendSwipe(room, isLiked) {
  const payload = {
    id: 0,
    userId: state.userId,
    roomId: room.id,
    isLiked,
  };

  try {
    const response = await fetch(`${API_BASE_URL}/RoomSwipe`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    });

    if (!response.ok) {
      throw new Error('Failed to register swipe');
    }
    setStatus(`Recorded ${isLiked ? 'like' : 'skip'} for room ${room.id}.`);
    await loadMatches();
  } catch (error) {
    setStatus('Could not reach API to save swipe (check server).');
  }
}

function swipeCurrent(isLiked) {
  const cards = Array.from(cardStack.children);
  const topCard = cards.at(-1);
  if (!topCard) return;

  animateSwipe(topCard, isLiked);
  const room = state.rooms[state.currentIndex];
  handleSwipe(room, isLiked);
}

function renderMatches() {
  matchesList.innerHTML = '';
  if (!state.matches.length) {
    matchesList.innerHTML = '<li class="muted">No matches yet.</li>';
    return;
  }

  state.matches.forEach((match) => {
    const item = document.createElement('li');
    item.className = state.activeMatchId === match.id ? 'active' : '';
    const otherUserId = match.user1Id === state.userId ? match.user2Id : match.user1Id;
    item.innerHTML = `
      <div class="match-title">Match ${match.id}</div>
      <div class="muted">Room #${match.roomId} • with user #${otherUserId}</div>
    `;
    item.addEventListener('click', () => setActiveMatch(match.id));
    matchesList.appendChild(item);
  });
}

async function setActiveMatch(matchId) {
  state.activeMatchId = matchId;
  const match = state.matches.find((m) => m.id === matchId);
  if (match) {
    activeMatchTitle.textContent = `Match #${match.id}`;
    await loadMessages(matchId);
  }
  renderMatches();
}

function renderMessages() {
  messagesList.innerHTML = '';
  if (!state.messages.length) {
    messagesList.innerHTML = '<li class="muted">No messages yet.</li>';
    return;
  }

  state.messages
    .slice()
    .sort((a, b) => new Date(a.sentAt) - new Date(b.sentAt))
    .forEach((msg) => {
      const li = document.createElement('li');
      li.className = msg.fromUserId === state.userId ? 'from-self' : 'from-them';
      li.innerHTML = `
        <span class="sender">User #${msg.fromUserId}</span>
        <p>${msg.messageText}</p>
        <span class="timestamp">${formatDateTime(msg.sentAt)}</span>
      `;
      messagesList.appendChild(li);
    });
}

function formatDateTime(input) {
  const date = new Date(input);
  if (Number.isNaN(date.getTime())) return '';
  return `${date.toLocaleDateString()} ${date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}`;
}

function setActiveUser(userId) {
  state.userId = userId;
  setStatus(`Active user set to #${userId}.`);
  loadRooms();
  loadMatches();
}

(async function init() {
  await loadUsers();
  await loadRooms();
  await loadMatches();
})();
