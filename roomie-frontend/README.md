# Roomie Frontend (Angular)

This frontend is designed to run as a standard Angular CLI project. The `src/` folder already contains the Roomie pages and services; you just need the usual Angular workspace files (package.json, angular.json, tsconfig*, etc.).

## Prerequisites
- Node.js 18+ and npm
- Angular CLI installed globally (optional but recommended):
  ```bash
  npm install -g @angular/cli
  ```

## First-time setup
1. If you do **not** yet have the Angular workspace files in this folder, scaffold them (keeps existing `src/`):
   ```bash
   cd roomie-frontend
   ng new . --routing --style=css --skip-install
   ```
   When prompted about overwriting files, choose **No** for the existing `src/` content so the provided components stay intact.

2. Install dependencies (creates `node_modules/`):
   ```bash
   npm install
   ```

3. Set the backend API URL in `src/environments/environment.ts` if your port differs from the default:
   ```typescript
   export const environment = {
     production: false,
     apiUrl: 'http://localhost:5159'
   };
   ```

## Run the dev server
From the `roomie-frontend` directory:
```bash
npm start
# or
ng serve --open
```
The app will be available at `http://localhost:4200/`.

## Build for production
```bash
npm run build
```
The compiled assets will be emitted to `dist/` (Angular default).
