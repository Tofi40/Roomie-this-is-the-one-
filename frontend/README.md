# Roomie Swipe frontend

A lightweight, framework-free UI that lets users browse rooms, swipe left/right to skip or like, sign in, and chat with matches.
It talks to the existing RoomieSystem API or falls back to demo data when the API is not reachable.

## Running the frontend
1. From the `frontend` directory, start a simple static server (for example with Python):
   ```bash
   python -m http.server 8000
   ```
2. Open http://localhost:8000 in your browser.

## How it works
- Cards are loaded from the API endpoints `GET /api/Room` and `GET /api/RoomPhoto`. If those calls fail, three demo cards are shown instead.
- Swipes are registered with `POST /api/RoomSwipe` using the active user id you enter in the header.
- Clicking the like/skip buttons triggers the same animation as dragging the card left or right.
- You can create an account with `POST /api/User` or sign in by email/id after fetching `GET /api/User`.
- Matches are loaded from `GET /api/Match/user/{userId}`, and messages are loaded with `GET /api/Message/match/{matchId}` and posted via `POST /api/Message`.

Update `API_BASE_URL` in `app.js` if your API runs on a different port.
